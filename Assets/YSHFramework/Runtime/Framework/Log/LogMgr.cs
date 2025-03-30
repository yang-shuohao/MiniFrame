
using System.Text;
using UnityEngine;

namespace YSH.Framework
{
    public class LogMgr : MonoSingleton<LogMgr>
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

        // ������λ��
        private Vector2 scrollPosition = Vector2.zero;
        private Texture2D solidBackgroundTexture;

        private void Awake()
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
            }
        }

        // ��ӡ������Ϣ����Ļ
        void OnGUI()
        {
            if (isPrintErrorMsgOnScreen && errorMessageSB != null && errorMessageSB.Length > 0)
            {
                // �������û�д������򴴽�һ����͸���ȵı�������
                if (solidBackgroundTexture == null)
                {
                    solidBackgroundTexture = new Texture2D(1, 1);
                    solidBackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.8f));
                    solidBackgroundTexture.Apply();
                }
                // ����һ��ȫ����������͸����
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), solidBackgroundTexture);

                // ������Ϣ��ʽ
                GUIStyle errorStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 30, // �Ŵ�����
                    alignment = TextAnchor.MiddleLeft, // ���������
                    wordWrap = true, // �Զ�����
                    normal = { textColor = Color.red }
                };

                // **��ť��ʽ**
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 40 // �Ŵ�ť����
                };

                float buttonHeight = 150;
                float textAreaHeight = Screen.height - buttonHeight; // �ù�������ռ��������Ļ����ť��������

                // **��������**
                GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, textAreaHeight - 100)); // ����һЩ�߾�
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 100), GUILayout.Height(textAreaHeight - 100));
                GUILayout.Label(errorMessageSB.ToString(), errorStyle);
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                // **�ײ��رհ�ť**
                GUILayout.BeginArea(new Rect(0, Screen.height - buttonHeight, Screen.width, buttonHeight));
                if (GUILayout.Button("Close", buttonStyle, GUILayout.Height(buttonHeight)))
                {
                    errorMessageSB.Clear();
                }
                GUILayout.EndArea();
            }
        }
    }
}