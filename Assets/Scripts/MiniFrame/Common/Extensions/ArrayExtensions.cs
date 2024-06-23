using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数组扩展
/// </summary>
public static class ArrayExtensions
{
    #region 一维数组

    /// <summary>
    /// 交换指定两个索引的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public static void Swap<T>(this T[] array, int index1, int index2)
    {
        if (array == null)
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
    /// 判断指定索引是否有效
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsValidIndex<T>(this T[] array, int index)
    {
        return index >= 0 && index < array.Length;
    }

    /// <summary>
    /// 查找一个符合条件的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static T FindBy<T>(this T[] array, Func<T, bool> condition)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
            {
                return array[i];
            }
        }

        return default(T);
    }

    /// <summary>
    /// 查找所有符合条件的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static List<T> FindAllBy<T>(this T[] array, Func<T, bool> condition)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < array.Length; i++)
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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="array"></param>
    /// <param name="condition"></param>
    public static void OrderBy<T, Q>(this T[] array, Func<T, Q> condition)
    {
        Array.Sort(array, (x, y) => Comparer<Q>.Default.Compare(condition(x), condition(y)));
    }

    /// <summary>
    /// 降序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="array"></param>
    /// <param name="condition"></param>
    public static void OrderByDescending<T, Q>(this T[] array, Func<T, Q> condition)
    {
        Array.Sort(array, (x, y) => Comparer<Q>.Default.Compare(condition(y), condition(x)));
    }

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static T MaxBy<T, Q>(this T[] array, Func<T, Q> selector)
    {
        if (array == null || array.Length == 0)
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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static T MinBy<T, Q>(this T[] array, Func<T, Q> selector)
    {
        if (array == null || array.Length == 0)
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

    /// <summary>
    /// 判断指定索引是否有效
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public static bool IsValidIndex<T>(this T[,] array, int row, int col)
    {
        return row >= 0 && row < array.GetLength(0) && col >= 0 && col < array.GetLength(1);
    }

    /// <summary>
    /// 判断指定索引是否有效
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="rowCol"></param>
    /// <returns></returns>
    public static bool IsValidIndex<T>(this T[,] array, Vector2Int rowCol)
    {
        return array.IsValidIndex(rowCol.x, rowCol.y);
    }

    #endregion
}
