using System;
using System.Runtime.Serialization;

namespace YSH.Framework.Exceptions
{
    /// <summary>
    /// 当尝试获取一个不存在的 UI Panel 时抛出的异常。
    /// </summary>
    [Serializable]
    public class PanelNotFoundException : Exception
    {
        /// <summary>
        /// 无参数构造函数，提供默认错误消息。
        /// </summary>
        public PanelNotFoundException()
            : base("UI Panel not found.") { }

        /// <summary>
        /// 传入错误消息的构造函数。
        /// </summary>
        /// <param name="message">错误消息。</param>
        public PanelNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// 传入错误消息和内部异常的构造函数。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="innerException">内部异常对象。</param>
        public PanelNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// 用于反序列化异常对象的构造函数。
        /// </summary>
        /// <param name="info">序列化信息。</param>
        /// <param name="context">流上下文。</param>
        protected PanelNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
