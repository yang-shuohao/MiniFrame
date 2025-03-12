using System;

namespace YSH.Framework
{

    /// <summary>
    /// ��Χ�ַ���
    /// ��ʾ��Source�ַ����У���StartIndex��EndIndex��Χ���ַ����ɵ��ַ���
    /// </summary>
    public struct RangeString : IEquatable<RangeString>
    {
        /// <summary>
        /// Դ�ַ���
        /// </summary>
        private string sourceStr;

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private int startIndex;

        /// <summary>
        /// ������Χ
        /// </summary>
        private int endIndex;

        /// <summary>
        /// ����
        /// </summary>
        private int length;

        /// <summary>
        /// Դ�ַ����Ƿ�ΪNull��Empty
        /// </summary>
        private bool isSourceNullOrEmpty;

        /// <summary>
        /// ��ϣ��
        /// </summary>
        private int hashCode;


        public RangeString(string source, int startIndex, int endIndex)
        {
            sourceStr = source;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            length = endIndex - startIndex + 1;
            isSourceNullOrEmpty = string.IsNullOrEmpty(source);
            hashCode = 0;
        }

        public bool Equals(RangeString other)
        {

            bool isOtherNullOrEmpty = string.IsNullOrEmpty(other.sourceStr);

            if (isSourceNullOrEmpty && isOtherNullOrEmpty)
            {
                return true;
            }

            if (isSourceNullOrEmpty || isOtherNullOrEmpty)
            {
                return false;
            }

            if (length != other.length)
            {
                return false;
            }

            for (int i = startIndex, j = other.startIndex; i <= endIndex; i++, j++)
            {
                if (sourceStr[i] != other.sourceStr[j])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (hashCode == 0 && !isSourceNullOrEmpty)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    hashCode = 31 * hashCode + sourceStr[i];
                }
            }

            return hashCode;
        }

        public override string ToString()
        {
            RedPointMgr.Instance.CachedSb.Clear();
            for (int i = startIndex; i <= endIndex; i++)
            {
                RedPointMgr.Instance.CachedSb.Append(sourceStr[i]);
            }
            string str = RedPointMgr.Instance.CachedSb.ToString();

            return str;
        }
    }
}

