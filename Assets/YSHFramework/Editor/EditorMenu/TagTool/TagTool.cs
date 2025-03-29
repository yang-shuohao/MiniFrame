using UnityEditor;
using System.IO;
using System.Text;

namespace YSH.Framework.Editor
{
    public class TagTool
    {
        //½Å±¾±£´æÂ·¾¶
        private static readonly string savFilePath = "Assets/Scripts/Constants/Tags.cs";

        [MenuItem("YSHFramework/TagTool/Generate Tags", priority = 2)]
        private static void GenerateTags()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static class Tags");
            sb.AppendLine("{");

            foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                sb.AppendLine($"    public static readonly string {tag.Replace(" ", "_")}_Tag = \"{tag}\";");
            }

            sb.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(savFilePath));
            File.WriteAllText(savFilePath, sb.ToString());

            AssetDatabase.Refresh();
        }
    }
}