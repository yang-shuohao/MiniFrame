using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    /// <summary>
    /// ���� Vector3 �Ľ��ƱȽϺ͹�ϣ���㣬�����ݲ�ֵ��
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
                // ʹ�� 17 �� 31 ��Ϊ�˷����������ɹ�ϣ
                int hash = 17;
                hash = hash * 31 + Mathf.RoundToInt(obj.x / tolerance).GetHashCode();
                hash = hash * 31 + Mathf.RoundToInt(obj.y / tolerance).GetHashCode();
                hash = hash * 31 + Mathf.RoundToInt(obj.z / tolerance).GetHashCode();
                return hash;
            }
        }
    }
}