#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static class AddressableDefineChecker
{
    const string Define = "USE_ADDRESSABLES";

    static AddressableDefineChecker()
    {
        CheckAddressables();
    }

    static void CheckAddressables()
    {
        // 是否有 Addressables 命名空间的程序集
        bool hasAddressables = CompilationPipeline
            .GetAssemblies()
            .Any(a => a.name.Contains("Unity.Addressables"));

        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (hasAddressables && !defines.Contains(Define))
        {
            defines += ";" + Define;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            Debug.Log("添加 Addressables define: " + Define);
        }
        else if (!hasAddressables && defines.Contains(Define))
        {
            defines = defines.Replace(Define, "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            Debug.Log("移除 Addressables define: " + Define);
        }
    }
}
#endif
