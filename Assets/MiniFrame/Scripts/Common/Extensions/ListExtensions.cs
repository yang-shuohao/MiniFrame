using System;
using System.Collections.Generic;

/// <summary>
/// List��չ
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// ����ָ������������Ԫ��
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
    /// �ж�ָ�������Ƿ���Ч
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
    /// ����һ������������Ԫ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static T FindBy<T>(this List<T> list, Func<T,bool> condition)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(condition(list[i]))
            {
                return list[i];
            }
        }

        return default(T);
    }

    /// <summary>
    /// �������з���������Ԫ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static List<T> FindAllBy<T>(this List<T> list, Func<T, bool> condition)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (condition(list[i]))
            {
                result.Add(list[i]);
            }
        }

        return result;
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    public static void OrderBy<T,Q>(this List<T> list, Func<T, Q> condition)
    {
        list.Sort((x, y) => Comparer<Q>.Default.Compare(condition(x), condition(y)));
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="list"></param>
    /// <param name="condition"></param>
    public static void OrderByDescending<T, Q>(this List<T> list, Func<T, Q> condition)
    {
        list.Sort((x, y) => Comparer<Q>.Default.Compare(condition(y), condition(x)));
    }

    /// <summary>
    /// ��ȡ���ֵ
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

        for (int i = 1; i < list.Count; i++)
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
    /// ��ȡ��Сֵ
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
            throw new ArgumentException("List is empty or null.");
        }

        T minElement = list[0];
        Q minValue = selector(minElement);

        for (int i = 1; i < list.Count; i++)
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
}
