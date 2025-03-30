
using UnityEngine;
using UnityEditor;

#region �����־˫����Դ����

#if UNITY_EDITOR
public static class LogTraceUtil
{
    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    static bool OnOpenAsset(int instanceID, int line)
    {
        string stackTrace = GetStackTrace();
        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("LogMgr:Log"))
        {
            // ʹ��������ʽƥ��at���ĸ��ű�����һ��
            var matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string pathLine = "";
            while (matches.Success)
            {
                pathLine = matches.Groups[1].Value;

                if (!pathLine.Contains("LogMgr.cs"))
                {
                    int splitIndex = pathLine.LastIndexOf(":");
                    // �ű�·��
                    string path = pathLine.Substring(0, splitIndex);
                    // �к�
                    line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                    fullPath = fullPath + path;
                    // ��ת��Ŀ�������ض���
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
    /// ��ȡ��ǰ��־����ѡ�е���־�Ķ�ջ��Ϣ
    /// </summary>
    /// <returns></returns>
    static string GetStackTrace()
    {
        // ͨ�������ȡConsoleWindow��
        var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        // ��ȡ����ʵ��
        var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic);
        var consoleInstance = fieldInfo.GetValue(null);
        if (consoleInstance != null)
        {
            if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
            {
                // ��ȡm_ActiveText��Ա
                fieldInfo = ConsoleWindowType.GetField("m_ActiveText",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);
                // ��ȡm_ActiveText��ֵ
                string activeText = fieldInfo.GetValue(consoleInstance).ToString();
                return activeText;
            }
        }
        return null;
    }
}
#endif

#endregion �����־˫����Դ����
