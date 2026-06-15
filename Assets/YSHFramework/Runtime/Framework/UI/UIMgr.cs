using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using YSH.Framework.Exceptions;

namespace YSH.Framework
{
    public enum UILayerType
    {
        Background,
        Normal,
        Popup,
        Top,
        System,
    }

    public enum UICanvasType
    {
        Overlay,
        Camera,
        World
    }

    public class UIMgr : MonoSingleton<UIMgr>
    {
        public EventSystem CurEventSystem { get; private set; }
        public Camera UICamera { get; private set; }

        //保存所有UI
        private Dictionary<string, BaseUI> uiDic = new Dictionary<string, BaseUI>();

        //Canvas
        private Canvas overlayCanvas;
        private Canvas cameraCanvas;
        private Canvas worldCanvas;

        //层
        private Dictionary<UICanvasType, Transform[]> canvasLayersDic = new Dictionary<UICanvasType, Transform[]>(3);

        private void Awake()
        {
            CurEventSystem = transform.GetComponentInChildren<EventSystem>();
            UICamera = transform.GetComponentInChildren<Camera>();

            overlayCanvas = transform.Find("OverlayCanvas").GetComponent<Canvas>();
            cameraCanvas = transform.Find("CameraCanvas").GetComponent<Canvas>();
            worldCanvas = transform.Find("WorldCanvas").GetComponent<Canvas>();

            canvasLayersDic[UICanvasType.Overlay] = overlayCanvas.transform.GetComponentsInChildren<Transform>();
            canvasLayersDic[UICanvasType.Camera] = cameraCanvas.transform.GetComponentsInChildren<Transform>();
            canvasLayersDic[UICanvasType.World] = worldCanvas.transform.GetComponentsInChildren<Transform>();
        }

        /// <summary>
        /// 显示UI
        /// </summary>
        public void ShowUI<T>(string uiName, UICanvasType uICanvasType, UILayerType layer = UILayerType.Normal, UnityAction<T> callBack = null) where T : BaseUI
        {
            if (uiDic.ContainsKey(uiName))
            {
                uiDic[uiName].gameObject.SetActive(true);

                callBack?.Invoke(uiDic[uiName] as T);

                uiDic[uiName].OnOpen();
            }
            else
            {
                AddressableMgr.Instance.LoadAssetAsync<GameObject>(uiName, result =>
                {
                    if (!uiDic.ContainsKey(uiName))
                    {
                        Transform father = GetUILayer(uICanvasType, layer);
                        GameObject obj = GameObject.Instantiate(result, father, false);
                        obj.name = result.name;

                        T panel = obj.GetComponent<T>();

                        callBack?.Invoke(panel);

                        //把面板存起来
                        uiDic.Add(uiName, panel);

                        uiDic[uiName].OnInit();

                        uiDic[uiName].OnOpen();
                    }
                });
            }
        }

        public Canvas GetCanvas(UICanvasType uICanvasType)
        {
            switch (uICanvasType)
            {
                case UICanvasType.Overlay:
                    return overlayCanvas;
                case UICanvasType.Camera:
                    return cameraCanvas;
                case UICanvasType.World:
                    return worldCanvas;
            }

            return cameraCanvas;
        }

        public Transform GetUILayer(UICanvasType uICanvasType, UILayerType uILayerType)
        {
            Transform[] layers = canvasLayersDic[uICanvasType];

            return layers[(int)uILayerType];
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="uiName"></param>
        public void DestroyUI(string uiName)
        {
            if (uiDic.ContainsKey(uiName))
            {
                uiDic[uiName].OnClose();
                uiDic[uiName].OnDestroyUI();
                GameObject.Destroy(uiDic[uiName].gameObject);
                uiDic.Remove(uiName);
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiName"></param>
        public void HideUI(string uiName)
        {
            if (uiDic.ContainsKey(uiName))
            {
                uiDic[uiName].OnClose();
                uiDic[uiName].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 得到UI
        /// </summary>
        public T GetUI<T>(string name) where T : BaseUI
        {
            if (uiDic.ContainsKey(name))
            {
                return uiDic[name] as T;
            }

            throw new PanelNotFoundException();
        }
    }
}

