
using UnityEngine;

/// <summary>
/// 遇到错误在屏幕上面打印错误信息和堆栈
/// </summary>
public class PrintErrorOnScreenMgr : Singleton<PrintErrorOnScreenMgr>
{
    private string errorMessage = null;
    private Vector2 scrollPosition = Vector2.zero;
    private bool showError = true;

    // 在这里监听错误
    public PrintErrorOnScreenMgr()
    {
        Application.logMessageReceived += HandleLog;

        EventMgr.Instance.AddEventListener(MonoEventName.OnGUI, OnGUI);
    }

    // 错误处理方法
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // 将错误消息和堆栈信息存储在变量中
            errorMessage = logString + "\n" + stackTrace;
        }
    }

    //显示错误信息
    private void OnGUI()
    {
        if (showError && !string.IsNullOrEmpty(errorMessage))
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.wordWrap = true;
            style.fontSize = 30;
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.red;

            float boxWidth = Screen.width * 0.8f; // 框的宽度为屏幕宽度的80%
            float boxHeight = Screen.height * 0.8f; // 框的高度为屏幕高度的80%
            float x = (Screen.width - boxWidth) * 0.5f;
            float y = (Screen.height - boxHeight) * 0.5f;

            Rect scrollViewRect = new Rect(x, y, boxWidth, boxHeight);

            // 获取文本的实际高度
            GUIStyle labelStyle = new GUIStyle(style);
            float contentHeight = labelStyle.CalcHeight(new GUIContent(errorMessage), boxWidth * 0.9f);

            Rect contentRect = new Rect(0, 0, boxWidth * 0.9f, contentHeight); // 这里设置内容大小以决定是否可以滚动

            scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, contentRect);
            GUI.Box(new Rect(0, 0, contentRect.width, contentRect.height), errorMessage, style);
            GUI.EndScrollView();

            // 显示关闭按钮
            float buttonWidth = 250f;
            float buttonHeight = 125f;
            if (GUI.Button(new Rect(Screen.width * 0.5f - buttonWidth * 0.5f, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Close"))
            {
                showError = false; // 关闭错误信息
            }
        }
    }

}
