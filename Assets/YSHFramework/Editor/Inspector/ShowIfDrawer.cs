using UnityEditor;
using UnityEngine;
using YSH.Framework.Attributes;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace YSH.Framework.EditorExtensions
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        private bool ShouldShow(SerializedProperty property)
        {
            ShowIfAttribute attr = (ShowIfAttribute)attribute;

            if (!string.IsNullOrEmpty(attr.Expression))
            {
                return EvaluateExpression(property.serializedObject, attr.Expression);
            }

            if (!string.IsNullOrEmpty(attr.FieldName) && attr.CompareValue != null)
            {
                return CompareValue(property.serializedObject, attr.FieldName, attr.CompareValue);
            }

            if (!string.IsNullOrEmpty(attr.FieldName))
            {
                return EvaluateBoolField(property.serializedObject, attr.FieldName);
            }

            return true;
        }

        private bool EvaluateBoolField(SerializedObject so, string fieldName)
        {
            SerializedProperty sp = so.FindProperty(fieldName);
            return sp != null && sp.propertyType == SerializedPropertyType.Boolean && sp.boolValue;
        }

        private bool CompareValue(SerializedObject so, string fieldName, object expected)
        {
            SerializedProperty sp = so.FindProperty(fieldName);
            if (sp == null) return true;

            object actual = GetSerializedValue(sp);

            // 枚举用字符串比较
            if (actual is string actualStr && expected is Enum)
                return actualStr == expected.ToString();

            return Equals(actual, expected);
        }

        private bool EvaluateExpression(SerializedObject so, string expr)
        {
            // 只支持简单表达式 field op value（支持空格）
            var match = Regex.Match(expr, @"^\s*(\w+)\s*(==|!=|>|<|>=|<=)\s*(\w+)\s*$");
            if (!match.Success) return true;

            string fieldName = match.Groups[1].Value;
            string op = match.Groups[2].Value;
            string valueStr = match.Groups[3].Value;

            SerializedProperty sp = so.FindProperty(fieldName);
            if (sp == null) return true;

            object actual = GetSerializedValue(sp);
            object expected = TryParse(valueStr, actual?.GetType());

            if (actual == null || expected == null)
                return true;

            return CompareWithOperator(actual, expected, op);
        }

        private object TryParse(string value, Type targetType)
        {
            try
            {
                if (targetType == typeof(int)) return int.Parse(value);
                if (targetType == typeof(float)) return float.Parse(value);
                if (targetType == typeof(bool)) return bool.Parse(value);
                if (targetType == typeof(string)) return value;
                if (targetType.IsEnum) return Enum.Parse(targetType, value);
            }
            catch { }

            return null;
        }

        private object GetSerializedValue(SerializedProperty sp)
        {
            return sp.propertyType switch
            {
                SerializedPropertyType.Boolean => sp.boolValue,
                SerializedPropertyType.Integer => sp.intValue,
                SerializedPropertyType.Float => sp.floatValue,
                SerializedPropertyType.String => sp.stringValue,
                SerializedPropertyType.Enum => sp.enumNames[sp.enumValueIndex],
                _ => null
            };
        }

        private bool CompareWithOperator(object a, object b, string op)
        {
            try
            {
                float fa = Convert.ToSingle(a);
                float fb = Convert.ToSingle(b);

                return op switch
                {
                    "==" => fa == fb,
                    "!=" => fa != fb,
                    ">" => fa > fb,
                    "<" => fa < fb,
                    ">=" => fa >= fb,
                    "<=" => fa <= fb,
                    _ => true
                };
            }
            catch
            {
                return op switch
                {
                    "==" => Equals(a, b),
                    "!=" => !Equals(a, b),
                    _ => true
                };
            }
        }
    }
}
