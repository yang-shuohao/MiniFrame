using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using YSH.Framework.Attributes;

namespace YSH.Framework.EditorExtensions
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                string[] layerNames = GetSortingLayerNames();
                int currentIndex = System.Array.IndexOf(layerNames, property.stringValue);
                if (currentIndex < 0) currentIndex = 0;

                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, layerNames);
                property.stringValue = layerNames[newIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [SortingLayer] with string");
            }
        }

        private string[] GetSortingLayerNames()
        {
            var layersProp = typeof(InternalEditorUtility)
                .GetProperty("sortingLayerNames", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            return (string[])layersProp.GetValue(null, null);
        }
    }
}