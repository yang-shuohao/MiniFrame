
using System.Collections.Generic;

/// <summary>
/// �ֵ���չ����
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// ����ֵ�Ƴ�ָ��Ԫ��
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="value"></param>
    public static void RemoveByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
    {
        TKey keyToRemove = default(TKey);
        bool keyFound = false;

        foreach (var item in dictionary)
        {
            if (EqualityComparer<TValue>.Default.Equals(item.Value, value))
            {
                keyToRemove = item.Key;
                keyFound = true;
                break;
            }
        }

        if (keyFound)
        {
            dictionary.Remove(keyToRemove);
        }
    }
}
