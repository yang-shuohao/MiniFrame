using UnityEditor;
using UnityEngine;
using YSH.Framework.Attributes;

namespace YSH.Framework.EditorExtensions
{
    [CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
    public class HorizontalLineDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            HorizontalLineAttribute line = (HorizontalLineAttribute)attribute;
            return line.Padding * 2 + line.Thickness + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            HorizontalLineAttribute line = (HorizontalLineAttribute)attribute;
            Color lineColor = ToUnityColor(line.LineColor);
            Color labelColor = ToUnityColor(line.LabelColor);

            float y = position.y + line.Padding;
            float lineY = y + EditorGUIUtility.singleLineHeight / 2f;

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = labelColor;
            labelStyle.fontSize = line.LabelSize;
            labelStyle.alignment = line.Alignment switch
            {
                TextAlignment.Left => TextAnchor.MiddleLeft,
                TextAlignment.Right => TextAnchor.MiddleRight,
                _ => TextAnchor.MiddleCenter
            };

            if (!string.IsNullOrEmpty(line.Label))
            {
                Vector2 labelSize = labelStyle.CalcSize(new GUIContent(line.Label));
                float margin = 6f;
                float labelWidth = labelSize.x;

                // 计算文字位置
                Rect labelRect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
                float labelX = line.Alignment switch
                {
                    TextAlignment.Left => position.x + margin,
                    TextAlignment.Right => position.x + position.width - labelWidth - margin,
                    _ => position.x + (position.width - labelWidth) / 2
                };

                // 线左边
                float leftLineWidth = labelX - position.x - margin;
                if (leftLineWidth > 0)
                    EditorGUI.DrawRect(new Rect(position.x, lineY, leftLineWidth, line.Thickness), lineColor);

                // 线右边
                float rightLineStart = labelX + labelWidth + margin;
                float rightLineWidth = position.x + position.width - rightLineStart;
                if (rightLineWidth > 0)
                    EditorGUI.DrawRect(new Rect(rightLineStart, lineY, rightLineWidth, line.Thickness), lineColor);

                // Label
                EditorGUI.LabelField(new Rect(labelX, y, labelWidth, EditorGUIUtility.singleLineHeight), line.Label, labelStyle);
            }
            else
            {
                // 没有文字就画整条线
                EditorGUI.DrawRect(new Rect(position.x, lineY, position.width, line.Thickness), lineColor);
            }
        }

        private Color ToUnityColor(EColor unityColor)
        {
            return unityColor switch
            {
                EColor.Red => Color.red,
                EColor.Green => Color.green,
                EColor.Blue => Color.blue,
                EColor.Yellow => Color.yellow,
                EColor.Cyan => Color.cyan,
                EColor.Magenta => Color.magenta,
                EColor.Gray => Color.gray,
                EColor.White => Color.white,
                EColor.Black => Color.black,
                _ => Color.gray
            };
        }
    }
}
