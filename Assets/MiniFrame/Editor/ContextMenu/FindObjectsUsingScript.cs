using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindObjectsUsingScript
{
    [MenuItem("Assets/Tools/Find Objects Using Script", false, 1200)]
    private static void FindObjects()
    {
        MonoScript selectedScript = Selection.activeObject as MonoScript;

        if (selectedScript == null)
        {
            Debug.LogError("Please select a script file.");
            return;
        }

        System.Type scriptType = selectedScript.GetClass();

        if (scriptType == null)
        {
            Debug.LogError("Unable to get script class type.");
            return;
        }

        List<string> sceneObjects = FindObjectsInScene(scriptType);
        List<string> projectAssets = FindObjectsInProject(scriptType);

        PrintResults(selectedScript.name, sceneObjects, projectAssets);
    }

    // 验证右键菜单项
    [MenuItem("Assets/Tools/Find Objects Using Script", true)]
    private static bool ValidateFindObjects()
    {
        return Selection.activeObject is MonoScript;
    }

    // 在场景中查找使用指定脚本的对象
    private static List<string> FindObjectsInScene(System.Type scriptType)
    {
        List<string> sceneObjects = new List<string>();
        MonoBehaviour[] allSceneObjects = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allSceneObjects)
        {
            // Check if the object has the specified script type
            if (obj.GetType() == scriptType)
            {
                sceneObjects.Add(obj.gameObject.name);
            }
        }
        return sceneObjects;
    }

    // 在项目中查找使用指定脚本的资源
    private static List<string> FindObjectsInProject(System.Type scriptType)
    {
        List<string> projectAssets = new List<string>();
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssetPaths)
        {
            // Load the asset at the path
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (asset != null)
            {
                // Check if the asset has the specified script type
                var components = asset.GetComponents<MonoBehaviour>();
                foreach (var component in components)
                {
                    if (component.GetType() == scriptType)
                    {
                        projectAssets.Add(assetPath);
                        break; // No need to check further components of this asset
                    }
                }
            }
        }
        return projectAssets;
    }

    // 打印查找结果
    private static void PrintResults(string scriptName, List<string> sceneObjects, List<string> projectAssets)
    {
        Debug.Log("Objects using script " + scriptName + " in the scene:");
        if (sceneObjects.Count > 0)
        {
            foreach (var name in sceneObjects)
            {
                Debug.Log(name);
            }
        }
        else
        {
            Debug.Log("No objects in the scene are using the script " + scriptName);
        }

        Debug.Log("Objects using script " + scriptName + " in the project:");
        if (projectAssets.Count > 0)
        {
            foreach (var assetPath in projectAssets)
            {
                Debug.Log(assetPath);
            }
        }
        else
        {
            Debug.Log("No assets in the project are using the script " + scriptName);
        }
    }
}
