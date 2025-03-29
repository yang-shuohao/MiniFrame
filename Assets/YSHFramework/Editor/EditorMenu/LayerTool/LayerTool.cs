using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace YSH.Framework.Editor
{
    public class LayerTool
    {
        private static readonly string savFilePath = "Assets/Scripts/Constants/Layers.cs";

        [MenuItem("YSHFramework/LayerTool/Generate Layers", priority = 1)]
        private static void GenerateLayers()
        {
            var sb = new StringBuilder();
            sb.AppendLine("public static class Layers");
            sb.AppendLine("{");

            foreach (var layer in UnityEditorInternal.InternalEditorUtility.layers)
            {
                sb.AppendLine($"    public static readonly string {layer.Replace(" ", "_")}_Layer = \"{layer}\";");
            }

            sb.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(savFilePath));
            File.WriteAllText(savFilePath, sb.ToString());

            AssetDatabase.Refresh();

            Debug.Log($"<color=cyan>Layers.cs 生成成功：</color> {savFilePath}");
        }
    }
}
