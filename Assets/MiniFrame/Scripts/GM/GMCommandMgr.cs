using System;
using System.Collections.Generic;
using System.Reflection;

public class GMCommandMgr : Singleton<GMCommandMgr>
{
    private Dictionary<string, string> commandDescriptions = new Dictionary<string, string>();

    public GMCommandMgr()
    {
        // 初始化方法描述
        commandDescriptions["AddLevel"] = "增加等级";
        commandDescriptions["ChangeSpeed"] = "改变速度";
    }

    /// <summary>
    /// 根据输入的字符串调用方法
    /// </summary>
    /// <param name="command"></param>
    public void ExecuteCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            LogMgr.Instance.LogWarning("Command is empty");
            return;
        }

        // 拆分命令和参数
        string[] parts = command.Split(' ');
        if (parts.Length == 0)
        {
            LogMgr.Instance.LogWarning("Invalid command format");
            return;
        }

        string methodName = parts[0];
        string[] stringArgs = parts.Length > 1 ? parts[1..] : new string[0];

        // 获取方法信息
        MethodInfo method = typeof(GMCommandMgr).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
        if (method == null)
        {
            LogMgr.Instance.LogWarning($"Method {methodName} not found");
            return;
        }

        // 解析参数
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

        // 调用方法
        try
        {
            method?.Invoke(this, args);
        }
        catch (Exception e)
        {
            LogMgr.Instance.LogError($"Error invoking method {methodName}: {e.Message}");
        }
    }

    /// <summary>
    /// 获取匹配的命令
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string[] GetMatchingCommands(string input)
    {
        List<string> matchingCommands = new List<string>();
        MethodInfo[] methods = typeof(GMCommandMgr).GetMethods(BindingFlags.Instance | BindingFlags.Public);
        foreach (MethodInfo method in methods)
        {
            if (method.Name.StartsWith(input, StringComparison.OrdinalIgnoreCase))
            {
                matchingCommands.Add(method.Name);
            }
        }
        return matchingCommands.ToArray();
    }

    /// <summary>
    /// 获取命令的描述
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string GetCommandDescription(string command)
    {
        return commandDescriptions.TryGetValue(command, out var description) ? description : "No description available.";
    }


    #region GM命令

    public void AddLevel(int level)
    {
        LogMgr.Instance.Log($"AddLevel called with level: {level}");
    }

    public void ChangeSpeed(float speed)
    {
        LogMgr.Instance.Log($"ChangeSpeed called with speed: {speed}");
    }

    // 添加更多 GM 命令

    #endregion
}
