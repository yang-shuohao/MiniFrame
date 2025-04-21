using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 引入 TMP_Text 相关命名空间
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


/// <summary>
/// 还不是特别适用
/// 注意：
/// 1、添加命名空间会生成失败
/// 2、增加新控件再次生成可以正确添加
/// 3、删除控件再次生成不能移除
/// </summary>
namespace YSH.Framework.EditorExtensions
{
    public class UICodeGeneratorMenu
    {
        [MenuItem("GameObject/YSH FrameWork/UI Code Generator", false, 10)]
        private static void UICodeGenerator()
        {
            GameObject selectedObj = Selection.activeGameObject;
            if (selectedObj == null)
            {
                Debug.LogError("请先选中一个 UI 物体！");
                return;
            }

            if (selectedObj.GetComponent<RectTransform>() == null)
            {
                Debug.LogError($"选中的物体 {selectedObj.name} 不是 UI 物体（缺少 RectTransform 组件）！");
                return;
            }

            string scriptName = selectedObj.name;
            Component targetComponent = selectedObj.GetComponent(scriptName);
            if (targetComponent == null)
            {
                Debug.LogError($"选中的物体 {selectedObj.name} 没有找到同名脚本 {scriptName}！");
                return;
            }

            if (!(targetComponent is BaseUI))
            {
                Debug.LogError($"选中的物体 {selectedObj.name} 的脚本 {scriptName} 必须继承自 BaseUI！");
                return;
            }

            string scriptPath = FindScriptPath(scriptName);
            if (string.IsNullOrEmpty(scriptPath))
            {
                Debug.LogError($"无法找到脚本 {scriptName}.cs，请确保它在 Assets 目录下！");
                return;
            }

            Dictionary<string, string> uiElements = new Dictionary<string, string>(); // 初始化字典
            CollectUIElements(selectedObj, uiElements); // 收集 UI 元素

            InsertCodeIntoScript(scriptPath, selectedObj, uiElements); // 插入代码
        }

        private static string FindScriptPath(string scriptName)
        {
            string[] guids = AssetDatabase.FindAssets(scriptName + " t:Script");
            if (guids.Length == 0) return null;
            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }

        private static void InsertCodeIntoScript(string scriptPath, GameObject selectedObj, Dictionary<string, string> uiElements)
        {
            string[] lines = File.ReadAllLines(scriptPath);
            StringBuilder newScript = new StringBuilder();

            bool isInsideClass = false;
            bool isAwakeExists = false;
            bool isOnClickExists = false;
            bool isAddFieldCompleted = false;
            bool isinsideOnClickSwitch = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("public class"))
                    isInsideClass = true;

                // 添加字段
                if (isInsideClass && line == "{" && !isAddFieldCompleted)
                {
                    newScript.AppendLine(lines[i]);
                    foreach (var element in uiElements)
                    {
                        string fieldDeclaration = $"    private {element.Value} {element.Key};";
                        if (!lines.Any(l => l.Contains(fieldDeclaration)))
                        {
                            newScript.AppendLine(fieldDeclaration);
                        }
                    }
                    continue;
                }

                if (line.StartsWith("protected override void Awake"))
                {
                    isAwakeExists = true;
                    isAddFieldCompleted = true;
                }

                // Awake
                if (isAwakeExists && line.StartsWith("base.Awake"))
                {
                    newScript.AppendLine(lines[i]);
                    foreach (var element in uiElements)
                    {
                        string initCode = $"        {element.Key} = GetControl<{element.Value}>(nameof({element.Key}));";
                        if (!lines.Any(l => l.Contains(initCode)))
                            newScript.AppendLine(initCode);
                    }
                    continue;
                }

                if (line.StartsWith("protected override void OnClick"))
                {
                    isOnClickExists = true;
                }

                if (isOnClickExists && line.StartsWith("switch"))
                {
                    isinsideOnClickSwitch = true;
                }

                // 插入按钮 OnClick
                if (isinsideOnClickSwitch && line == "{")
                {
                    newScript.AppendLine(lines[i]);
                    foreach (var element in uiElements.Where(e => e.Value == "Button"))
                    {
                        string caseCode = $"            case \"{element.Key}\":";
                        if (!lines.Any(l => l.Contains(caseCode)))
                        {
                            newScript.AppendLine(caseCode);
                            newScript.AppendLine("                break;");
                        }
                    }
                    continue;
                }

                // 不存在的方法直接插入
                if (i == lines.Length - 1 && !isAwakeExists)
                {
                    newScript.AppendLine();
                    newScript.AppendLine("    protected override void Awake()");
                    newScript.AppendLine("    {");
                    newScript.AppendLine("        base.Awake();");

                    foreach (var element in uiElements)
                        newScript.AppendLine($"        {element.Key} = GetControl<{element.Value}>(nameof({element.Key}));");

                    newScript.AppendLine("    }");
                }

                if (i == lines.Length - 1 && !isOnClickExists && uiElements.Any(e => e.Value == "Button"))
                {
                    newScript.AppendLine();
                    newScript.AppendLine("    protected override void OnClick(string btnName)");
                    newScript.AppendLine("    {");
                    newScript.AppendLine("        switch (btnName)");
                    newScript.AppendLine("        {");

                    foreach (var element in uiElements.Where(e => e.Value == "Button"))
                    {
                        newScript.AppendLine($"            case \"{element.Key}\":");
                        newScript.AppendLine("                break;");
                    }

                    newScript.AppendLine("        }");
                    newScript.AppendLine("    }");
                }

                newScript.AppendLine(lines[i]);
            }

            File.WriteAllText(scriptPath, newScript.ToString());
            AssetDatabase.Refresh();
            Debug.Log($"已成功修改 {scriptPath}，插入 UI 代码！");
        }

        private static void CollectUIElements(GameObject selectedObj, Dictionary<string, string> uiElements)
        {
            AddUIElement<Image>(selectedObj, uiElements, "img", "Image");
            AddUIElement<TMP_Text>(selectedObj, uiElements, "txt", "TMP_Text");
            AddUIElement<Button>(selectedObj, uiElements, "btn", "Button");
            AddUIElement<Toggle>(selectedObj, uiElements, "tgl", "Toggle");
            AddUIElement<RawImage>(selectedObj, uiElements, "rimg", "RawImage");
        }

        private static void AddUIElement<T>(GameObject selectedObj, Dictionary<string, string> uiElements, string namePrefix, string controlType) where T : Component
        {
            foreach (var element in selectedObj.GetComponentsInChildren<T>(true))
            {
                if (element.gameObject.name.StartsWith(namePrefix))
                {
                    uiElements[element.gameObject.name] = controlType;
                }
            }
        }
    }
}
