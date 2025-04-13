using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using YSH.Framework.Attributes;

namespace YSH.Framework.EditorExtensions
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class InspectorButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetType = target.GetType();

            // 限制仅 MonoBehaviour
            if (!(target is MonoBehaviour monoTarget))
                return;

            var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(InspectorButtonAttribute), true);
                foreach (InspectorButtonAttribute attr in attributes)
                {
                    // 参数个数必须为0
                    if (method.GetParameters().Length > 0)
                    {
                        EditorGUILayout.HelpBox($"[InspectorButton] 无法绑定带参数的方法：{method.Name}", MessageType.Warning);
                        continue;
                    }

                    // 根据模式决定是否显示
                    bool shouldDraw = attr.ExecuteMode switch
                    {
                        InspectorButtonAttribute.Mode.EditorOnly => !Application.isPlaying,
                        InspectorButtonAttribute.Mode.PlayModeOnly => Application.isPlaying,
                        _ => true
                    };

                    if (!shouldDraw) continue;

                    string label = string.IsNullOrEmpty(attr.ButtonLabel) ? method.Name : attr.ButtonLabel;
                    if (GUILayout.Button(label))
                    {
                        Undo.RecordObject(target, $"Invoke {method.Name}");
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
}