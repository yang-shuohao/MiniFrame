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
                Debug.LogWarning("��ѡ��һ���ʲ���");
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
                Debug.LogWarning("��Ч���ʲ�·����");
                return;
            }

            sceneResults.Clear();
            projectResults.Clear();

            // ������Ŀ��������õ��ʲ�
            FindInProject(assetPath);
            // ���ҳ��������õ��ʲ�
            FindInScene(asset);
        }

        private static void FindInProject(string assetPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab t:ScriptableObject t:Material t:Scene"); // ��չ��������
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
            // �������м��صĳ���
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;

                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject rootObj in rootObjects)
                {
                    // �ݹ�������������壬����δ�����
                    FindInHierarchy(rootObj, asset);
                }
            }
        }

        private static void FindInHierarchy(GameObject obj, Object asset)
        {
            // ������������
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

            // �ݹ����������
            foreach (Transform child in obj.transform)
            {
                FindInHierarchy(child.gameObject, asset);
            }
        }

        private void OnGUI()
        {
            if (selectedAsset == null)
            {
                EditorGUILayout.LabelField("��ѡ��һ���ʲ���");
                return;
            }

            // ��ʾ��Ŀ����е�����
            EditorGUILayout.LabelField("��Ŀ�����ø��ʲ����ļ�:");
            foreach (string result in projectResults)
            {
                if (GUILayout.Button(result))
                {
                    SelectAndPingObject(result);
                }
            }

            // ��ʾ�����е�����
            EditorGUILayout.LabelField("���������ø��ʲ��Ķ���:");
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
            if (result.StartsWith("Assets")) // ��Ŀ�е��ʲ�
            {
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(result);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            else // �����еĶ���
            {
                GameObject obj = GameObject.Find(result);
                if (obj != null)
                {
                    // ѡ�в������������е�����
                    Selection.activeGameObject = obj;
                    EditorGUIUtility.PingObject(obj);

                    // ȷ�������ڲ���������ʾ
                    EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
                }
            }
        }
    }
}
