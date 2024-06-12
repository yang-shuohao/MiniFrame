
using System;
using System.Reflection;

public class GMCommandMgr : Singleton<GMCommandMgr>
{
    /// <summary>
    /// ����������ַ������÷���
    /// </summary>
    /// <param name="command"></param>
    public void ExecuteCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            LogMgr.Instance.LogWarning("Command is empty");
            return;
        }

        // �������Ͳ���
        string[] parts = command.Split(' ');
        if (parts.Length == 0)
        {
            LogMgr.Instance.LogWarning("Invalid command format");
            return;
        }

        string methodName = parts[0];
        string[] stringArgs = parts.Length > 1 ? parts[1..] : new string[0];

        // ��ȡ������Ϣ
        MethodInfo method = typeof(GMCommandMgr).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
        if (method == null)
        {
            LogMgr.Instance.LogWarning($"Method {methodName} not found");
            return;
        }

        // ��������
        ParameterInfo[] parameters = method.GetParameters();
        if (parameters.Length != stringArgs.Length)
        {
            LogMgr.Instance.LogWarning($"Parameter count mismatch for method {methodName}");
            return;
        }

        object[] args = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            try
            {
                args[i] = Convert.ChangeType(stringArgs[i], parameters[i].ParameterType);
            }
            catch (Exception e)
            {
                LogMgr.Instance.LogWarning($"Error converting parameter {stringArgs[i]} to {parameters[i].ParameterType}: {e.Message}");
                return;
            }
        }

        // ���÷���
        try
        {
            method?.Invoke(this, args);
        }
        catch (Exception e)
        {
            LogMgr.Instance.LogError($"Error invoking method {methodName}: {e.Message}");
        }
    }

    #region GM����

    public void AddLevel(int level)
    {
        LogMgr.Instance.Log($"AddLevel called with level: {level}");
    }

    public void ChangeSpeed(float speed)
    {
        LogMgr.Instance.Log($"ChangeSpeed called with speed: {speed}");
    }

    #endregion
}
