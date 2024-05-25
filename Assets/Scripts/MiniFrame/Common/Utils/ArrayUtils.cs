using System;
using UnityEngine;

public static class ArrayUtils
{
    public static T[] GetRandomElements<T>(T[] sourceArray, int count)
    {
        // 检查count是否超过数组长度
        if (count > sourceArray.Length)
        {
            throw new ArgumentException("count不能超过sourceArray的长度");
        }

        // 创建一个临时数组用于存储结果
        T[] result = new T[count];

        // 创建一个副本数组用于洗牌
        T[] copyArray = new T[sourceArray.Length];
        Array.Copy(sourceArray, copyArray, sourceArray.Length);

        // Fisher-Yates 洗牌算法
        for (int i = copyArray.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = copyArray[i];
            copyArray[i] = copyArray[j];
            copyArray[j] = temp;
        }

        // 取前 count 个元素作为结果
        Array.Copy(copyArray, result, count);

        return result;
    }
}