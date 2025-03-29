using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    /// <summary>
    /// 负责 Vector3 的近似比较和哈希计算，考虑容差值。
    /// </summary>
    public struct Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private readonly float tolerance;

        public Vector3EqualityComparer(float tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(Vector3 v1, Vector3 v2)
        {
            return (v1 - v2).sqrMagnitude < tolerance * tolerance;
        }

        public int GetHashCode(Vector3 obj)
        {
            unchecked
            {
                // 使用 17 和 31 作为乘法因子来生成哈希
                int hash = 17;
                hash = hash * 31 + Mathf.RoundToInt(obj.x / tolerance).GetHashCode();
                hash = hash * 31 + Mathf.RoundToInt(obj.y / tolerance).GetHashCode();
                hash = hash * 31 + Mathf.RoundToInt(obj.z / tolerance).GetHashCode();
                return hash;
            }
        }
    }
}