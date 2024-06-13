
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
            // ���ô��ڵľ�������
            Rect windowRect = new Rect(10, 10, Screen.width - 20, Screen.height - 20);

            // ��ʼ���ڻ���
            GUILayout.BeginArea(windowRect, GUI.skin.window);

            // �������м���ʾ��־����
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("��־", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            // �����Ͻ���ʾ�رհ�ť
            if (GUILayout.Button("�ر�", GUILayout.Width(100), GUILayout.Height(40))) // ���ð�ť��СΪԭ��������
            {
                errorMessageSB.Clear();
            }
            GUILayout.EndHorizontal();

            // ��ʾ��־���ݲ���Ӵ�ֱ������
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));
            GUIStyle logStyle = new GUIStyle(GUI.skin.label);
            logStyle.normal.textColor = Color.red; // �����ı���ɫΪ��ɫ
            logStyle.fontSize = 20;
            GUILayout.Label(errorMessageSB.ToString(), logStyle);
            GUILayout.EndScrollView();

            // �������ڻ���
            GUILayout.EndArea();
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
