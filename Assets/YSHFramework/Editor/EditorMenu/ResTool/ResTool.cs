using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace YSH.Framework.Editor
{

    public class ResTool
    {
        private static readonly string ScriptFolder = "Assets/Scripts/Constants";

        /// <summary>
        /// ���� AA ��Դ��
        /// </summary>
        [MenuItem("YSHFramework/ResTool/Generate AAResNames", priority = 3)]
        private static void GenerateAAResNames()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressable Asset Settings not found.");
                return;
            }

            Dictionary<string, string> resDict = new Dictionary<string, string>();
            foreach (var group in settings.groups)
            {
                if (group == null || group.entries == null || group.Name == "Built In Data") continue;

                foreach (var entry in group.entries)
                {
                    string address = entry.address;
                    string extension = Path.GetExtension(entry.AssetPath).Replace(".", "_");
                    string fieldName = GenerateValidFieldName(address, extension, resDict);
                    resDict[fieldName] = address;
                }
            }

            GenerateScript("AAResNames.cs", "AAResNames", resDict);
        }

        /// <summary>
        /// ���� Resources ��Դ��
        /// </summary>
        [MenuItem("YSHFramework/ResTool/Generate ResNames", priority = 4)]
        private static void GenerateResNames()
        {
            string targetFolder = "Assets/Resources";
            if (!Directory.Exists(targetFolder))
            {
                Debug.LogError("Resources folder not found.");
                return;
            }

            Dictionary<string, string> resDict = new Dictionary<string, string>();
            foreach (string file in Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".meta")) continue;

                string fileName = Path.GetFileNameWithoutExtension(file);
                string extension = Path.GetExtension(file).Replace(".", "_");
                string resourcePath = GetResourcePath(file);

                if (!string.IsNullOrEmpty(resourcePath))
                {
                    string fieldName = GenerateValidFieldName(fileName, extension, resDict);
                    resDict[fieldName] = resourcePath;
                }
            }

            GenerateScript("ResNames.cs", "ResNames", resDict);
        }

        /// <summary>
        /// ���� C# �������ļ�
        /// </summary>
        private static void GenerateScript(string fileName, string className, Dictionary<string, string> resDict)
        {
            string scriptPath = Path.Combine(ScriptFolder, fileName);

            if (!Directory.Exists(ScriptFolder))
            {
                Directory.CreateDirectory(ScriptFolder);
            }

            StringBuilder sb = new StringBuilder(resDict.Count * 50);
            sb.AppendLine($"public static class {className}");
            sb.AppendLine("{");

            foreach (var kvp in resDict)
            {
                sb.AppendLine($"    public const string {kvp.Key} = \"{kvp.Value}\";");
            }

            sb.AppendLine("}");

            File.WriteAllText(scriptPath, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log($"{fileName} script generated successfully!");
        }

        /// <summary>
        /// ����Ψһ��Ч���ֶ���
        /// </summary>
        private static string GenerateValidFieldName(string name, string extension, Dictionary<string, string> existingFields)
        {
            // �ϲ��ո��滻�ͷǷ��ַ��Ƴ�������Ч��
            string sanitized = Regex.Replace(name, "[^a-zA-Z0-9_]", "_");
            string fieldName = $"{sanitized}{extension}";

            // ȷ��Ψһ��
            int counter = 1;
            string uniqueFieldName = fieldName;
            while (existingFields.ContainsKey(uniqueFieldName))
            {
                uniqueFieldName = $"{fieldName}_{counter++}";
            }

            return uniqueFieldName;
        }

        /// <summary>
        /// ��ȡ��Դ�ļ������ Resources �ļ��е�·������������չ����
        /// </summary>
        private static string GetResourcePath(string fullPath)
        {
            fullPath = fullPath.Replace("\\", "/");
            int index = fullPath.IndexOf("Resources/");
            return index >= 0 ? Path.ChangeExtension(fullPath.Substring(index + 10), null) : null;
        }
    }
}
