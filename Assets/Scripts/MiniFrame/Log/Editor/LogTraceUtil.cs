
using UnityEngine;
using UnityEditor;

#region 解决日志双击溯源问题

#if UNITY_EDITOR
public static class LogTraceUtil
{
    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    static bool OnOpenAsset(int instanceID, int line)
    {
        string stackTrace = GetStackTrace();
        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("LogMgr:Log"))
        {
            // 使用正则表达式匹配at的哪个脚本的哪一行
            var matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string pathLine = "";
            while (matches.Success)
            {
                pathLine = matches.Groups[1].Value;

                if (!pathLine.Contains("LogMgr.cs"))
                {
                    int splitIndex = pathLine.LastIndexOf(":");
                    // 脚本路径
                    string path = pathLine.Substring(0, splitIndex);
                    // 行号
                    line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                    fullPath = fullPath + path;
                    // 跳转到目标代码的特定行
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                    break;
                }
                matches = matches.NextMatch();
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取当前日志窗口选中的日志的堆栈信息
    /// </summary>
    /// <returns></returns>
    static string GetStackTrace()
    {
        // 通过反射获取ConsoleWindow类
        var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        // 获取窗口实例
        var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic);
        var consoleInstance = fieldInfo.GetValue(null);
        if (consoleInstance != null)
        {
            if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
            {
                // 获取m_ActiveText成员
                fieldInfo = ConsoleWindowType.GetField("m_ActiveText",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);
                // 获取m_ActiveText的值
                string activeText = fieldInfo.GetValue(consoleInstance).ToString();
                return activeText;
            }
        }
        return null;
    }
}
#endif

#endregion 解决日志双击溯源问题
