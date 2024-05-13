
using UnityEngine;

/// <summary>
/// ������������Ļ�����ӡ������Ϣ�Ͷ�ջ
/// </summary>
public class PrintErrorOnScreenMgr : Singleton<PrintErrorOnScreenMgr>
{
    private string errorMessage = null;
    private Vector2 scrollPosition = Vector2.zero;
    private bool showError = true;

    // �������������
    public PrintErrorOnScreenMgr()
    {
        Application.logMessageReceived += HandleLog;

        EventMgr.Instance.AddEventListener(MonoEventName.OnGUI, OnGUI);
    }

    // ��������
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // ��������Ϣ�Ͷ�ջ��Ϣ�洢�ڱ�����
            errorMessage = logString + "\n" + stackTrace;
        }
    }

    //��ʾ������Ϣ
    private void OnGUI()
    {
        if (showError && !string.IsNullOrEmpty(errorMessage))
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.wordWrap = true;
            style.fontSize = 30;
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.red;

            float boxWidth = Screen.width * 0.8f; // ��Ŀ��Ϊ��Ļ��ȵ�80%
            float boxHeight = Screen.height * 0.8f; // ��ĸ߶�Ϊ��Ļ�߶ȵ�80%
            float x = (Screen.width - boxWidth) * 0.5f;
            float y = (Screen.height - boxHeight) * 0.5f;

            Rect scrollViewRect = new Rect(x, y, boxWidth, boxHeight);

            // ��ȡ�ı���ʵ�ʸ߶�
            GUIStyle labelStyle = new GUIStyle(style);
            float contentHeight = labelStyle.CalcHeight(new GUIContent(errorMessage), boxWidth * 0.9f);

            Rect contentRect = new Rect(0, 0, boxWidth * 0.9f, contentHeight); // �����������ݴ�С�Ծ����Ƿ���Թ���

            scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, contentRect);
            GUI.Box(new Rect(0, 0, contentRect.width, contentRect.height), errorMessage, style);
            GUI.EndScrollView();

            // ��ʾ�رհ�ť
            float buttonWidth = 250f;
            float buttonHeight = 125f;
            if (GUI.Button(new Rect(Screen.width * 0.5f - buttonWidth * 0.5f, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Close"))
            {
                showError = false; // �رմ�����Ϣ
            }
        }
    }

}
