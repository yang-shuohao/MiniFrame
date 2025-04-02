using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace YSH.Framework.Editor
{
    public class GenerateUICodeMenu
    {
        [MenuItem("GameObject/YSHFrameWork/Generate UI Code", false, 10)]
        private static void GenerateUICode()
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

            InsertCodeIntoScript(scriptPath, selectedObj);
        }

        private static string FindScriptPath(string scriptName)
        {
            string[] guids = AssetDatabase.FindAssets(scriptName + " t:Script");
            if (guids.Length == 0) return null;
            return AssetDatabase.GUIDToAssetPath(guids[0]);
        }

        private static void InsertCodeIntoScript(string scriptPath, GameObject selectedObj)
        {
            // **收集 UI 组件**
            var uiElements = new Dictionary<string, string>(); // 变量名 -> 类型
            foreach (var img in selectedObj.GetComponentsInChildren<Image>(true))
                if (img.gameObject.name.StartsWith("img"))
                    uiElements[img.gameObject.name] = "Image";

            foreach (var btn in selectedObj.GetComponentsInChildren<Button>(true))
                if (btn.gameObject.name.StartsWith("btn"))
                    uiElements[btn.gameObject.name] = "Button";

            if (uiElements.Count == 0)
            {
                Debug.LogWarning($"未找到符合条件的 UI 组件，无需修改代码！");
                return;
            }

            string[] lines = File.ReadAllLines(scriptPath);
            StringBuilder newScript = new StringBuilder();
            bool insideClass = false;
            bool awakeExists = false;
            bool insideAwake = false;
            bool fieldInserted = false;
            bool onClickExists = false;
            bool insideOnClick = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("public class") || line.StartsWith("class"))
                    insideClass = true;

                if (insideClass && line == "{")
                {
                    newScript.AppendLine(lines[i]);
                    foreach (var element in uiElements)
                    {
                        string fieldDeclaration = $"    private {element.Value} {element.Key};";
                        if (!lines.Any(l => l.Contains(fieldDeclaration)))
                        {
                            newScript.AppendLine(fieldDeclaration);
                            fieldInserted = true;
                        }
                    }
                    continue;
                }

                if (line.StartsWith("protected override void Awake()") || line.StartsWith("void Awake()"))
                {
                    awakeExists = true;
                    insideAwake = true;
                }

                if (insideAwake && line == "{")
                {
                    newScript.AppendLine(lines[i]);
                    if (!lines.Any(l => l.Contains("base.Awake();")))
                        newScript.AppendLine("        base.Awake();");

                    foreach (var element in uiElements)
                    {
                        string initCode = $"        {element.Key} = GetControl<{element.Value}>(nameof({element.Key}));";
                        if (!lines.Any(l => l.Contains(initCode)))
                            newScript.AppendLine(initCode);
                    }

                    insideAwake = false;
                    continue;
                }

                if (line.StartsWith("protected override void OnClick("))
                {
                    onClickExists = true;
                    insideOnClick = true;
                }

                if (insideOnClick && line == "{")
                {
                    newScript.AppendLine(lines[i]);
                    foreach (var element in uiElements.Where(e => e.Value == "Button"))
                    {
                        string caseCode = $"        case \"{element.Key}\":";
                        if (!lines.Any(l => l.Contains(caseCode)))
                        {
                            newScript.AppendLine(caseCode);
                            newScript.AppendLine("            break;");
                        }
                    }

                    insideOnClick = false;
                    continue;
                }

                if (i == lines.Length - 1 && !awakeExists)
                {
                    newScript.AppendLine();
                    newScript.AppendLine("    protected override void Awake()");
                    newScript.AppendLine("    {");
                    newScript.AppendLine("        base.Awake();");

                    foreach (var element in uiElements)
                        newScript.AppendLine($"        {element.Key} = GetControl<{element.Value}>(nameof({element.Key}));");

                    newScript.AppendLine("    }");
                }

                if (i == lines.Length - 1 && !onClickExists && uiElements.Any(e => e.Value == "Button"))
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

            if (fieldInserted || !awakeExists || !onClickExists)
            {
                File.WriteAllText(scriptPath, newScript.ToString());
                AssetDatabase.Refresh();
                Debug.Log($"已成功修改 {scriptPath}，插入 UI 代码！");
            }
            else
            {
                Debug.LogWarning($"脚本 {scriptPath} 已包含所有字段，无需修改！");
            }
        }
    }
}
