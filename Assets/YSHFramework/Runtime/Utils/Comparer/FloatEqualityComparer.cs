using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    /// <summary>
    /// �ṩ�������Ľ��ƱȽϹ�����
    /// </summary>
    public static class FloatComparer
    {
        /// <summary>
        /// �ж������������Ƿ�������
        /// </summary>
        public static bool AreNearlyEqual(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// ʹ�� Unity �ṩ�� Mathf.Approximately �ж��Ƿ�������
        /// </summary>
        public static bool AreApproximatelyEqual(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        /// <summary>
        /// �ж� a �Ƿ��ϸ���� b
        /// </summary>
        public static bool IsGreaterThan(float a, float b)
        {
            return a > b;
        }

        /// <summary>
        /// �ж� a �Ƿ��ϸ�С�� b
        /// </summary>
        public static bool IsLessThan(float a, float b)
        {
            return a < b;
        }

        /// <summary>
        /// �ж� a �Ƿ���ڵ��� b�����ǽ�����ȣ�
        /// </summary>
        public static bool IsGreaterThanOrEqualTo(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return IsGreaterThan(a, b) || AreNearlyEqual(a, b, tolerance);
        }

        /// <summary>
        /// �ж� a �Ƿ�С�ڵ��� b�����ǽ�����ȣ�
        /// </summary>
        public static bool IsLessThanOrEqualTo(float a, float b, float tolerance = ComparerConstants.DefaultTolerance)
        {
            return IsLessThan(a, b) || AreNearlyEqual(a, b, tolerance);
        }
    }

    /// <summary>
    /// ���ڸ������ڼ����еĽ��ƱȽ�
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
            // ʹ���ݲ�ֵ�����������룬�����ϣ��ײ
            int roundedValue = Mathf.RoundToInt(obj / tolerance); // �������뵽�ݲΧ
            return roundedValue.GetHashCode(); // ʹ������������ֵ�����ɹ�ϣ
        }
    }
}
