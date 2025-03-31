
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace YSH.Framework
{
    public class DebugUIMgr : MonoSingleton<DebugUIMgr>
    {
        private Dictionary<string, GameObject> panelDic = new Dictionary<string, GameObject>();

        private Canvas canvas;

        //弹框提示
        private DebugUIPopup debugUIPopup;

        private void Awake()
        {
            CreateCanvas();
        }

        // 创建Canvas并设置为屏幕覆盖模式
        private void CreateCanvas()
        {
            canvas = new GameObject("Canvas").AddComponent<Canvas>();
            canvas.transform.SetParent(transform, false);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; 

            CanvasScaler canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0;
            canvas.gameObject.AddComponent<GraphicRaycaster>();
        }

        public void ShowPanel(GameObject panel)
        {
            if(panelDic.ContainsKey(panel.name))
            {
                panelDic[panel.name].SetActive(true);
            }
            else
            {
                panelDic[panel.name] = panel;
                panelDic[panel.name].transform.SetParent(canvas.transform, false);
            }

            panelDic[panel.name].transform.SetAsLastSibling();
        }

        public void HidePanel(string panelName)
        {
            if (panelDic.ContainsKey(panelName))
            {
                panelDic[panelName].SetActive(false);
            }
        }

        public void ShowPopupPanel(string msg, float duration = 2f)
        {
            if(debugUIPopup == null)
            {
                debugUIPopup = new DebugUIPopup(canvas.transform);
            }
            debugUIPopup.ShowPopup(msg, duration);
        }
    }
}

