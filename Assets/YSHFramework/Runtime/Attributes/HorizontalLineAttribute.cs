using UnityEngine;

namespace YSH.Framework.Attributes
{
    public enum EColor
    {
        Red, Green, Blue, Yellow, Cyan, Magenta, Gray, White, Black
    }

    public class HorizontalLineAttribute : PropertyAttribute
    {
        public EColor LineColor;
        public float Thickness;
        public float Padding;
        public string Label;

        public TextAlignment Alignment;
        public EColor LabelColor;
        public int LabelSize;

        public HorizontalLineAttribute(
            EColor lineColor = EColor.Gray,
            float thickness = 1f,
            float padding = 10f,
            string label = "",
            TextAlignment alignment = TextAlignment.Center,
            int labelSize = 12,
            EColor labelColor = EColor.Gray)
        {
            LineColor = lineColor;
            Thickness = thickness;
            Padding = padding;
            Label = label;
            Alignment = alignment;
            LabelSize = labelSize;
            LabelColor = labelColor;
        }
    }
}