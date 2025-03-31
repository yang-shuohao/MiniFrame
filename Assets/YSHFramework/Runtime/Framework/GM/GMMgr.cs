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
        private Texture2D solidBackgroundTexture;

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

            // �������û�д������򴴽�һ����͸���ȵı�������
            if (solidBackgroundTexture == null)
            {
                solidBackgroundTexture = new Texture2D(1, 1);
                solidBackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.8f));
                solidBackgroundTexture.Apply();
            }
            // ����һ��ȫ����������͸����
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), solidBackgroundTexture);

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 20,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft
            };

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleLeft
            };

            GUIStyle inputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 24
            };

            GUIStyle executeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter // �á�ִ�С���ť�����־���
            };

            GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter // �á���ա���ť�����־���
            };

            // ���� GUI �߶�
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float inputHeight = 80; // �����߶�
            float resultHeight = screenHeight - inputHeight; // �������߶�

            // ��ȡģ��ƥ����
            var matchedCommands = gmCmd.GetMatchingCommands(inputText);

            // ��ʼ��������
            scrollPosition = GUI.BeginScrollView(new Rect(0, 0, screenWidth, resultHeight), scrollPosition, new Rect(0, 0, screenWidth - 20, matchedCommands.Count * 90));

            float yOffset = 0;
            foreach (var command in matchedCommands)
            {
                string displayText = $"{command.Name}��{command.Description}��"; // ���磺AddGold������������ӽ�ҡ�

                if (GUI.Button(new Rect(20, yOffset, screenWidth - 40, 80), displayText, buttonStyle))
                {
                    gmCmd.ExecuteCommand(command.Name); // �����ťĬ��ִ������޲�����
                    inputText = command.Name;
                }
                yOffset += 90;
            }

            GUI.EndScrollView();

            // �����
            inputText = GUI.TextField(new Rect(20, screenHeight - inputHeight, screenWidth - 350, inputHeight - 20), inputText, inputStyle);

            // ִ�а�ť
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "Run", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // ��հ�ť
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "Clear", clearButtonStyle))
            {
                inputText = ""; // ��������
            }
        }
    }
}

