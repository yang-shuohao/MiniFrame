
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
            // 设置窗口的矩形区域
            Rect windowRect = new Rect(10, 10, Screen.width - 20, Screen.height - 20);

            // 开始窗口绘制
            GUILayout.BeginArea(windowRect, GUI.skin.window);

            // 最上面中间显示日志标题
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("日志", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            // 最右上角显示关闭按钮
            if (GUILayout.Button("关闭", GUILayout.Width(100), GUILayout.Height(40))) // 设置按钮大小为原来的两倍
            {
                errorMessageSB.Clear();
            }
            GUILayout.EndHorizontal();

            // 显示日志内容并添加垂直滚动条
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowRect.width - 20), GUILayout.Height(windowRect.height - 60));
            GUIStyle logStyle = new GUIStyle(GUI.skin.label);
            logStyle.normal.textColor = Color.red; // 设置文本颜色为红色
            logStyle.fontSize = 20;
            GUILayout.Label(errorMessageSB.ToString(), logStyle);
            GUILayout.EndScrollView();

            // 结束窗口绘制
            GUILayout.EndArea();
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
