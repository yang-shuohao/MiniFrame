using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class GMMgr : Singleton<GMMgr>
    {
        // 是否显示 GM 界面
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        // UI
        private GameObject gmPanel;
        private GameObject bgGO;
        private GameObject contentGO;
        private TMP_InputField inputField;

        //启用GM
        public void EnableGM()
        {
            MonoMgr.Instance.AddUpdateListener(OnUpdate);
        }

        private void OnUpdate()
        {
            // PC 端按 ~ 键 / 移动端检测四指点击，打开 GM 面板
            if (Input.GetKeyDown(KeyCode.BackQuote) || Input.touchCount >= 4)
            {
                ShowGMUI();
            }
        }

        private void ShowGMUI()
        {
            showConsole = !showConsole;
            InputMgr.Instance.IsEnableInput = !showConsole;

            if(gmPanel == null)
            {
                CreateGMUI();
            }

            if (showConsole)
            {
                DebugUIMgr.Instance.ShowPanel(gmPanel);
            }
            else
            {
                DebugUIMgr.Instance.HidePanel(gmPanel.name);
            }
        }

        //创建GMUI
        private void CreateGMUI()
        {
            // **创建GMPanel**
            CreateGMPanel();

            // **创建输入框和按钮**
            CreateInputFieldAndButton();

            // **滚动区域**
            CreateScrollView();

            // **滚动区域内容**
            CreateScrollViewContent();
        }

        //创建GMPanel
        private void CreateGMPanel()
        {
            //根
            gmPanel = new GameObject("GMPanel");
            RectTransform imgGMRT = gmPanel.AddComponent<RectTransform>();
            imgGMRT.anchorMin = Vector2.zero;
            imgGMRT.anchorMax = Vector2.one;
            imgGMRT.offsetMin = Vector2.zero;
            imgGMRT.offsetMax = Vector2.zero;

            //背景
            bgGO = new GameObject("Bg");
            Image imgBg = bgGO.AddComponent<Image>();
            imgBg.color = new Color(0.176f, 0.176f, 0.176f, 0.5f);
            bgGO.transform.SetParent(gmPanel.transform, false);
            RectTransform imgBgRT = imgBg.transform as RectTransform;
            bgGO.AddComponent<SafeAreaAdjuster>();
            imgBgRT.anchorMin = Vector2.zero;
            imgBgRT.anchorMax = Vector2.one;
            imgBgRT.offsetMin = Vector2.zero;
            imgBgRT.offsetMax = Vector2.zero;
        }

        // 创建滚动区域
        private void CreateScrollView()
        {
            //创建ScrollRect
            GameObject scrollViewGO = new GameObject("ScrollView");
            Image imgScrollView = scrollViewGO.AddComponent<Image>();
            scrollViewGO.transform.SetParent(bgGO.transform, false);
            imgScrollView.color = Vector4.zero;

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

            // 创建内容区域
            contentGO = new GameObject("Content");
            RectTransform contentRT = contentGO.AddComponent<RectTransform>();
            contentGO.transform.SetParent(viewport.transform, false);
            contentRT.offsetMin = Vector2.zero;
            contentRT.offsetMax = Vector2.zero;
            contentRT.anchorMin = new Vector2(0f, 1f);
            contentRT.anchorMax = Vector2.one;
            contentRT.pivot = new Vector2(0f, 1f);
            ContentSizeFitter contentSizeFitter = contentGO.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            VerticalLayoutGroup verticalLayoutGroup = contentGO.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.spacing = 20f;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childScaleWidth = false;
            verticalLayoutGroup.childScaleHeight = true;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = false;
            scrollRect.content = contentRT;

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
        }

        //创建滚动区域里面内容
        private void CreateScrollViewContent()
        {
            // 获取模糊匹配结果
            var matchedCommands = gmCmd.GetMatchingCommands(inputField.text);

            foreach (var command in matchedCommands)
            {
                string displayText = $"<color=red>[{command.Name}]</color><color=green>{command.Description}</color>";

                GameObject cmdGO = new GameObject("CmdGO");
                cmdGO.transform.SetParent(contentGO.transform, false);
                Image imgCmd = cmdGO.AddComponent<Image>();
                imgCmd.color = new Color(0.141f, 0.141f, 0.141f, 0.5f);
                RectTransform cmdGORT = cmdGO.transform as RectTransform;
                cmdGORT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
                Button btnCmd = cmdGO.AddComponent<Button>();
                btnCmd.targetGraphic = imgCmd;
                btnCmd.onClick.AddListener(() =>
                {
                    gmCmd.ExecuteCommand(command.Name);
                    inputField.text = command.Name;
                });

                //文本
                GameObject txtCmdGO = new GameObject("txtCmd");
                TMP_Text txtCmd = txtCmdGO.AddComponent<TextMeshProUGUI>();
                txtCmd.fontSize = 50f;
                txtCmd.color = Color.black;
                txtCmd.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Center;
                txtCmd.text = displayText;
                RectTransform txtCmdGORT = txtCmdGO.transform as RectTransform;
                txtCmdGORT.anchorMin = Vector2.zero;
                txtCmdGORT.anchorMax = Vector2.one;
                txtCmdGORT.offsetMin = Vector2.zero;
                txtCmdGORT.offsetMax = Vector2.zero;
                txtCmdGO.transform.SetParent(cmdGO.transform, false);
            }
        }

        //创建输入框，执行和清空按钮
        private void CreateInputFieldAndButton()
        {
            //输入框
            GameObject ifGO = new GameObject("IfGO");
            Image imgIf = ifGO.AddComponent<Image>();
            imgIf.color = new Color(0.263f, 0.686f, 0.988f, 1f);
            RectTransform ifGORT = ifGO.transform as RectTransform;
            ifGO.transform.SetParent(bgGO.transform, false);
            ifGORT.anchorMin = Vector2.zero;
            ifGORT.anchorMax = new Vector2(1f, 0f);
            ifGORT.offsetMin = new Vector2(0, 20);
            ifGORT.offsetMax = new Vector2(-400, 100);

            //Text Area
            GameObject textArea = new GameObject("TextArea");
            RectTransform textAreaRT = textArea.AddComponent<RectTransform>();
            textArea.transform.SetParent(ifGO.transform, false);
            RectMask2D rectMask2D = textArea.AddComponent<RectMask2D>();
            rectMask2D.padding = new Vector4(-8f, -5f, -8f, -5f);
            textAreaRT.anchorMin = Vector2.zero;
            textAreaRT.anchorMax = Vector2.one;
            textAreaRT.offsetMin = new Vector2(10f, 6f);
            textAreaRT.offsetMax = new Vector2(-10f, -7f);

            //Placeholder
            GameObject placeholder = new GameObject("Placeholder");
            TMP_Text txtPlaceholder = placeholder.AddComponent<TextMeshProUGUI>();
            txtPlaceholder.fontSize = 40f;
            txtPlaceholder.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Center;
            txtPlaceholder.color = Color.black;
            txtPlaceholder.enableWordWrapping = false;
            txtPlaceholder.extraPadding = true;
            txtPlaceholder.text = "Enter text...";
            LayoutElement layoutElement = placeholder.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;
            layoutElement.layoutPriority = 1;
            placeholder.transform.SetParent(textArea.transform, false);
            RectTransform placeholderRT = placeholder.transform as RectTransform;
            placeholderRT.anchorMin = Vector2.zero;
            placeholderRT.anchorMax = Vector2.one;
            placeholderRT.offsetMin = Vector2.zero;
            placeholderRT.offsetMax = Vector2.zero;

            //Text
            GameObject txtGO = new GameObject("Text");
            TMP_Text txt = txtGO.AddComponent<TextMeshProUGUI>();
            txt.fontSize = 40f;
            txt.color = new Color(0f, 0f, 0f, 1f);
            txt.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Center;
            txt.enableWordWrapping = false;
            txt.extraPadding = true;
            txtGO.transform.SetParent(textArea.transform, false);
            RectTransform txtGORT = txtGO.transform as RectTransform;
            txtGORT.anchorMin = Vector2.zero;
            txtGORT.anchorMax = Vector2.one;
            txtGORT.offsetMin = Vector2.zero;
            txtGORT.offsetMax = Vector2.zero;

            //InputField
            inputField = ifGO.AddComponent<TMP_InputField>();
            inputField.textViewport = textAreaRT;
            inputField.textComponent = txt;
            inputField.pointSize = 40f;
            inputField.placeholder = txtPlaceholder;
            inputField.caretWidth = 2;

            //*按钮*//
            //Run按钮
            GameObject btnRunGO = new GameObject("btnRun");
            Image imgRun = btnRunGO.AddComponent<Image>();
            imgRun.color = new Color(0.157f, 0.569f, 0.984f, 1f);
            Button btnRun = btnRunGO.AddComponent<Button>();
            btnRun.targetGraphic = imgRun;
            btnRun.onClick.AddListener(() =>
            {
                gmCmd.ExecuteCommand(inputField.text);
            });
            RectTransform btnRunRT = (btnRunGO.transform as RectTransform);
            btnRunRT.anchorMin = new Vector2(1f, 0f);
            btnRunRT.anchorMax = new Vector2(1f, 0f);
            btnRunRT.offsetMin = new Vector2(-355f, 20f);
            btnRunRT.offsetMax = new Vector2(-205f, 100f);

            GameObject txtRunGO = new GameObject("txtRun");
            TMP_Text txtRun = txtRunGO.AddComponent<TextMeshProUGUI>();
            txtRun.fontSize = 50f;
            txtRun.color = Color.black;
            txtRun.alignment = TextAlignmentOptions.Center;
            txtRun.text = "Run";
            RectTransform txtRunRT = txtRunGO.transform as RectTransform;
            txtRunRT.anchorMin = Vector2.zero;
            txtRunRT.anchorMax = Vector2.one;
            txtRunRT.offsetMin = Vector2.zero;
            txtRunRT.offsetMax = Vector2.zero;
            btnRunGO.transform.SetParent(bgGO.transform, false);
            txtRunGO.transform.SetParent(btnRunGO.transform, false);

            //Clear按钮
            GameObject btnClearGO = new GameObject("btnClear");
            Image imgClear = btnClearGO.AddComponent<Image>();
            imgClear.color = new Color(0.157f, 0.569f, 0.984f, 1f);
            Button btnClear = btnClearGO.AddComponent<Button>();
            btnClear.targetGraphic = imgClear;
            btnClear.onClick.AddListener(() =>
            {
                inputField.text = "";
            });
            RectTransform btnClearRT = (btnClearGO.transform as RectTransform);
            btnClearRT.anchorMin = new Vector2(1f, 0f);
            btnClearRT.anchorMax = new Vector2(1f, 0f);
            btnClearRT.offsetMin = new Vector2(-175f, 20f);
            btnClearRT.offsetMax = new Vector2(-25f, 100f);
            GameObject txtClearGO = new GameObject("txtClear");
            TMP_Text txtClear = txtClearGO.AddComponent<TextMeshProUGUI>();
            txtClear.fontSize = 50f;
            txtClear.color = Color.black;
            txtClear.alignment = TextAlignmentOptions.Center; ;
            txtClear.text = "Clear";
            RectTransform txtClearRT = txtClearGO.transform as RectTransform;
            txtClearRT.anchorMin = Vector2.zero;
            txtClearRT.anchorMax = Vector2.one;
            txtClearRT.offsetMin = Vector2.zero;
            txtClearRT.offsetMax = Vector2.zero;
            btnClearGO.transform.SetParent(bgGO.transform, false);
            txtClearGO.transform.SetParent(btnClearGO.transform, false);
        }
    }
}

