using Common.Message;
using Google.Protobuf;
using System;
using System.IO;

namespace YSH.Framework
{
    /// <summary>
    /// PackageHandler 简化版本，默认 object 发送者
    /// </summary>
    public class PackageHandler : PackageHandler<object>
    {
        public PackageHandler(object sender) : base(sender)
        {
        }
    }

    /// <summary>
    /// PackageHandler 泛型版，使用 Google.Protobuf
    /// </summary>
    /// <typeparam name="T">消息发送者类型</typeparam>
    public class PackageHandler<T>
    {
        private MemoryStream stream = new MemoryStream(64 * 1024);
        private int readOffset = 0;
        private T sender;

        public PackageHandler(T sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// 接收数据写入缓冲区
        /// </summary>
        public void ReceiveData(byte[] data, int offset, int count)
        {
            if (stream.Position + count > stream.Capacity)
            {
                throw new Exception("PackageHandler write buffer overflow");
            }
            stream.Position = stream.Length; // 追加写入
            stream.Write(data, offset, count);

            ParsePackage();
        }

        /// <summary>
        /// 使用 Google.Protobuf 序列化并加包长前缀
        /// </summary>
        public static byte[] PackMessage(NetMessage message)
        {
            byte[] body = message.ToByteArray();

            byte[] package = new byte[4 + body.Length];
            Buffer.BlockCopy(BitConverter.GetBytes(body.Length), 0, package, 0, 4);
            Buffer.BlockCopy(body, 0, package, 4, body.Length);
            return package;
        }

        /// <summary>
        /// 使用 Google.Protobuf 反序列化
        /// </summary>
        public static NetMessage UnpackMessage(byte[] packet, int offset, int length)
        {
            return NetMessage.Parser.ParseFrom(packet, offset, length);
        }

        /// <summary>
        /// 解析数据包，递归处理粘包、半包
        /// </summary>
        private bool ParsePackage()
        {
            // 缓冲区有效数据长度
            long available = stream.Position - readOffset;

            if (available >= 4)
            {
                byte[] buffer = stream.GetBuffer();

                int packageSize = BitConverter.ToInt32(buffer, readOffset);
                if (packageSize < 0 || packageSize > 10 * 1024 * 1024)
                {
                    throw new Exception("Invalid package size: " + packageSize);
                }

                if (available >= packageSize + 4)
                {
                    NetMessage message = UnpackMessage(buffer, readOffset + 4, packageSize);
                    if (message == null)
                    {
                        throw new Exception("PackageHandler ParsePackage failed, invalid package");
                    }

                    // 交给消息分发器处理
                    MessageDistributer<T>.Instance.ReceiveMessage(sender, message);

                    readOffset += packageSize + 4;
                    return ParsePackage(); // 递归处理剩余数据
                }
            }

            // 没有完整包，移动剩余数据到开头
            if (readOffset > 0)
            {
                long size = stream.Position - readOffset;
                if (size > 0)
                {
                    Array.Copy(stream.GetBuffer(), readOffset, stream.GetBuffer(), 0, size);
                }
                readOffset = 0;
                stream.Position = size;
                stream.SetLength(size);
            }
            return true;
        }
    }
}
