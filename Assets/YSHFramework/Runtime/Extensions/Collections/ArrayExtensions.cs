using System;
using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Extensions
{

    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArrayExtensions
    {
        #region 一维数组

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsValidIndex<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        /// <summary>
        /// 交换指定两个索引的元素
        /// </summary>
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            if (array.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (!IsValidIndex(array, index1) || !IsValidIndex(array, index2))
            {
                throw new ArgumentOutOfRangeException("Index is out of range.");
            }

            if (index1 != index2)
            {
                T temp = array[index1];
                array[index1] = array[index2];
                array[index2] = temp;
            }
        }

        /// <summary>
        /// 查找一个符合条件的元素
        /// </summary>
        public static T FindBy<T>(this T[] array, Func<T, bool> condition)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (condition(array[i]))
                {
                    return array[i];
                }
            }

            return default;
        }

        /// <summary>
        /// 查找所有符合条件的元素
        /// </summary>
        public static List<T> FindAllBy<T>(this T[] array, Func<T, bool> condition)
        {
            List<T> result = new List<T>();

            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (condition(array[i]))
                {
                    result.Add(array[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// 升序
        /// </summary>
        public static void OrderBy<T, Q>(this T[] array, Func<T, Q> condition)
        {
            Array.Sort(array, (x, y) => Comparer<Q>.Default.Compare(condition(x), condition(y)));
        }

        /// <summary>
        /// 降序
        /// </summary>
        public static void OrderByDescending<T, Q>(this T[] array, Func<T, Q> condition)
        {
            Array.Sort(array, (x, y) => Comparer<Q>.Default.Compare(condition(y), condition(x)));
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        public static T MaxBy<T, Q>(this T[] array, Func<T, Q> selector)
        {
            if (array.IsNullOrEmpty())
            {
                throw new ArgumentException("Array is empty or null.");
            }

            T maxElement = array[0];
            Q maxValue = selector(maxElement);

            for (int i = 1; i < array.Length; i++)
            {
                Q value = selector(array[i]);
                if (Comparer<Q>.Default.Compare(value, maxValue) > 0)
                {
                    maxElement = array[i];
                    maxValue = value;
                }
            }

            return maxElement;
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        public static T MinBy<T, Q>(this T[] array, Func<T, Q> selector)
        {
            if (array.IsNullOrEmpty())
            {
                throw new ArgumentException("Array is empty or null.");
            }

            T minElement = array[0];
            Q minValue = selector(minElement);

            for (int i = 1; i < array.Length; i++)
            {
                Q value = selector(array[i]);
                if (Comparer<Q>.Default.Compare(value, minValue) < 0)
                {
                    minElement = array[i];
                    minValue = value;
                }
            }

            return minElement;
        }

        #endregion

        #region 二维数组

        public static bool IsNullOrEmpty<T>(this T[,] array)
        {
            return array == null || array.GetLength(0) == 0 || array.GetLength(1) == 0;
        }

        public static bool IsValidIndex<T>(this T[,] array, int row, int col)
        {
            return row >= 0 && row < array.GetLength(0) && col >= 0 && col < array.GetLength(1);
        }

        public static bool IsValidIndex<T>(this T[,] array, Vector2Int rowCol)
        {
            return array.IsValidIndex(rowCol.x, rowCol.y);
        }

        /// <summary>
        /// 根据元素找到行列索引
        /// </summary>
        public static Vector2Int FindElementIndex<T>(this T[,] array, T element)
        {
            int rowCount = array.GetLength(0);
            int colCount = array.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (array[row, col].Equals(element))
                    {
                        return new Vector2Int(row, col);
                    }
                }
            }

            return Vector2Int.left;
        }

        /// <summary>
        /// 判断是否包含某个元素
        /// </summary>
        public static bool IsContains<T>(this T[,] array, T element)
        {
            int rowCount = array.GetLength(0);
            int colCount = array.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (array[row, col].Equals(element))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }

}
