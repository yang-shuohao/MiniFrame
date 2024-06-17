
using System.IO;
using System.Text;
using UnityEditor;

public class LayerTool
{
    //½Å±¾±£´æÂ·¾¶
    private static readonly string savFilePath = "Assets/Scripts/MiniFrame/Constants/Layers.cs";

    [MenuItem("Tools/Tag&LayerTool/GenerateLayers")]
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
    }
}
