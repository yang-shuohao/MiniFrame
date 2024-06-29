using System;
using UnityEngine;

public static class ArrayUtils
{
    public static T[] GetRandomElements<T>(T[] sourceArray, int count)
    {
        // ���count�Ƿ񳬹����鳤��
        if (count > sourceArray.Length)
        {
            throw new ArgumentException("count���ܳ���sourceArray�ĳ���");
        }

        // ����һ����ʱ�������ڴ洢���
        T[] result = new T[count];

        // ����һ��������������ϴ��
        T[] copyArray = new T[sourceArray.Length];
        Array.Copy(sourceArray, copyArray, sourceArray.Length);

        // Fisher-Yates ϴ���㷨
        for (int i = copyArray.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = copyArray[i];
            copyArray[i] = copyArray[j];
            copyArray[j] = temp;
        }

        // ȡǰ count ��Ԫ����Ϊ���
        Array.Copy(copyArray, result, count);

        return result;
    }
}