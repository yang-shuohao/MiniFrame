using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class GMMgr : Singleton<GMMgr>
    {
        // �Ƿ���ʾ GM ����
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        // UI
        private GameObject gmPanel;
        private GameObject bgGO;
        private GameObject contentGO;
        private TMP_InputField inputField;

        //����GM
        public void EnableGM()
        {
            MonoMgr.Instance.AddUpdateListener(OnUpdate);
        }

        private void OnUpdate()
        {
            // PC �˰� ~ �� / �ƶ��˼����ָ������� GM ���
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

        //����GMUI
        private void CreateGMUI()
        {
            // **����GMPanel**
            CreateGMPanel();

            // **���������Ͱ�ť**
            CreateInputFieldAndButton();

            // **��������**
            CreateScrollView();

            // **������������**
            CreateScrollViewContent();
        }

        //����GMPanel
        private void CreateGMPanel()
        {
            //��
            gmPanel = new GameObject("GMPanel");
            Image imgGM = gmPanel.AddComponent<Image>();
            imgGM.color = new Color(0f, 0f, 0f, 0.5f);
            RectTransform imgGMRT = imgGM.transform as RectTransform;
            imgGMRT.anchorMin = Vector2.zero;
            imgGMRT.anchorMax = Vector2.one;
            imgGMRT.offsetMin = Vector2.zero;
            imgGMRT.offsetMax = Vector2.zero;

            //����
            bgGO = new GameObject("Bg");
            bgGO.AddComponent<SafeAreaAdjuster>();
            Image imgBg = bgGO.AddComponent<Image>();
            imgBg.color = new Color(0f, 0f, 0f, 0.5f);
            bgGO.transform.SetParent(gmPanel.transform, false);
            RectTransform imgBgRT = imgBg.transform as RectTransform;
            imgBgRT.anchorMin = Vector2.zero;
            imgBgRT.anchorMax = Vector2.one;
            imgBgRT.offsetMin = Vector2.zero;
            imgBgRT.offsetMax = Vector2.zero;
        }

        // ������������
        private void CreateScrollView()
        {
            //����ScrollRect
            GameObject scrollViewGO = new GameObject("ScrollView");
            Image imgScrollView = scrollViewGO.AddComponent<Image>();
            scrollViewGO.transform.SetParent(bgGO.transform, false);
            imgScrollView.color = new Color(0.2f, 0.2f, 0.2f, 0.3f);

            RectTransform scrollViewRT = scrollViewGO.GetComponent<RectTransform>();
            scrollViewRT.anchorMin = Vector2.zero;
            scrollViewRT.anchorMax = Vector2.one;
            scrollViewRT.offsetMin = new Vector2(0f,150f);
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

            // ������������
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
        }

        //��������������������
        private void CreateScrollViewContent()
        {
            // ��ȡģ��ƥ����
            var matchedCommands = gmCmd.GetMatchingCommands(inputField.text);

            foreach (var command in matchedCommands)
            {
                string displayText = $"<color=red>[{command.Name}]</color><color=green>{command.Description}</color>";

                GameObject cmdGO = new GameObject("CmdGO");
                cmdGO.transform.SetParent(contentGO.transform, false);
                Image imgCmd = cmdGO.AddComponent<Image>();
                RectTransform cmdGORT = cmdGO.transform as RectTransform;
                cmdGORT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
                Button btnCmd = cmdGO.AddComponent<Button>();
                btnCmd.targetGraphic = imgCmd;
                btnCmd.onClick.AddListener(() =>
                {
                    gmCmd.ExecuteCommand(command.Name);
                    inputField.text = command.Name;
                });

                //�ı�
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

        //���������ִ�к���հ�ť
        private void CreateInputFieldAndButton()
        {
            //�����
            GameObject ifGO = new GameObject("IfGO");
            ifGO.AddComponent<Image>();
            RectTransform ifGORT = ifGO.transform as RectTransform;
            ifGO.transform.SetParent(bgGO.transform, false);
            ifGORT.anchorMin = Vector2.zero;
            ifGORT.anchorMax = new Vector2(1f,0f);
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

            //*��ť*//
            //Run��ť
            GameObject btnRunGO = new GameObject("btnRun");
            Image imgRun = btnRunGO.AddComponent<Image>();
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

            //Clear��ť
            GameObject btnClearGO = new GameObject("btnClear");
            Image imgClear = btnClearGO.AddComponent<Image>();
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

        //private void OnGUI()
        //{
        //    if (!showConsole) return;

        //    // ����һ��ȫ����������͸����
        //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), GUIMgr.Instance.MakeTexture(1, 1, new Color(0f, 0f, 0f, 0.8f)));

        //    // **��ť��ʽ**
        //    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        //    {
        //        fontSize = 40, // ���ָ���
        //        alignment = TextAnchor.MiddleLeft
        //    };

        //    // **�������ʽ**
        //    GUIStyle inputStyle = new GUIStyle(GUI.skin.textField)
        //    {
        //        fontSize = 40 // ���ָ���
        //    };

        //    // **ִ�а�ť��ʽ**
        //    GUIStyle executeButtonStyle = new GUIStyle(GUI.skin.button)
        //    {
        //        fontSize = 40, // ���ָ���
        //        alignment = TextAnchor.MiddleCenter
        //    };

        //    // **��հ�ť��ʽ**
        //    GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button)
        //    {
        //        fontSize = 40, // ���ָ���
        //        alignment = TextAnchor.MiddleCenter
        //    };

        //    // **�޸Ĺ�������ʽ**
        //    GUI.skin.verticalScrollbar.fixedWidth = 40; // **���������**
        //    GUI.skin.verticalScrollbarThumb.fixedWidth = 40; // **���飨thumb��Ҳ���**

        //    // �Զ��廬����ʽ
        //    GUIStyle thumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
        //    {
        //        fixedWidth = 40, // ���û���Ŀ��Ϊ40
        //        normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.green) } // ���û�����ɫ
        //    };

        //    // ���ù����������ʽ
        //    GUIStyle trackStyle = new GUIStyle(GUI.skin.verticalScrollbar)
        //    {
        //        fixedWidth = 40, // ���ù���������Ŀ��Ϊ40
        //        normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.gray) } // ���ù����ɫ
        //    };

        //    // ��ȡģ��ƥ����
        //    var matchedCommands = gmCmd.GetMatchingCommands(inputText);

        //    // **���� GUI �߶�**
        //    float screenWidth = Screen.width;
        //    float screenHeight = Screen.height;
        //    float inputHeight = 80; // **�����߶Ȳ���**
        //    float resultHeight = screenHeight - inputHeight; // **�������߶�**
        //    float scrollContentHeight = matchedCommands.Count * 90; // **��������߶�**

        //    // **��������**
        //    scrollPosition = GUI.BeginScrollView(
        //        new Rect(0, 0, screenWidth, resultHeight),
        //        scrollPosition,
        //        new Rect(0, 0, screenWidth - 20, scrollContentHeight), // ���ְ�ť��С����
        //        GUIStyle.none, // **ȥ��ˮƽ������**
        //        GUI.skin.verticalScrollbar // **ʹ���޸ĺ�Ĺ�����**
        //    );

        //    // ���ù�������ʽ
        //    GUI.skin.verticalScrollbarThumb = thumbStyle;
        //    GUI.skin.verticalScrollbar = trackStyle;

        //    float yOffset = 0;
        //    foreach (var command in matchedCommands)
        //    {
        //        string displayText = $"{command.Name}��{command.Description}��";

        //        if (GUI.Button(new Rect(20, yOffset, screenWidth - 40, 80), displayText, buttonStyle)) // ��ť��С����
        //        {
        //            gmCmd.ExecuteCommand(command.Name);
        //            inputText = command.Name;
        //        }
        //        yOffset += 90;
        //    }

        //    GUI.EndScrollView();

        //    // **�����**
        //    inputText = GUI.TextField(new Rect(20, screenHeight - inputHeight, screenWidth - 350, inputHeight - 20), inputText, inputStyle);

        //    // **ִ�а�ť**
        //    if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "Run", executeButtonStyle))
        //    {
        //        gmCmd.ExecuteCommand(inputText);
        //    }

        //    // **��հ�ť**
        //    if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "Clear", clearButtonStyle))
        //    {
        //        inputText = "";
        //    }
        //}
    }
}

