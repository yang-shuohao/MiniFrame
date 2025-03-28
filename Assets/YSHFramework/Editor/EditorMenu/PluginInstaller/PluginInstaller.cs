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
            PluginInstaller window = GetWindow<PluginInstaller>("插件安装");
            window.Show();
        }

        private void OnEnable()
        {
            CheckPluginsStatus();
        }

        private void OnGUI()
        {
            GUILayout.Label("请选择要安装的插件", EditorStyles.boldLabel);

            // Addressables 插件
            if (isAddressablesInstalled)
            {
                GUILayout.Label("Addressables 插件已安装", EditorStyles.label);
                GUI.enabled = false; // 禁用按钮
            }
            else
            {
                if (GUILayout.Button("安装 Addressables"))
                {
                    InstallPlugin("com.unity.addressables", "Addressables");
                }
            }

            // 恢复按钮的启用状态
            GUI.enabled = true;
        }

        private void CheckPluginsStatus()
        {
            // 获取已安装的包列表并检查插件是否已安装
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
                    // 检查 Addressables 是否已安装
                    isAddressablesInstalled = IsPluginInstalled("com.unity.addressables");
                    Repaint(); // UI 刷新
                }
                else
                {
                    Debug.LogError("获取已安装插件列表失败！");
                }
            }
        }

        private bool IsPluginInstalled(string packageName)
        {
            // 检查包是否已经安装
            return listRequest.Result.Any(pkg => pkg.name == packageName);
        }

        private void InstallPlugin(string packageName, string pluginName)
        {
            bool confirm = EditorUtility.DisplayDialog($"安装 {pluginName}",
                $"{pluginName} 插件对于框架的功能至关重要，是否立即安装？", "是", "否");

            if (confirm)
            {
                InstallPackage(packageName);
            }
        }

        private void InstallPackage(string packageName)
        {
            // 安装插件包
            var addRequest = Client.Add(packageName);

            // 监听安装完成
            EditorApplication.update += () => WaitForInstall(addRequest);
        }

        private void WaitForInstall(AddRequest addRequest)
        {
            if (addRequest.IsCompleted)
            {
                EditorApplication.update -= () => WaitForInstall(addRequest);

                if (addRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"插件安装成功：{addRequest.Result.packageId}");
                    EditorUtility.DisplayDialog("安装成功", $"{addRequest.Result.packageId} 已安装！", "确定");

                    // 安装完成后重新检查插件状态
                    CheckPluginsStatus();
                }
                else
                {
                    Debug.LogError($"插件安装失败：{addRequest.Error.message}");
                    EditorUtility.DisplayDialog("安装失败", $"插件安装失败：{addRequest.Error.message}", "确定");
                }
            }
        }
    }
}
