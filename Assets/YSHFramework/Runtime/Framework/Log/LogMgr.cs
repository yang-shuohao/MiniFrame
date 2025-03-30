
using System.Text;
using UnityEngine;

namespace YSH.Framework
{
    public class LogMgr : MonoSingleton<LogMgr>
    {
        //是否启用普通日志
        public bool isEnableDebugLog;
        //是否启用警告日志
        public bool isEnableDebugLogWarning;
        //是否启用错误日志
        public bool isEnableDebugLogError;
        //是否打印错误日志到屏幕
        public bool isPrintErrorMsgOnScreen;

        //错误数据
        private StringBuilder errorMessageSB;

        // 滚动条位置
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
            }
        }

        // 打印错误信息到屏幕
        void OnGUI()
        {
            if (isPrintErrorMsgOnScreen && errorMessageSB != null && errorMessageSB.Length > 0)
            {
                // 如果纹理还没有创建，则创建一个带透明度的背景纹理
                if (solidBackgroundTexture == null)
                {
                    solidBackgroundTexture = new Texture2D(1, 1);
                    solidBackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.8f));
                    solidBackgroundTexture.Apply();
                }
                // 绘制一个全屏背景，带透明度
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), solidBackgroundTexture);

                // 错误信息样式
                GUIStyle errorStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 30, // 放大字体
                    alignment = TextAnchor.MiddleLeft, // 文字左对齐
                    wordWrap = true, // 自动换行
                    normal = { textColor = Color.red }
                };

                // **按钮样式**
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 40 // 放大按钮文字
                };

                float buttonHeight = 150;
                float textAreaHeight = Screen.height - buttonHeight; // 让滚动区域占据整个屏幕，按钮不被覆盖

                // **滚动区域**
                GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, textAreaHeight - 100)); // 留出一些边距
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width - 100), GUILayout.Height(textAreaHeight - 100));
                GUILayout.Label(errorMessageSB.ToString(), errorStyle);
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                // **底部关闭按钮**
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