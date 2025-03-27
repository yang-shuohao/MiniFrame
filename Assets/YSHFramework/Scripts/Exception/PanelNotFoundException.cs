using System;
using System.Runtime.Serialization;

namespace YSH.Framework.Exceptions
{
    /// <summary>
    /// �����Ի�ȡһ�������ڵ� UI Panel ʱ�׳����쳣��
    /// </summary>
    [Serializable]
    public class PanelNotFoundException : Exception
    {
        /// <summary>
        /// �޲������캯�����ṩĬ�ϴ�����Ϣ��
        /// </summary>
        public PanelNotFoundException()
            : base("UI Panel not found.") { }

        /// <summary>
        /// ���������Ϣ�Ĺ��캯����
        /// </summary>
        /// <param name="message">������Ϣ��</param>
        public PanelNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// ���������Ϣ���ڲ��쳣�Ĺ��캯����
        /// </summary>
        /// <param name="message">������Ϣ��</param>
        /// <param name="innerException">�ڲ��쳣����</param>
        public PanelNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// ���ڷ����л��쳣����Ĺ��캯����
        /// </summary>
        /// <param name="info">���л���Ϣ��</param>
        /// <param name="context">�������ġ�</param>
        protected PanelNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
