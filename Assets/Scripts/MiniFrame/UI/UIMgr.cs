
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}

/// <summary>
/// UI管理器
/// </summary>
public class UIMgr : Singleton<UIMgr>
{
    //保存所有面板
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //面板父类
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    //UI的Canvas父对象
    public RectTransform canvas;

    public UIMgr()
    {
        //创建Canvas 让其过场景的时候 不被移除
        ResMgr.Instance.LoadAssetAsync<GameObject>("Canvas", (handle) =>
        {
            GameObject go = GameObject.Instantiate(handle.Result);
            canvas = go.transform as RectTransform;
            GameObject.DontDestroyOnLoad(go);
        });

        //找到各层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");
    }

    /// <summary>
    /// 通过层级枚举 得到对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    {
        //判断面板是否存在
        if (panelDic.ContainsKey(panelName))
        {
            //显示面板
            panelDic[panelName].ShowMe();

            // 面板创建完成
            callBack?.Invoke(panelDic[panelName] as T);
        }
        else
        {

            ResMgr.Instance.LoadAssetAsync<GameObject>(panelName, (handle) =>
            {
                GameObject obj = GameObject.Instantiate(handle.Result);

                //设置面板父物体
                Transform father = bot;
                switch (layer)
                {
                    case E_UI_Layer.Mid:
                        father = mid;
                        break;
                    case E_UI_Layer.Top:
                        father = top;
                        break;
                    case E_UI_Layer.System:
                        father = system;
                        break;
                }
                obj.transform.SetParent(father);

                //设置面板位置和缩放
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;

                (obj.transform as RectTransform).offsetMax = Vector2.zero;
                (obj.transform as RectTransform).offsetMin = Vector2.zero;

                //得到预设体身上的面板脚本
                T panel = obj.GetComponent<T>();

                // 面板创建完成
                callBack?.Invoke(panel);

                //显示面板
                panel.ShowMe();

                //把面板存起来
                panelDic.Add(panelName, panel);
            });
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
