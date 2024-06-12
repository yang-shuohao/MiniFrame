
using System.IO;
using System.Text;
using UnityEngine;

public class LogMgr : MonoSingleton<LogMgr>
{
    //�Ƿ�������ͨ��־
    public bool isEnableDebugLog;
    //�Ƿ����þ�����־
    public bool isEnableDebugLogWarning;
    //�Ƿ����ô�����־
    public bool isEnableDebugLogError;
    //�Ƿ��ӡ������־����Ļ
    public bool isPrintErroronScreen;

    //
    private StringBuilder logSB;
    //��־�ļ�����·��
    private string logFileSavePath;

    //��������
    private StringBuilder errorMessageSB;
    private Vector2 scrollPosition = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();

        isEnableDebugLog = true;
        isEnableDebugLogWarning = true;
        isEnableDebugLogError = true;
        isPrintErroronScreen = true;

        logSB = new StringBuilder();
        errorMessageSB = new StringBuilder();

        logFileSavePath = string.Format("{0}/{1}_Log_{2}", Application.persistentDataPath, Application.productName, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

        Application.logMessageReceived += OnLogCallBack;
    }

    /// <summary>
    /// ��ͨ������־
    /// </summary>
    public void Log(object message, Object context = null)
    {
        if (isEnableDebugLog)
        {
            Debug.Log(message, context);
        }
    }

    /// <summary>
    /// ���������־
    /// </summary>
    public void LogWarning(object message, Object context = null)
    {
        if (isEnableDebugLogWarning)
        {
            Debug.LogWarning(message, context);
        }
    }

    /// <summary>
    /// ���������־
    /// </summary>
    public void LogError(object message, Object context = null)
    {
        if (isEnableDebugLogError)
        {
            Debug.LogError(message, context);
        }
    }

    /// <summary>
    /// ��ӡ��־�ص�
    /// </summary>
    /// <param name="logString">��־�ı�</param>
    /// <param name="stackTrace">���ö�ջ</param>
    /// <param name="type">��־����</param>
    private void OnLogCallBack(string logString, string stackTrace, LogType type)
    {
        logSB.Append(logString);
        logSB.Append("\n");
        logSB.Append(stackTrace);
        logSB.Append("\n");

        if (logSB.Length > 0)
        {
            // ��������Ϣ�Ͷ�ջ��Ϣ�洢�ڱ�����
            if (isPrintErroronScreen && (type == LogType.Error || type == LogType.Exception))
            {
                errorMessageSB.Append(logString);
                errorMessageSB.Append("\n");
                errorMessageSB.Append(stackTrace);
                errorMessageSB.Append("\n");
            }

            //������־���ļ�
            if (!File.Exists(logFileSavePath))
            {
                var fs = File.Create(logFileSavePath);
                fs.Close();
            }
            using (var sw = File.AppendText(logFileSavePath))
            {
                sw.WriteLine(logSB.ToString());
            }
            logSB.Remove(0, logSB.Length);
        }
    }

    /// <summary>
    /// ����Ļ�����ӡ������Ϣ
    /// </summary>
    private void OnGUI()
    {
        if (errorMessageSB != null && errorMessageSB.Length > 0)
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
            float contentHeight = labelStyle.CalcHeight(new GUIContent(errorMessageSB.ToString()), boxWidth * 0.9f);

            Rect contentRect = new Rect(0, 0, boxWidth * 0.9f, contentHeight); // �����������ݴ�С�Ծ����Ƿ���Թ���

            scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, contentRect);
            GUI.Box(new Rect(0, 0, contentRect.width, contentRect.height), errorMessageSB.ToString(), style);
            GUI.EndScrollView();

            // ��ʾ�رհ�ť
            float buttonWidth = 250f;
            float buttonHeight = 125f;
            if (GUI.Button(new Rect(Screen.width * 0.5f - buttonWidth * 0.5f, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Close"))
            {
                errorMessageSB = null;
            }
        }
    }

    /// <summary>
    /// �ϴ���־��������
    /// </summary>
    /// <param name="url"></param>
    public void UploadLog(string url)
    {
        LogUploader.UploadLog(url, logFileSavePath);
    }
   
}
