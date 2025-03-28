using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YSH.Framework.Extensions;

namespace YSH.Framework.Editor
{
    public class AssetUsageFinder : EditorWindow
    {
        private static Object selectedAsset;
        private static List<string> sceneResults = new List<string>();
        private static List<string> projectResults = new List<string>();

        [MenuItem("Assets/Tools/Find References In Scene and Project")]
        public static void OpenWindow()
        {
            selectedAsset = Selection.activeObject;
            if (selectedAsset == null)
            {
                Debug.LogWarning("请选择一个资产！");
                return;
            }

            FindReferences(selectedAsset);
            AssetUsageFinder window = GetWindow<AssetUsageFinder>("Asset Usage Finder");
            window.Show();
        }

        private static void FindReferences(Object asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning("无效的资产路径。");
                return;
            }

            sceneResults.Clear();
            projectResults.Clear();

            // 查找项目面板中引用的资产
            FindInProject(assetPath);
            // 查找场景中引用的资产
            FindInScene(asset);
        }

        private static void FindInProject(string assetPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab t:ScriptableObject t:Material t:Scene"); // 扩展搜索类型
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string[] dependencies = AssetDatabase.GetDependencies(path, true);

                foreach (string dependency in dependencies)
                {
                    if (dependency == assetPath)
                    {
                        projectResults.Add(path);
                        break;
                    }
                }
            }
        }

        private static void FindInScene(Object asset)
        {
            // 遍历所有加载的场景
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;

                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject rootObj in rootObjects)
                {
                    // 递归查找所有子物体，包括未激活的
                    FindInHierarchy(rootObj, asset);
                }
            }
        }

        private static void FindInHierarchy(GameObject obj, Object asset)
        {
            // 查找物体的组件
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                SerializedObject serializedObject = new SerializedObject(comp);
                SerializedProperty prop = serializedObject.GetIterator();
                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (prop.objectReferenceValue == asset)
                        {
                            sceneResults.Add(obj.transform.GetFullHierarchyPath());
                            break;
                        }
                    }
                }
            }

            // 递归查找子物体
            foreach (Transform child in obj.transform)
            {
                FindInHierarchy(child.gameObject, asset);
            }
        }

        private void OnGUI()
        {
            if (selectedAsset == null)
            {
                EditorGUILayout.LabelField("请选择一个资产！");
                return;
            }

            // 显示项目面板中的引用
            EditorGUILayout.LabelField("项目中引用该资产的文件:");
            foreach (string result in projectResults)
            {
                if (GUILayout.Button(result))
                {
                    SelectAndPingObject(result);
                }
            }

            // 显示场景中的引用
            EditorGUILayout.LabelField("场景中引用该资产的对象:");
            foreach (string result in sceneResults)
            {
                if (GUILayout.Button(result))
                {
                    SelectAndPingObject(result);
                }
            }
        }

        private void SelectAndPingObject(string result)
        {
            if (result.StartsWith("Assets")) // 项目中的资产
            {
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(result);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            else // 场景中的对象
            {
                GameObject obj = GameObject.Find(result);
                if (obj != null)
                {
                    // 选中并高亮层次面板中的物体
                    Selection.activeGameObject = obj;
                    EditorGUIUtility.PingObject(obj);

                    // 确保物体在层次面板中显示
                    EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
                }
            }
        }
    }
}
