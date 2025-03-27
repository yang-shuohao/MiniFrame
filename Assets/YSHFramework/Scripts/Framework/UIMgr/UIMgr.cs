
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YSH.Framework.Exceptions;
using YSH.Framework.Utils;

namespace YSH.Framework
{
    public enum UILayerType
    {
        Bot,
        Low,
        Mid,
        High,
        Top,
        System,
    }

    public class UIMgr : Singleton<UIMgr>
    {
        //保存所有面板
        public Dictionary<string, BaseUI> panelDic = new Dictionary<string, BaseUI>();

        //UI的Canvas父对象
        public RectTransform canvasRT;
        public Canvas canvas;
        public Transform worldCanvasTrans;
        public Canvas worldCanvas;

        //层
        private Transform[] canvasLayers;
        private Transform[] worldCanvasLayers;

        public UIMgr()
        {
            InitUIObject();
        }

        //生成UI对象
        public void InitUIObject()
        {
            //创建UI根物体
            GameObject uiObj = new GameObject("UI");
            GameObject.DontDestroyOnLoad(uiObj);

            //UI摄像机
            GameObject cameraObj = new GameObject("UICamera");
            canvasRT = cameraObj.transform as RectTransform;
            Camera uiCamera = cameraObj.AddComponent<Camera>();
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = LayerMask.GetMask("UI");
            uiCamera.orthographic = true;
            cameraObj.transform.SetParent(uiObj.transform);

            //创建Canvas
            GameObject canvasObj = new GameObject("Canvas");
            canvasObj.layer = LayerMask.NameToLayer("UI");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            if (SortingLayerUtils.IsContains("UI"))
            {
                canvas.sortingLayerName = "UI";
            }
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0f;
            canvasObj.AddComponent<GraphicRaycaster>();
            canvasObj.transform.SetParent(uiObj.transform);

            //创建Canvas中的层
            UILayerType[] layerTypes = (UILayerType[])System.Enum.GetValues(typeof(UILayerType));
            canvasLayers = new Transform[layerTypes.Length];
            for (int i = 0; i < layerTypes.Length; i++)
            {
                GameObject layerObj = new GameObject(layerTypes[i].ToString());
                layerObj.layer = LayerMask.NameToLayer("UI");
                layerObj.transform.SetParent(canvasObj.transform);
                RectTransform layerRT = layerObj.AddComponent<RectTransform>();
                layerRT.anchorMin = Vector2.zero;
                layerRT.anchorMax = Vector2.one;
                layerRT.offsetMin = Vector2.zero;
                layerRT.offsetMax = Vector2.zero;
                layerRT.localPosition = Vector3.zero;
                layerRT.localScale = Vector3.one;
                canvasLayers[i] = layerObj.transform;
            }

            //创建WorldCanvas
            GameObject worldCanvasObj = new GameObject("WorldCanvas");
            worldCanvasObj.layer = LayerMask.NameToLayer("UI");
            worldCanvas = worldCanvasObj.AddComponent<Canvas>();
            worldCanvas.renderMode = RenderMode.WorldSpace;
            worldCanvas.worldCamera = uiCamera;
            if (SortingLayerUtils.IsContains("WorldUI"))
            {
                canvas.sortingLayerName = "WorldUI";
            }
            worldCanvasObj.AddComponent<CanvasScaler>();
            worldCanvasObj.AddComponent<GraphicRaycaster>();
            worldCanvasObj.transform.SetParent(uiObj.transform);

            //创建Canvas中的层
            worldCanvasLayers = new Transform[layerTypes.Length];
            for (int i = 0; i < layerTypes.Length; i++)
            {
                GameObject layerObj = new GameObject(layerTypes[i].ToString());
                layerObj.transform.SetParent(worldCanvasObj.transform);
                RectTransform layerRT = layerObj.AddComponent<RectTransform>();
                layerRT.anchorMin = Vector2.zero;
                layerRT.anchorMax = Vector2.one;
                layerRT.offsetMin = Vector2.zero;
                layerRT.offsetMax = Vector2.zero;
                layerRT.localPosition = Vector3.zero;
                layerRT.localScale = Vector3.one;
                worldCanvasLayers[i] = layerObj.transform;
            }

            //添加EventSystem
            GameObject eventSystemObj = new GameObject(typeof(EventSystem).Name);
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
            eventSystemObj.transform.SetParent(uiObj.transform);

        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public void ShowPanel<T>(string panelName, UILayerType layer = UILayerType.Mid, UnityAction<T> callBack = null) where T : BaseUI
        {
            if (panelDic.ContainsKey(panelName))
            {
                panelDic[panelName].transform.SetAsLastSibling();

                panelDic[panelName].gameObject.SetActive(true);

                // 面板创建完成
                callBack?.Invoke(panelDic[panelName] as T);
            }
            else
            {
                //加载面板
                ResMgr.Instance.LoadAssetAsync<GameObject>(panelName, ResMgr.Instance.resLoadType, result =>
                {
                    if (!panelDic.ContainsKey(panelName))
                    {
                        // 实例化对象并直接设置父物体
                        Transform father = canvasLayers[(int)layer];
                        GameObject obj = GameObject.Instantiate(result, father, false);
                        obj.name = result.name;

                        //得到预设体身上的面板脚本
                        T panel = obj.GetComponent<T>();

                        // 面板创建完成
                        callBack?.Invoke(panel);

                        //把面板存起来
                        panelDic.Add(panelName, panel);
                    }
                });
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        /// <param name="panelName"></param>
        public void DestroyPanel(string panelName)
        {
            if (panelDic.ContainsKey(panelName))
            {
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
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
                panelDic[panelName].gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///  隐藏所有面板
        /// </summary>
        /// <param name="exceptPanels">不关闭的面板</param>
        public void HideAllPanel(params string[] exceptPanels)
        {
            foreach (var panel in panelDic)
            {
                if (Array.Exists(exceptPanels, name => name == panel.Key))
                    continue;

                HidePanel(panel.Key);
            }
        }

        /// <summary>
        /// 得到某一个已经显示的面板
        /// </summary>
        public T GetPanel<T>(string name) where T : BaseUI
        {
            if (panelDic.ContainsKey(name))
            {
                return panelDic[name] as T;
            }

            throw new PanelNotFoundException();
        }

        //显示普通UI
        public void ShowUI(Transform uiTransform, UILayerType layer = UILayerType.Mid)
        {
            Transform father = canvasLayers[(int)layer];
            uiTransform.SetParent(father, false);
        }

        //显示世界UI
        public void ShowWorldUI(Transform uiTransform, UILayerType layer = UILayerType.Mid)
        {
            Vector3 scale = uiTransform.localScale;
            uiTransform.SetParent(worldCanvasLayers[(int)layer], false);
            uiTransform.localScale = scale;
        }

        /// <summary>
        /// 给控件添加自定义事件监听
        /// </summary>
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
}

