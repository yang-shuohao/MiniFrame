

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace YSH.Framework.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 根据值移除字典中的第一个匹配项
        /// </summary>
        public static void RemoveFirstByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
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

        public static TKey GetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (var pair in dictionary)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                {
                    return pair.Key;
                }
            }

            return default;
        }

        public static bool TryGetKeyByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value, out TKey key)
        {
            foreach (var pair in dictionary)
            {
                if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                {
                    key = pair.Key;
                    return true;
                }
            }
            key = default;
            return false;
        }

        /// <summary>
        /// 修改键
        /// </summary>
        public static void ChangeKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
        {
            // 检查旧键是否存在
            if (!dictionary.ContainsKey(oldKey))
            {
                return;
            }

            // 获取旧键的值
            TValue value = dictionary[oldKey];

            // 删除旧键值对
            dictionary.Remove(oldKey);

            dictionary[newKey] = value;
        }

        /// <summary>
        /// 移除字典中null键
        /// </summary>
        public static void RemoveNullKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : Object
        {
            List<TKey> keysToRemove = ListPool<TKey>.Get();

            foreach (var key in dictionary.Keys)
            {
                if (key == null)
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }

            ListPool<TKey>.Release(keysToRemove);
        }
    }
}

