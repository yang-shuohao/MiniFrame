using UnityEngine;

namespace YSH.Framework.Attributes
{
    public class ProgressBarAttribute : PropertyAttribute
    {
        public string label;
        public float maxValue;
        public EColor barColor;

        public ProgressBarAttribute(string label, float maxValue, EColor barColor = EColor.Green)
        {
            this.label = label;
            this.maxValue = maxValue;
            this.barColor = barColor;
        }
    }
}
