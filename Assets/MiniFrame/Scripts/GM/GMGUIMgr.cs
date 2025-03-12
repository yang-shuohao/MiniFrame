using UnityEngine;

namespace YSH.Framework
{
    public class GMGUIMgr : MonoSingleton<GMGUIMgr>
    {
        private string logContent = "";
        private Vector2 logScrollPosition;
        private string commandInput = "";
        private string[] matchingCommands = new string[0];
        private Vector2 commandListScrollPosition;

        public bool isShowGMUI;

        private void Awake()
        {
            isShowGMUI = true;
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            // PC ƽ̨
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                //���²��˼�
                isShowGMUI = true;
            }
#elif UNITY_ANDROID || UNITY_IOS
        // �ƶ�ƽ̨
        if (Input.touchCount == 4)
        {
            bool allFingersDown = true;
            foreach (Touch touch in Input.touches)
            {
                if (!touch.phase.Equals(TouchPhase.Began) && !touch.phase.Equals(TouchPhase.Moved) && !touch.phase.Equals(TouchPhase.Stationary))
                {
                    allFingersDown = false;
                    break;
                }
            }

            if (allFingersDown)
            {
                //�ĸ���ָͬʱ����
                isShowGMUI = true;
            }
        }
#endif


        }

        public void OnGUI()
        {
            if (isShowGMUI)
            {
                ShowGMUI();
            }
        }

        protected void ShowGMUI()
        {
            // ���û�����ʽ
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 16
            };
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14
            };
            GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 14
            };
            GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea)
            {
                fontSize = 14
            };

            // �м䲿����ʾ��־
            GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 100), "��־");
            logScrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width - 40, Screen.height - 150), logScrollPosition, new Rect(0, 0, Screen.width - 60, logContent.Length * 14), false, false);
            GUI.TextArea(new Rect(0, 0, Screen.width - 60, logContent.Length * 14), logContent, textAreaStyle);
            GUI.EndScrollView();

            // ���½���ʾ������ʾ
            GUI.Label(new Rect(10, Screen.height - 80, 80, 30), "������ָ��", labelStyle);

            // �����м���ʾ�����
            commandInput = GUI.TextField(new Rect(100, Screen.height - 80, Screen.width - 320, 30), commandInput, textFieldStyle);

            // ��ʾƥ��������б�
            if (!string.IsNullOrEmpty(commandInput))
            {
                matchingCommands = GMCommandMgr.Instance.GetMatchingCommands(commandInput);
            }
            else
            {
                matchingCommands = new string[0];
            }

            // ������ͼ��ʾƥ��ķ����б�
            float commandListHeight = Mathf.Min(matchingCommands.Length * 30, 150);
            commandListScrollPosition = GUI.BeginScrollView(new Rect((Screen.width - 400) / 2, Screen.height - 220 - commandListHeight, Screen.width - 100, commandListHeight), commandListScrollPosition, new Rect(0, 0, Screen.width - 100, matchingCommands.Length * 30));
            for (int i = 0; i < matchingCommands.Length; i++)
            {
                if (GUI.Button(new Rect(0, i * 30, 180, 30), matchingCommands[i], buttonStyle))
                {
                    commandInput = matchingCommands[i];
                }
                string description = GMCommandMgr.Instance.GetCommandDescription(matchingCommands[i]);
                GUI.Label(new Rect(200, i * 30, 300, 30), description, labelStyle);
            }
            GUI.EndScrollView();

            // �����ұ���ʾ�ύ��ť
            if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 80, 80, 30), "�ύ", buttonStyle))
            {
                if (!string.IsNullOrEmpty(commandInput))
                {
                    // ִ������
                    GMCommandMgr.Instance.ExecuteCommand(commandInput);
                    // �ύ������߼�
                    LogMgr.Instance.Log("�ύ��ť�������������" + commandInput);
                    // �������������ӵ���־��
                    logContent += "\n" + commandInput;
                    // ��������
                    commandInput = "";
                }
            }

            // ���½ǵĹرհ�ť
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 80, 80, 30), "�ر�", buttonStyle))
            {
                isShowGMUI = false;
            }
        }
    }
}

