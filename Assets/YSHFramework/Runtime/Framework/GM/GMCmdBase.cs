using System;
using System.Collections.Generic;
using System.Linq;

namespace YSH.Framework
{
    // GM ����������
    public class GMCommand
    {
        public string Name;          // ��������
        public string Description;   // ��������
        public Action<string[]> Action; // ����ִ�з��������ܶ��������

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

            string cmd = parts[0]; // ��������
            string[] parameters = parts.Skip(1).ToArray(); // ��������

            if (gmCommands.TryGetValue(cmd, out var command))
            {
                command.Action?.Invoke(parameters);
            }
            else
            {
                LogMgr.Instance.LogError($"δ֪ GM ����: {cmd}");
            }
        }

        public List<GMCommand> GetMatchingCommands(string input)
        {
            return gmCommands.Values.Where(cmd => cmd.Name.ToLower().Contains(input.ToLower())).ToList();
        }
    }
}

