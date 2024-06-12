
using System.IO;
using System.Text;
using UnityEngine;

public class LogMgr : MonoSingleton<LogMgr>
{
    //是否启用普通日志
    public bool isEnableDebugLog;
    //是否启用警告日志
    public bool isEnableDebugLogWarning;
    //是否启用错误日志
    public bool isEnableDebugLogError;
    //是否打印错误日志到屏幕
    public bool isPrintErroronScreen;

    //
    private StringBuilder logSB;
    //日志文件保存路径
    private string logFileSavePath;

    //错误数据
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
    /// 普通调试日志
    /// </summary>
    public void Log(object message, Object context = null)
    {
        if (isEnableDebugLog)
        {
            Debug.Log(message, context);
        }
    }

    /// <summary>
    /// 警告调试日志
    /// </summary>
    public void LogWarning(object message, Object context = null)
    {
        if (isEnableDebugLogWarning)
        {
            Debug.LogWarning(message, context);
        }
    }

    /// <summary>
    /// 错误调试日志
    /// </summary>
    public void LogError(object message, Object context = null)
    {
        if (isEnableDebugLogError)
        {
            Debug.LogError(message, context);
        }
    }

    /// <summary>
    /// 打印日志回调
    /// </summary>
    /// <param name="logString">日志文本</param>
    /// <param name="stackTrace">调用堆栈</param>
    /// <param name="type">日志类型</param>
    private void OnLogCallBack(string logString, string stackTrace, LogType type)
    {
        logSB.Append(logString);
        logSB.Append("\n");
        logSB.Append(stackTrace);
        logSB.Append("\n");

        if (logSB.Length > 0)
        {
            // 将错误消息和堆栈信息存储在变量中
            if (isPrintErroronScreen && (type == LogType.Error || type == LogType.Exception))
            {
                errorMessageSB.Append(logString);
                errorMessageSB.Append("\n");
                errorMessageSB.Append(stackTrace);
                errorMessageSB.Append("\n");
            }

            //保存日志到文件
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
    /// 在屏幕上面打印错误信息
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

            float boxWidth = Screen.width * 0.8f; // 框的宽度为屏幕宽度的80%
            float boxHeight = Screen.height * 0.8f; // 框的高度为屏幕高度的80%
            float x = (Screen.width - boxWidth) * 0.5f;
            float y = (Screen.height - boxHeight) * 0.5f;

            Rect scrollViewRect = new Rect(x, y, boxWidth, boxHeight);

            // 获取文本的实际高度
            GUIStyle labelStyle = new GUIStyle(style);
            float contentHeight = labelStyle.CalcHeight(new GUIContent(errorMessageSB.ToString()), boxWidth * 0.9f);

            Rect contentRect = new Rect(0, 0, boxWidth * 0.9f, contentHeight); // 这里设置内容大小以决定是否可以滚动

            scrollPosition = GUI.BeginScrollView(scrollViewRect, scrollPosition, contentRect);
            GUI.Box(new Rect(0, 0, contentRect.width, contentRect.height), errorMessageSB.ToString(), style);
            GUI.EndScrollView();

            // 显示关闭按钮
            float buttonWidth = 250f;
            float buttonHeight = 125f;
            if (GUI.Button(new Rect(Screen.width * 0.5f - buttonWidth * 0.5f, Screen.height - buttonHeight, buttonWidth, buttonHeight), "Close"))
            {
                errorMessageSB = null;
            }
        }
    }

    /// <summary>
    /// 上传日志到服务器
    /// </summary>
    /// <param name="url"></param>
    public void UploadLog(string url)
    {
        LogUploader.UploadLog(url, logFileSavePath);
    }
   
}
