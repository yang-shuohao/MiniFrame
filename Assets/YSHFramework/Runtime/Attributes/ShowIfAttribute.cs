using UnityEngine;

namespace YSH.Framework.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string Expression { get; private set; }

        public ShowIfAttribute(string expression)
        {
            Expression = expression;
        }
    }
}