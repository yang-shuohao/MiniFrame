
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class LogMgr : Singleton<LogMgr>
    {
        //是否启用日志
        public bool isEnableLog;
        //是否打印错误日志到屏幕
        public bool isPrintErrorMsgOnScreen;
        //是否上传错误日志
        public bool isUpLoadErrorMsg;

        //错误数据
        private StringBuilder errorMessageSB;

        private GameObject logErrorPanel;
        private TMP_Text txtError;

        public LogMgr()
        {
            errorMessageSB = new StringBuilder();

            Application.logMessageReceived += OnLogCallBack;
        }

        public void EnableLog(bool isEnable)
        {
            isEnableLog = isEnable;
            isPrintErrorMsgOnScreen = isEnable;
            isUpLoadErrorMsg = isEnable;
        }

        public void Log(object message, Object context = null)
        {
            if (isEnableLog)
            {
                Debug.Log(message, context);
            }
        }

        public void LogWarning(object message, Object context = null)
        {
            if (isEnableLog)
            {
                Debug.LogWarning(message, context);
            }
        }

        public void LogError(object message, Object context = null)
        {
            if (isEnableLog)
            {
                Debug.LogError(message, context);
            }
        }

        // 日志回调
        private void OnLogCallBack(string logString, string stackTrace, LogType type)
        {
            // 将错误消息和堆栈信息存储在变量中
            if (isPrintErrorMsgOnScreen && (type == LogType.Error || type == LogType.Exception))
            {
                errorMessageSB.Append(logString);
                errorMessageSB.Append("\n");
                errorMessageSB.Append(stackTrace);
                errorMessageSB.Append("\n");

                PrintErrorMsgOnScreen();

                UploadErrorMsg();
            }
        }
        // 上传错误信息
        private void UploadErrorMsg()
        {
            if (!isUpLoadErrorMsg)
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
        UpLoadErrorMsgInWXMiniGame();
#endif
        }

        private void UpLoadErrorMsgInWXMiniGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WeChatWASM.WXBase.cloud.Init(new WeChatWASM.CallFunctionInitParam()
            {
                env = WXMgr.envID,
                traceUser = false
            });

            WeChatWASM.WXBase.cloud.CallFunction(new WeChatWASM.CallFunctionParam()
            {
                name = WXMgr.upLoadErrorDataName,
                data = JsonMapper.ToJson(errorMessageSB.ToString()),
                success = (res) =>
                {
                    LogMgr.Instance.Log("UpLoadErrorData Success!");
                },
                fail = (res) =>
                {
                    LogMgr.Instance.Log("UpLoadErrorData Failed!" + res.errMsg);
                },
            });
#endif
        }

        // 打印错误信息到屏幕
        private void PrintErrorMsgOnScreen()
        {
            if (isPrintErrorMsgOnScreen && errorMessageSB != null && errorMessageSB.Length > 0)
            {
                if (logErrorPanel == null)
                {
                    CreateLogErrorPanel();
                }

                txtError.text = errorMessageSB.ToString();

                DebugUIMgr.Instance.ShowPanel(logErrorPanel);
            }
        }

        //创建错误面板
        private void CreateLogErrorPanel()
        {
            logErrorPanel = new GameObject("LogErrorPanel");
            RectTransform logErrorPanelRT = logErrorPanel.AddComponent<RectTransform>();
            logErrorPanelRT.anchorMin = Vector2.zero;
            logErrorPanelRT.anchorMax = Vector2.one;
            logErrorPanelRT.offsetMin = Vector2.zero;
            logErrorPanelRT.offsetMax = Vector2.zero;

            GameObject bgGO = new GameObject("Bg");
            Image imgBg = bgGO.AddComponent<Image>();
            imgBg.color = new Color(0f, 0f, 0f, 0.5f);
            RectTransform bgRT = bgGO.transform as RectTransform;
            bgRT.transform.SetParent(logErrorPanel.transform, false);
            bgRT.anchorMin = Vector2.zero;
            bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero;
            bgRT.offsetMax = Vector2.zero;
            bgGO.AddComponent<SafeAreaAdjuster>();

            //创建ScrollRect
            GameObject scrollViewGO = new GameObject("ScrollView");
            Image imgScrollView = scrollViewGO.AddComponent<Image>();
            imgScrollView.color = new Color(0f, 0f, 0f, 0.5f);
            scrollViewGO.transform.SetParent(bgGO.transform, false);
            RectTransform scrollViewRT = scrollViewGO.GetComponent<RectTransform>();
            scrollViewRT.anchorMin = Vector2.zero;
            scrollViewRT.anchorMax = Vector2.one;
            scrollViewRT.offsetMin = new Vector2(0f, 150f);
            scrollViewRT.offsetMax = Vector2.zero;

            ScrollRect scrollRect = scrollViewGO.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.horizontalScrollbar = null;

            //创建Viewport
            GameObject viewport = new GameObject("Viewport");
            RectTransform viewportRT = viewport.AddComponent<RectTransform>();
            viewport.transform.SetParent(scrollViewGO.transform, false);
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.offsetMin = Vector2.zero;
            viewportRT.offsetMax = Vector2.zero;
            viewport.AddComponent<RectMask2D>();
            scrollRect.viewport = viewportRT;

            // 创建Text
            GameObject txtErrorGO = new GameObject("txtError");
            txtError = txtErrorGO.AddComponent<TextMeshProUGUI>();
            txtError.fontSize = 50f;
            txtError.color = Color.red;
            ContentSizeFitter contentSizeFitter = txtErrorGO.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            RectTransform txtErrorRT = txtErrorGO.transform as RectTransform;
            txtErrorGO.transform.SetParent(viewportRT, false);
            txtErrorRT.offsetMin = Vector2.zero;
            txtErrorRT.offsetMax = Vector2.zero;
            txtErrorRT.anchorMin = Vector2.zero;
            txtErrorRT.anchorMax = Vector2.one;
            scrollRect.content = txtErrorRT;

            //创建垂直滚动条
            GameObject scrollbarGO = new GameObject("ScrollbarVertical");
            Image imgScrollbar = scrollbarGO.AddComponent<Image>();
            imgScrollbar.color = new Color(0.373f, 0.373f, 0.373f, 1f);
            Scrollbar scrollbar = scrollbarGO.AddComponent<Scrollbar>();
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            RectTransform scrollbarRT = scrollbarGO.GetComponent<RectTransform>();
            scrollbarRT.pivot = Vector2.one;
            scrollbarRT.anchorMin = new Vector2(1f, 0f);
            scrollbarRT.anchorMax = Vector2.one;
            scrollbarRT.offsetMin = new Vector2(-40f, 0f);
            scrollbarRT.offsetMax = Vector2.zero;

            GameObject handleGO = new GameObject("Handle");
            Image imgHandle = handleGO.AddComponent<Image>();
            imgHandle.color = new Color(0.345f, 0.882f, 0.737f, 1f);
            scrollbar.handleRect = (handleGO.transform as RectTransform);
            scrollbar.targetGraphic = imgHandle;
            RectTransform handleGORT = handleGO.transform as RectTransform;
            handleGORT.offsetMin = Vector2.zero;
            handleGORT.offsetMax = Vector2.zero;

            handleGO.transform.SetParent(scrollbarGO.transform, false);
            scrollbarGO.transform.SetParent(scrollViewGO.transform, false);

            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarSpacing = -3f;
            scrollbar.value = 1f;

            //创建底部关闭按钮
            GameObject btnCloseGO = new GameObject("btnClose");
            Image imgClose = btnCloseGO.AddComponent<Image>();
            imgClose.color = new Color(0.098f, 0.580f, 0.984f, 0.5f);
            Button btnClose = btnCloseGO.AddComponent<Button>();
            btnClose.targetGraphic = imgClose;
            btnClose.onClick.AddListener(() =>
            {
                errorMessageSB.Clear();

                DebugUIMgr.Instance.HidePanel(logErrorPanel.name);
            });
            RectTransform btnCloseRT = (btnCloseGO.transform as RectTransform);
            btnCloseRT.anchorMin = Vector2.zero;
            btnCloseRT.anchorMax = new Vector2(1f, 0f);
            btnCloseRT.offsetMin = new Vector2(0f, 0f);
            btnCloseRT.offsetMax = new Vector2(0f, 150f);

            GameObject txtCloseGO = new GameObject("txtClose");
            TMP_Text txtRun = txtCloseGO.AddComponent<TextMeshProUGUI>();
            txtRun.fontSize = 70f;
            txtRun.color = Color.white;
            txtRun.alignment = TextAlignmentOptions.Center;
            txtRun.text = "Close";
            RectTransform txtRunRT = txtCloseGO.transform as RectTransform;
            txtRunRT.anchorMin = Vector2.zero;
            txtRunRT.anchorMax = Vector2.one;
            txtRunRT.offsetMin = Vector2.zero;
            txtRunRT.offsetMax = Vector2.zero;
            btnCloseGO.transform.SetParent(bgGO.transform, false);
            txtCloseGO.transform.SetParent(btnCloseGO.transform, false);
        }
    }
}