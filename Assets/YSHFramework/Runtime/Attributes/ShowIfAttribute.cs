using UnityEngine;

namespace YSH.Framework.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string FieldName { get; private set; }
        public object CompareValue { get; private set; }
        public string Expression { get; private set; }

        // bool字段简写
        public ShowIfAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        // 字段 + 值
        public ShowIfAttribute(string fieldName, object compareValue)
        {
            FieldName = fieldName;
            CompareValue = compareValue;
        }

        // 表达式
        public ShowIfAttribute(string expression, bool isExpression = true)
        {
            Expression = expression;
        }
    }
}
