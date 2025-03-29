using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    /// <summary>
    /// 提供浮点数的近似比较工具类
    /// </summary>
    public static class FloatComparer
    {
        /// <summary>
        /// 判断两个浮点数是否近似相等
        /// </summary>
        public static bool AreNearlyEqual(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// 使用 Unity 提供的 Mathf.Approximately 判断是否近似相等
        /// </summary>
        public static bool AreApproximatelyEqual(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        /// <summary>
        /// 判断 a 是否严格大于 b
        /// </summary>
        public static bool IsGreaterThan(float a, float b)
        {
            return a > b;
        }

        /// <summary>
        /// 判断 a 是否严格小于 b
        /// </summary>
        public static bool IsLessThan(float a, float b)
        {
            return a < b;
        }

        /// <summary>
        /// 判断 a 是否大于等于 b（考虑近似相等）
        /// </summary>
        public static bool IsGreaterThanOrEqualTo(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return IsGreaterThan(a, b) || AreNearlyEqual(a, b, tolerance);
        }

        /// <summary>
        /// 判断 a 是否小于等于 b（考虑近似相等）
        /// </summary>
        public static bool IsLessThanOrEqualTo(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return IsLessThan(a, b) || AreNearlyEqual(a, b, tolerance);
        }
    }

    /// <summary>
    /// 用于浮点数在集合中的近似比较
    /// </summary>
    public struct FloatEqualityComparer : IEqualityComparer<float>
    {
        private readonly float tolerance;

        public FloatEqualityComparer(float tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(float x, float y)
        {
            return FloatComparer.AreNearlyEqual(x, y, tolerance);
        }

        public int GetHashCode(float obj)
        {
            // 使用容差值进行四舍五入，避免哈希碰撞
            int roundedValue = Mathf.RoundToInt(obj / tolerance); // 四舍五入到容差范围
            return roundedValue.GetHashCode(); // 使用四舍五入后的值来生成哈希
        }
    }
}
