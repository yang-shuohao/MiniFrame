using UnityEngine;

namespace YSH.Framework
{
    public class GMMgr : MonoSingleton<GMMgr>
    {
        //�Ƿ�����GM
        public bool isEnableGM;

        // �û����������
        private string inputText = "";
        // ����λ��
        private Vector2 scrollPosition = Vector2.zero;
        // �Ƿ���ʾ GM ����
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        private void Update()
        {
            if (!isEnableGM) return;

            // PC �˰� ~ ���� GM ���
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                showConsole = !showConsole;
            }

            // �ƶ��˼����ָ���
            if (Input.touchCount >= 4)
            {
                showConsole = !showConsole;
            }
        }

        private void OnGUI()
        {
            if (!showConsole) return;

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
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "ִ��", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // ��հ�ť
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "���", clearButtonStyle))
            {
                inputText = ""; // ��������
            }
        }
    }
}

