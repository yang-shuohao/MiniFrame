using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;

namespace YSH.Framework.Editor
{
    public class PluginInstaller : EditorWindow
    {
        private static ListRequest listRequest;
        private static bool isAddressablesInstalled;

        [MenuItem("Tools/Framework/Plugin Installer", priority = int.MaxValue)]
        public static void ShowWindow()
        {
            PluginInstaller window = GetWindow<PluginInstaller>("�����װ");
            window.Show();
        }

        private void OnEnable()
        {
            CheckPluginsStatus();
        }

        private void OnGUI()
        {
            GUILayout.Label("��ѡ��Ҫ��װ�Ĳ��", EditorStyles.boldLabel);

            // Addressables ���
            if (isAddressablesInstalled)
            {
                GUILayout.Label("Addressables ����Ѱ�װ", EditorStyles.label);
                GUI.enabled = false; // ���ð�ť
            }
            else
            {
                if (GUILayout.Button("��װ Addressables"))
                {
                    InstallPlugin("com.unity.addressables", "Addressables");
                }
            }

            // �ָ���ť������״̬
            GUI.enabled = true;
        }

        private void CheckPluginsStatus()
        {
            // ��ȡ�Ѱ�װ�İ��б�������Ƿ��Ѱ�װ
            listRequest = Client.List();
            EditorApplication.update += Update;
        }

        private void Update()
        {
            if (listRequest.IsCompleted)
            {
                EditorApplication.update -= Update;

                if (listRequest.Status == StatusCode.Success)
                {
                    // ��� Addressables �Ƿ��Ѱ�װ
                    isAddressablesInstalled = IsPluginInstalled("com.unity.addressables");
                    Repaint(); // UI ˢ��
                }
                else
                {
                    Debug.LogError("��ȡ�Ѱ�װ����б�ʧ�ܣ�");
                }
            }
        }

        private bool IsPluginInstalled(string packageName)
        {
            // �����Ƿ��Ѿ���װ
            return listRequest.Result.Any(pkg => pkg.name == packageName);
        }

        private void InstallPlugin(string packageName, string pluginName)
        {
            bool confirm = EditorUtility.DisplayDialog($"��װ {pluginName}",
                $"{pluginName} ������ڿ�ܵĹ���������Ҫ���Ƿ�������װ��", "��", "��");

            if (confirm)
            {
                InstallPackage(packageName);
            }
        }

        private void InstallPackage(string packageName)
        {
            // ��װ�����
            var addRequest = Client.Add(packageName);

            // ������װ���
            EditorApplication.update += () => WaitForInstall(addRequest);
        }

        private void WaitForInstall(AddRequest addRequest)
        {
            if (addRequest.IsCompleted)
            {
                EditorApplication.update -= () => WaitForInstall(addRequest);

                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"�����װ�ɹ���{addRequest.Result.packageId}");
                    EditorUtility.DisplayDialog("��װ�ɹ�", $"{addRequest.Result.packageId} �Ѱ�װ��", "ȷ��");

                    // ��װ��ɺ����¼����״̬
                    CheckPluginsStatus();
                }
                else
                {
                    Debug.LogError($"�����װʧ�ܣ�{addRequest.Error.message}");
                    EditorUtility.DisplayDialog("��װʧ��", $"�����װʧ�ܣ�{addRequest.Error.message}", "ȷ��");
                }
            }
        }
    }
}
