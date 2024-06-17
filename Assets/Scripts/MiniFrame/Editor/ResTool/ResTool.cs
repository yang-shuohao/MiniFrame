
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

public class ResTool
{
    /// <summary>
    /// ����AA������Դ��
    /// </summary>
    [MenuItem("Tools/ResTool/GenerateAAResNames")]
    private static void GenerateAAResNames()
    {
        // Ҫ�������ļ���·��
        string targetFolder = "Assets/GameRes";

        // �ű�����·��
        string scriptFolder = "Assets/Scripts/MiniFrame/Constants";
        string scriptPath = Path.Combine(scriptFolder, "AAResNames.cs");

        // �����ű��ļ��У����������
        if (!Directory.Exists(scriptFolder))
        {
            Directory.CreateDirectory(scriptFolder);
        }

        // ��ȡ�����ļ�
        string[] files = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);

        // ����StringBuilder�������ű�����
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public static class ResNames");
        sb.AppendLine("{");

        // ������ʽȥ������ĸ�����ַ����滻�ո�Ϊ�»���
        Regex invalidCharsRegex = new Regex("[^a-zA-Z0-9_]");
        Regex spaceRegex = new Regex("\\s+");

        // �����ļ�����Ӿ�̬�ֶ�
        foreach (string file in files)
        {
            // ����.meta�ļ�
            if (file.EndsWith(".meta"))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExtension = Path.GetExtension(file).Replace(".", "_"); // �滻�ļ���չ���е�"."Ϊ"_"
            string sanitizedFileName = spaceRegex.Replace(fileName, "_");
            sanitizedFileName = invalidCharsRegex.Replace(sanitizedFileName, "");

            // Ensure unique field name
            string fieldName = $"{sanitizedFileName}{fileExtension}";

            sb.AppendLine($"    public static readonly string {fieldName} = \"{fileName}\";");
        }

        sb.AppendLine("}");

        // д��ű��ļ�
        File.WriteAllText(scriptPath, sb.ToString());

        // ˢ��AssetDatabase
        AssetDatabase.Refresh();

        UnityEngine.Debug.Log("ResNames.cs script generated successfully!");
    }


    /// <summary>
    /// ����Resources�ļ�����������Դ��ӳ����
    /// </summary>
    [MenuItem("Tools/ResTool/GenerateResNames")]
    private static void GenerateResNames()
    {
        // Ҫ�������ļ���·��
        string targetFolder = "Assets/Resources";

        // �ű�����·��
        string scriptFolder = "Assets/Scripts/MiniFrame/Constants";
        string scriptPath = Path.Combine(scriptFolder, "ResNames.cs");

        // �����ű��ļ��У����������
        if (!Directory.Exists(scriptFolder))
        {
            Directory.CreateDirectory(scriptFolder);
        }

        // ��ȡ�����ļ�
        string[] files = Directory.GetFiles(targetFolder, "*.*", SearchOption.AllDirectories);

        // ����StringBuilder�������ű�����
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public static class ResNames");
        sb.AppendLine("{");

        // ������ʽȥ������ĸ�����ַ����滻�ո�Ϊ�»���
        Regex invalidCharsRegex = new Regex("[^a-zA-Z0-9_]");
        Regex spaceRegex = new Regex("\\s+");

        // �����ļ�����Ӿ�̬�ֶ�
        foreach (string file in files)
        {
            // ����.meta�ļ�
            if (file.EndsWith(".meta"))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExtension = Path.GetExtension(file).Replace(".", "_"); // �滻�ļ���չ���е�"."Ϊ"_"
            string sanitizedFileName = spaceRegex.Replace(fileName, "_");
            sanitizedFileName = invalidCharsRegex.Replace(sanitizedFileName, "");

            // Ensure unique field name
            string fieldName = $"{sanitizedFileName}{fileExtension}";

            string assetPath = GetResourcePath(file);

            sb.AppendLine($"    public static readonly string {fieldName} = \"{assetPath}\";");
        }

        sb.AppendLine("}");

        // д��ű��ļ�
        File.WriteAllText(scriptPath, sb.ToString());

        // ˢ��AssetDatabase
        AssetDatabase.Refresh();

        UnityEngine.Debug.Log("ResNames.cs script generated successfully!");
    }

    /// <summary>
    /// ��ȡ��Դ�ļ������ Resources �ļ��е�·������������չ��
    /// </summary>
    /// <param name="fullPath">��Դ�ļ�������·��</param>
    /// <returns>����� Resources �ļ��е�·������������չ��</returns>
    public static string GetResourcePath(string fullPath)
    {
        // ȷ��·��ʹ����б��
        fullPath = fullPath.Replace("\\", "/");

        // �ҵ� Resources �ļ��е�λ��
        int resourcesIndex = fullPath.IndexOf("Resources/");
        if (resourcesIndex == -1)
        {
            return null; // ���� Resources �ļ����µ��ļ�
        }

        // ��ȡ���·��
        string relativePath = fullPath.Substring(resourcesIndex + "Resources/".Length);

        // �Ƴ��ļ�����չ��
        relativePath = Path.ChangeExtension(relativePath, null);

        return relativePath;
    }
}
