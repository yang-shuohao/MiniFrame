using System;
using System.Collections.Generic;
using System.Linq;

namespace YSH.Framework
{
    // GM 命令数据类
    public class GMCommand
    {
        public string Name;          // 命令名称
        public string Description;   // 命令描述
        public Action<string[]> Action; // 命令执行方法（接受多个参数）

        public GMCommand(string name, string description, Action<string[]> action)
        {
            Name = name;
            Description = description;
            Action = action;
        }
    }

    public partial class GMCmd
    {
        public Dictionary<string, GMCommand> gmCommands = new Dictionary<string, GMCommand>();

        public void ExecuteCommand(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string cmd = parts[0]; // 命令名称
            string[] parameters = parts.Skip(1).ToArray(); // 参数数组

            if (gmCommands.TryGetValue(cmd, out var command))
            {
                command.Action?.Invoke(parameters);
            }
            else
            {
                LogMgr.Instance.LogError($"未知 GM 命令: {cmd}");
            }
        }

        public List<GMCommand> GetMatchingCommands(string input)
        {
            return gmCommands.Values.Where(cmd => cmd.Name.ToLower().Contains(input.ToLower())).ToList();
        }
    }
}

