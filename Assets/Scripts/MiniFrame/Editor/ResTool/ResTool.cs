
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

public class ResTool
{
    /// <summary>
    /// 生成AA所有资源名
    /// </summary>
    [MenuItem("Tools/ResTool/GenerateAAResNames")]
    private static void GenerateAAResNames()
    {
        // 要遍历的文件夹路径
        string targetFolder = "Assets/GameRes";

        // 脚本保存路径
        string scriptFolder = "Assets/Scripts/MiniFrame/Constants";
        string scriptPath = Path.Combine(scriptFolder, "AAResNames.cs");

        // 创建脚本文件夹，如果不存在
        if (!Directory.Exists(scriptFolder))
        {
            Directory.CreateDirectory(scriptFolder);
        }

        // 获取所有文件
        string[] files = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);

        // 创建StringBuilder来构建脚本内容
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public static class ResNames");
        sb.AppendLine("{");

        // 正则表达式去除非字母数字字符和替换空格为下划线
        Regex invalidCharsRegex = new Regex("[^a-zA-Z0-9_]");
        Regex spaceRegex = new Regex("\\s+");

        // 遍历文件，添加静态字段
        foreach (string file in files)
        {
            // 忽略.meta文件
            if (file.EndsWith(".meta"))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExtension = Path.GetExtension(file).Replace(".", "_"); // 替换文件扩展名中的"."为"_"
            string sanitizedFileName = spaceRegex.Replace(fileName, "_");
            sanitizedFileName = invalidCharsRegex.Replace(sanitizedFileName, "");

            // Ensure unique field name
            string fieldName = $"{sanitizedFileName}{fileExtension}";

            sb.AppendLine($"    public static readonly string {fieldName} = \"{fileName}\";");
        }

        sb.AppendLine("}");

        // 写入脚本文件
        File.WriteAllText(scriptPath, sb.ToString());

        // 刷新AssetDatabase
        AssetDatabase.Refresh();

        UnityEngine.Debug.Log("ResNames.cs script generated successfully!");
    }


    /// <summary>
    /// 生成Resources文件夹下所有资源的映射名
    /// </summary>
    [MenuItem("Tools/ResTool/GenerateResNames")]
    private static void GenerateResNames()
    {
        // 要遍历的文件夹路径
        string targetFolder = "Assets/Resources";

        // 脚本保存路径
        string scriptFolder = "Assets/Scripts/MiniFrame/Constants";
        string scriptPath = Path.Combine(scriptFolder, "ResNames.cs");

        // 创建脚本文件夹，如果不存在
        if (!Directory.Exists(scriptFolder))
        {
            Directory.CreateDirectory(scriptFolder);
        }

        // 获取所有文件
        string[] files = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);

        // 创建StringBuilder来构建脚本内容
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public static class ResNames");
        sb.AppendLine("{");

        // 正则表达式去除非字母数字字符和替换空格为下划线
        Regex invalidCharsRegex = new Regex("[^a-zA-Z0-9_]");
        Regex spaceRegex = new Regex("\\s+");

        // 遍历文件，添加静态字段
        foreach (string file in files)
        {
            // 忽略.meta文件
            if (file.EndsWith(".meta"))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExtension = Path.GetExtension(file).Replace(".", "_"); // 替换文件扩展名中的"."为"_"
            string sanitizedFileName = spaceRegex.Replace(fileName, "_");
            sanitizedFileName = invalidCharsRegex.Replace(sanitizedFileName, "");

            // Ensure unique field name
            string fieldName = $"{sanitizedFileName}{fileExtension}";

            string assetPath = GetResourcePath(file);

            sb.AppendLine($"    public static readonly string {fieldName} = \"{assetPath}\";");
        }

        sb.AppendLine("}");

        // 写入脚本文件
        File.WriteAllText(scriptPath, sb.ToString());

        // 刷新AssetDatabase
        AssetDatabase.Refresh();

        UnityEngine.Debug.Log("ResNames.cs script generated successfully!");
    }

    /// <summary>
    /// 获取资源文件相对于 Resources 文件夹的路径，不包括扩展名
    /// </summary>
    /// <param name="fullPath">资源文件的完整路径</param>
    /// <returns>相对于 Resources 文件夹的路径，不包括扩展名</returns>
    public static string GetResourcePath(string fullPath)
    {
        // 确保路径使用正斜杠
        fullPath = fullPath.Replace("\\", "/");

        // 找到 Resources 文件夹的位置
        int resourcesIndex = fullPath.IndexOf("Resources/");
        if (resourcesIndex == -1)
        {
            return null; // 不是 Resources 文件夹下的文件
        }

        // 获取相对路径
        string relativePath = fullPath.Substring(resourcesIndex + "Resources/".Length);

        // 移除文件的扩展名
        relativePath = Path.ChangeExtension(relativePath, null);

        return relativePath;
    }
}
