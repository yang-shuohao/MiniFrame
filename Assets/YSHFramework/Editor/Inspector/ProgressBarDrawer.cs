using UnityEditor;
using UnityEngine;
using YSH.Framework.Attributes;

namespace YSH.Framework.EditorExtensions
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawer
    {
        // 颜色映射函数
        private Color GetColorFromEnum(EColor colorEnum)
        {
            switch (colorEnum)
            {
                case EColor.Red: return Color.red;
                case EColor.Green: return Color.green;
                case EColor.Blue: return Color.blue;
                case EColor.Yellow: return Color.yellow;
                case EColor.White: return Color.white;
                case EColor.Black: return Color.black;
                case EColor.Gray: return Color.gray;
                case EColor.Cyan: return Color.cyan;
                case EColor.Magenta: return Color.magenta;
                default: return Color.green;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var progressBarAttr = (ProgressBarAttribute)attribute;

            // 绘制标签（字段名）
            float labelHeight = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, labelHeight), progressBarAttr.label);

            // 在标签下方绘制进度条
            Rect barRect = new Rect(position.x, position.y + labelHeight + 2f, position.width, 20f);

            // 获取进度条当前值和最大值
            float currentValue = property.floatValue; // 假设进度条字段是 float 类型
            float maxValue = progressBarAttr.maxValue;

            // 绘制进度条背景
            EditorGUI.DrawRect(barRect, Color.gray);

            // 绘制进度条填充部分
            float progressWidth = Mathf.Clamp(currentValue / maxValue, 0f, 1f) * barRect.width;
            EditorGUI.DrawRect(new Rect(barRect.x, barRect.y, progressWidth, barRect.height), GetColorFromEnum(progressBarAttr.barColor));

            // 绘制进度值文本
            GUIStyle textStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = Color.black },
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUI.LabelField(barRect, $"{currentValue}/{maxValue}", textStyle);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 20f + 2f; // 标签高度 + 进度条高度 + 小的间距
        }
    }
}