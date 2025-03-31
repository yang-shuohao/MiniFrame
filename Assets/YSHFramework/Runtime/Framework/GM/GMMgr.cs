using UnityEngine;

namespace YSH.Framework
{
    public class GMMgr : MonoSingleton<GMMgr>
    {
        // �û����������
        private string inputText = "";
        // �Ƿ���ʾ GM ����
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        // ����λ��
        private Vector2 scrollPosition = Vector2.zero;

        //����GM
        public void EnableGM()
        {

        }

        private void Update()
        {
            // PC �˰� ~ ���� GM ���
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                showConsole = !showConsole;
                InputMgr.Instance.IsEnableInput = !showConsole;
            }

            // �ƶ��˼����ָ���
            if (Input.touchCount >= 4)
            {
                showConsole = !showConsole;
                InputMgr.Instance.IsEnableInput = !showConsole;
            }
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            // ����һ��ȫ����������͸����
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), GUIMgr.Instance.MakeTexture(1, 1, new Color(0f, 0f, 0f, 0.8f)));

            // **��ť��ʽ**
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // ���ָ���
                alignment = TextAnchor.MiddleLeft
            };

            // **�������ʽ**
            GUIStyle inputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 40 // ���ָ���
            };

            // **ִ�а�ť��ʽ**
            GUIStyle executeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // ���ָ���
                alignment = TextAnchor.MiddleCenter
            };

            // **��հ�ť��ʽ**
            GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // ���ָ���
                alignment = TextAnchor.MiddleCenter
            };

            // **�޸Ĺ�������ʽ**
            GUI.skin.verticalScrollbar.fixedWidth = 40; // **���������**
            GUI.skin.verticalScrollbarThumb.fixedWidth = 40; // **���飨thumb��Ҳ���**

            // �Զ��廬����ʽ
            GUIStyle thumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                fixedWidth = 40, // ���û���Ŀ��Ϊ40
                normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.green) } // ���û�����ɫ
            };

            // ���ù����������ʽ
            GUIStyle trackStyle = new GUIStyle(GUI.skin.verticalScrollbar)
            {
                fixedWidth = 40, // ���ù���������Ŀ��Ϊ40
                normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.gray) } // ���ù����ɫ
            };

            // ��ȡģ��ƥ����
            var matchedCommands = gmCmd.GetMatchingCommands(inputText);

            // **���� GUI �߶�**
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float inputHeight = 80; // **�����߶Ȳ���**
            float resultHeight = screenHeight - inputHeight; // **�������߶�**
            float scrollContentHeight = matchedCommands.Count * 90; // **��������߶�**

            // **��������**
            scrollPosition = GUI.BeginScrollView(
                new Rect(0, 0, screenWidth, resultHeight),
                scrollPosition,
                new Rect(0, 0, screenWidth - 20, scrollContentHeight), // ���ְ�ť��С����
                GUIStyle.none, // **ȥ��ˮƽ������**
                GUI.skin.verticalScrollbar // **ʹ���޸ĺ�Ĺ�����**
            );

            // ���ù�������ʽ
            GUI.skin.verticalScrollbarThumb = thumbStyle;
            GUI.skin.verticalScrollbar = trackStyle;

            float yOffset = 0;
            foreach (var command in matchedCommands)
            {
                string displayText = $"{command.Name}��{command.Description}��";

                if (GUI.Button(new Rect(20, yOffset, screenWidth - 40, 80), displayText, buttonStyle)) // ��ť��С����
                {
                    gmCmd.ExecuteCommand(command.Name);
                    inputText = command.Name;
                }
                yOffset += 90;
            }

            GUI.EndScrollView();

            // **�����**
            inputText = GUI.TextField(new Rect(20, screenHeight - inputHeight, screenWidth - 350, inputHeight - 20), inputText, inputStyle);

            // **ִ�а�ť**
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "Run", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // **��հ�ť**
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "Clear", clearButtonStyle))
            {
                inputText = "";
            }
        }
    }
}

