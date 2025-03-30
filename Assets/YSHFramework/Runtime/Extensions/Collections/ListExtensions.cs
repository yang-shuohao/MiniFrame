using System;
using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (!list.IsValidIndex(index1) || !list.IsValidIndex(index2))
                throw new ArgumentOutOfRangeException($"Index out of range: index1={index1}, index2={index2}");
            if (index1 != index2)
            {
                T temp = list[index1];
                list[index1] = list[index2];
                list[index2] = temp;
            }
        }

        public static bool IsValidIndex<T>(this List<T> list, int index)
            => index >= 0 && index < list.Count;

        public static void OrderBy<T, Q>(this List<T> list, Func<T, Q> selector) where Q : IComparable
            => list.Sort((x, y) => selector(x).CompareTo(selector(y)));

        public static void OrderByDescending<T, Q>(this List<T> list, Func<T, Q> selector) where Q : IComparable<Q>
            => list.Sort((x, y) => selector(y).CompareTo(selector(x)));

        public static bool IsContains(this List<Vector3> list, Vector3 target)
            => list.Exists(v => v == target);

        public static void RemoveNulls<T>(this List<T> list) where T : class
            => list.RemoveAll(item => item == null);

        public static T MaxBy<T, Q>(this List<T> list, Func<T, Q> selector)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("List is empty or null.");
            }

            T maxElement = list[0];
            Q maxValue = selector(maxElement);

            int count = list.Count;
            for (int i = 1; i < count; i++)
            {
                Q value = selector(list[i]);
                if (Comparer<Q>.Default.Compare(value, maxValue) > 0)
                {
                    maxElement = list[i];
                    maxValue = value;
                }
            }

            return maxElement;
        }

        public static T MinBy<T, Q>(this List<T> list, Func<T, Q> selector)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("Array is empty or null.");
            }

            T minElement = list[0];
            Q minValue = selector(minElement);

            int count = list.Count;
            for (int i = 1; i < count; i++)
            {
                Q value = selector(list[i]);
                if (Comparer<Q>.Default.Compare(value, minValue) < 0)
                {
                    minElement = list[i];
                    minValue = value;
                }
            }

            return minElement;
        }

        public static T FindBy<T>(this List<T> list, Func<T, bool> condition)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (condition(list[i]))
                {
                    return list[i];
                }
            }

            return default;
        }

        public static List<T> FindAllBy<T>(this List<T> list, Func<T, bool> condition)
        {
            List<T> result = new List<T>();

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (condition(list[i]))
                {
                    result.Add(list[i]);
                }
            }

            return result;
        }
    }
}

