using UnityEditor;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Reflection;
using YSH.Framework.Attributes;

namespace YSH.Framework.EditorExtensions
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            if (EvaluateCondition(property, showIf.Expression))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;
            return EvaluateCondition(property, showIf.Expression)
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0f;
        }

        private bool EvaluateCondition(SerializedProperty property, string expression)
        {
            // 支持的表达式 pattern: "field == value"
            Match match = Regex.Match(expression, @"^\s*(\w+)\s*(==|!=|>|<|>=|<=)\s*(\w+)\s*$");

            if (!match.Success) return true; // 语法不对就默认显示

            string fieldName = match.Groups[1].Value;
            string op = match.Groups[2].Value;
            string compareValue = match.Groups[3].Value;

            SerializedObject so = property.serializedObject;
            SerializedProperty fieldProp = so.FindProperty(fieldName);

            if (fieldProp == null) return true;

            object fieldVal = GetSerializedValue(fieldProp);

            object parsedVal = ParseStringToType(compareValue, fieldVal?.GetType(), so.targetObject.GetType());

            if (fieldVal == null || parsedVal == null) return true;

            int comp = Comparer(fieldVal, parsedVal);

            return op switch
            {
                "==" => comp == 0,
                "!=" => comp != 0,
                ">" => comp > 0,
                "<" => comp < 0,
                ">=" => comp >= 0,
                "<=" => comp <= 0,
                _ => true,
            };
        }

        private object GetSerializedValue(SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Boolean: return prop.boolValue;
                case SerializedPropertyType.Enum: return prop.enumNames[prop.enumValueIndex];
                case SerializedPropertyType.Integer: return prop.intValue;
                case SerializedPropertyType.Float: return prop.floatValue;
                case SerializedPropertyType.String: return prop.stringValue;
            }
            return null;
        }

        private object ParseStringToType(string input, Type type, Type targetClassType)
        {
            try
            {
                if (type == typeof(bool)) return bool.Parse(input);
                if (type == typeof(int)) return int.Parse(input);
                if (type == typeof(float)) return float.Parse(input);
                if (type == typeof(string)) return input;

                // 如果是枚举
                if (type.IsEnum)
                {
                    foreach (var field in targetClassType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        if (field.FieldType == type)
                        {
                            return Enum.Parse(type, input);
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private int Comparer(object a, object b)
        {
            if (a is int ai && b is int bi) return ai.CompareTo(bi);
            if (a is float af && b is float bf) return af.CompareTo(bf);
            if (a is string sa && b is string sb) return sa.CompareTo(sb);
            if (a is bool ab && b is bool bb) return ab.CompareTo(bb);
            if (a is string es1 && b is string es2) return es1.CompareTo(es2); // 枚举字符串
            return 0;
        }
    }
}