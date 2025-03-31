
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class LogMgr : Singleton<LogMgr>
    {
        //�Ƿ�������ͨ��־
        public bool isEnableDebugLog;
        //�Ƿ����þ�����־
        public bool isEnableDebugLogWarning;
        //�Ƿ����ô�����־
        public bool isEnableDebugLogError;
        //�Ƿ��ӡ������־����Ļ
        public bool isPrintErrorMsgOnScreen;

        //��������
        private StringBuilder errorMessageSB;

        private GameObject logErrorPanel;
        private TMP_Text txtError;

        public LogMgr()
        {
            errorMessageSB = new StringBuilder();

            Application.logMessageReceived += OnLogCallBack;
        }

        public void ToggleLogging(bool isEnableLog)
        {
            isEnableDebugLog = isEnableLog;
            isEnableDebugLogWarning = isEnableLog;
            isEnableDebugLogError = isEnableLog;
            isPrintErrorMsgOnScreen = isEnableLog;
        }

        public void Log(object message, Object context = null)
        {
            if (isEnableDebugLog)
            {
                Debug.Log(message, context);
            }
        }

        public void LogWarning(object message, Object context = null)
        {
            if (isEnableDebugLogWarning)
            {
                Debug.LogWarning(message, context);
            }
        }

        public void LogError(object message, Object context = null)
        {
            if (isEnableDebugLogError)
            {
                Debug.LogError(message, context);
            }
        }

        // ��־�ص�
        private void OnLogCallBack(string logString, string stackTrace, LogType type)
        {
            // ��������Ϣ�Ͷ�ջ��Ϣ�洢�ڱ�����
            if (isPrintErrorMsgOnScreen && (type == LogType.Error || type == LogType.Exception))
            {
                errorMessageSB.Append(logString);
                errorMessageSB.Append("\n");
                errorMessageSB.Append(stackTrace);
                errorMessageSB.Append("\n");

                PrintErrorMsgOnScreen();
            }
        }

        // ��ӡ������Ϣ����Ļ
        private void PrintErrorMsgOnScreen()
        {
            if (isPrintErrorMsgOnScreen && errorMessageSB != null && errorMessageSB.Length > 0)
            {
                if(logErrorPanel == null)
                {
                    CreateLogErrorPanel();
                }

                txtError.text = errorMessageSB.ToString();

                DebugUIMgr.Instance.ShowPanel(logErrorPanel);
            }
        }

        //�����������
        private void CreateLogErrorPanel()
        {
            logErrorPanel = new GameObject("LogErrorPanel");
            Image imgLogErrorPanel = logErrorPanel.AddComponent<Image>();
            imgLogErrorPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            RectTransform logErrorPanelRT = logErrorPanel.transform as RectTransform;
            logErrorPanelRT.anchorMin = Vector2.zero;
            logErrorPanelRT.anchorMax = Vector2.one;
            logErrorPanelRT.offsetMin = Vector2.zero;
            logErrorPanelRT.offsetMax = Vector2.zero;

            GameObject bgGO = new GameObject("Bg");
            RectTransform bgRT = bgGO.AddComponent<RectTransform>();
            bgGO.AddComponent<SafeAreaAdjuster>();
            bgRT.transform.SetParent(logErrorPanel.transform, false);
            bgRT.anchorMin = Vector2.zero;
            bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero;
            bgRT.offsetMax = Vector2.zero;

            //����ScrollRect
            GameObject scrollViewGO = new GameObject("ScrollView");
            Image imgScrollView = scrollViewGO.AddComponent<Image>();
            imgScrollView.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            scrollViewGO.transform.SetParent(logErrorPanel.transform, false);
            RectTransform scrollViewRT = scrollViewGO.GetComponent<RectTransform>();
            scrollViewRT.anchorMin = Vector2.zero;
            scrollViewRT.anchorMax = Vector2.one;
            scrollViewRT.offsetMin = new Vector2(0f, 150f);
            scrollViewRT.offsetMax = Vector2.zero;

            ScrollRect scrollRect = scrollViewGO.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.horizontalScrollbar = null;

            //����Viewport
            GameObject viewport = new GameObject("Viewport");
            RectTransform viewportRT = viewport.AddComponent<RectTransform>();
            viewport.transform.SetParent(scrollViewGO.transform, false);
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.offsetMin = Vector2.zero;
            viewportRT.offsetMax = Vector2.zero;
            viewport.AddComponent<RectMask2D>();

            // ����Text
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

            //������ֱ������
            GameObject scrollbarGO = new GameObject("ScrollbarVertical");
            Image imgScrollbar = scrollbarGO.AddComponent<Image>();
            imgScrollbar.color = new Color(0f, 0f, 0f, 1f);
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
            imgHandle.color = new Color(0.5f, 0.5f, 0.5f, 1f);
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

            //�����ײ��رհ�ť
            GameObject btnCloseGO = new GameObject("btnClose");
            Image imgClose = btnCloseGO.AddComponent<Image>();
            imgClose.color = Color.gray;
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
            btnCloseGO.transform.SetParent(logErrorPanel.transform, false);
            txtCloseGO.transform.SetParent(btnCloseGO.transform, false);
        }
    }
}