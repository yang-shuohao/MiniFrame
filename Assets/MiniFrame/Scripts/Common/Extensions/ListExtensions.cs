using System;
using System.Collections.Generic;

public static class List
{

    /// <summary>
    /// 根据索引交换元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public static void Swap<T>(this List<T> list, int index1, int index2)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (!IsValidIndex(list, index1) || !IsValidIndex(list, index2))
        {
            throw new ArgumentOutOfRangeException("Index is out of range.");
        }

        if (index1 != index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

    /// <summary>
    /// 索引是否有效
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsValidIndex<T>(this List<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }

    /// <summary>
    /// 拷贝
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="sourceList"></param>
    public static void CopyFrom<T>(this List<T> list, List<T> sourceList)
    {
        foreach (var item in sourceList)
        {
            list.Add(item);
        }
    }

    /// <summary>
    /// 获取最大值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 获取最小值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 查找一个符合条件的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 查找所有符合条件的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 升序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    public static void OrderBy<T, Q>(this List<T> list, Func<T, Q> condition) where Q : IComparable
    {
        list.Sort((x, y) => condition(x).CompareTo(condition(y)));
    }

    /// <summary>
    /// 降序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    public static void OrderByDescending<T, Q>(this List<T> list, Func<T, Q> condition) where Q : IComparable<Q>
    {
        list.Sort((x, y) => condition(y).CompareTo(condition(x)));
    }

    /// <summary>
    /// 对集合每个元素进行特定操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="action"></param>
    public static void ForEachElement<T>(this List<T> list, Action<T> action)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        if (action == null) throw new ArgumentNullException(nameof(action));

        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            action?.Invoke(list[i]);
        }
    }
}
