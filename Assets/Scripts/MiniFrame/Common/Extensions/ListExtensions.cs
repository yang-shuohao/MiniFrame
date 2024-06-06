using System;
using System.Collections.Generic;

public static class ListExtensions
{
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

    public static bool IsValidIndex<T>(this List<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
}
