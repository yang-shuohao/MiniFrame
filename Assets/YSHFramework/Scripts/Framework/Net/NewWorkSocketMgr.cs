
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace YSH.Framework
{
    public class NewWorkSocketMgr : Singleton<NewWorkSocketMgr>
    {
        #region 接收数据
        private byte[] receiveBuffer = new byte[10240];
        private ExtendedMemoryStream receiveMS = new ExtendedMemoryStream();
        private Queue<byte[]> receiveQueue = new Queue<byte[]>();
        private int receiveCount;
        #endregion

        #region 发送数据
        private byte[] sendBuffer = new byte[10240];
        private Queue<byte[]> sendQueue = new Queue<byte[]>();
        private Action checkSendQueue;
        #endregion

        private Socket socket;

        public NewWorkSocketMgr()
        {
            MonoMgr.Instance.AddUpdateListener(OnUpdate);
        }

        /// <summary>
        /// 连接到Socket服务器
        /// </summary>
        public void Connect(string ip, int port)
        {
            if (socket != null && socket.Connected) return;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

                checkSendQueue = OnCheckSendQueueCallBack;

                ReceiveMsg();

                Debug.Log("连接服务器成功！");
            }
            catch (Exception e)
            {
                Debug.Log("连接服务器失败: " + e.Message);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (socket != null && socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendMsg(byte[] buffer)
        {
            byte[] sendBuffer = MakeData(buffer);

            lock(sendQueue)
            {
                sendQueue.Enqueue(sendBuffer);

                checkSendQueue.BeginInvoke(null, null);
            }
        }

        /// <summary>
        /// 封装数据包
        /// </summary>
        private byte[] MakeData(byte[] data)
        {
            byte[] retBuffer = null;
            using (ExtendedMemoryStream ms = new ExtendedMemoryStream())
            {
                ms.WriteUShort((ushort)data.Length);
                ms.Write(data, 0, data.Length);
                retBuffer = ms.ToArray();
            }

            return retBuffer;
        }

        /// <summary>
        /// 发送数据队列
        /// </summary>
        private void OnCheckSendQueueCallBack()
        {
            lock (sendQueue)
            {
                if (sendQueue.Count > 0)
                {
                    Send(sendQueue.Dequeue());
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private void Send(byte[] buffer)
        {
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, socket);
        }

        /// <summary>
        /// 发送数据回调
        /// </summary>
        private void SendCallBack(IAsyncResult ar)
        {
            socket.EndSend(ar);

            OnCheckSendQueueCallBack();
        }
        #endregion

        #region 接收数据

        private void OnUpdate()
        {
            while(true)
            {
                if(receiveCount <= 5)
                {
                    receiveCount++;
                    lock(receiveQueue)
                    {
                        if(receiveQueue.Count > 0)
                        {
                            byte[] buffer = receiveQueue.Dequeue();

                            using(ExtendedMemoryStream ms = new ExtendedMemoryStream(buffer))
                            {
                                string msg = ms.ReadUTF8String();
                                Debug.Log(msg);
                            }

                            using (ExtendedMemoryStream ms = new ExtendedMemoryStream())
                            {
                                ms.WriteUTF8String(string.Format("客户端时间" + DateTime.Now.ToString()));
                                SendMsg(ms.ToArray());
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    receiveCount = 0;
                    break;
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveMsg()
        {
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, socket);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int len = socket.EndReceive(ar);

                //接收到数据
                if (len > 0)
                {
                    //数据写入尾部
                    receiveMS.Position = receiveMS.Length;

                    receiveMS.Write(receiveBuffer, 0, len);

                    //至少有个不完整的包过来了，包头使用ushort2字节
                    if (receiveMS.Length > 2)
                    {
                        while (true)
                        {
                            receiveMS.Position = 0;
                            //包体长度
                            int msgLen = receiveMS.ReadUShort();
                            //完整长度，总包长度=包头长度+包体长度
                            int fullMsgLen = 2 + msgLen;

                            if (receiveMS.Length >= fullMsgLen)
                            {
                                //至少收到一个完整包
                                byte[] buffer = new byte[msgLen];
                                //包体位置
                                receiveMS.Position = 2;
                                receiveMS.Read(buffer, 0, msgLen);

                                //数据包放入队列
                                lock(receiveQueue)
                                {
                                    receiveQueue.Enqueue(buffer);
                                }

                                //处理剩余数据
                                int remainLen = (int)receiveMS.Length - fullMsgLen;
                                if (remainLen > 0)
                                {
                                    receiveMS.Position = fullMsgLen;
                                    byte[] remainBuffer = new byte[remainLen];
                                    receiveMS.Read(remainBuffer, 0, remainLen);
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);

                                    receiveMS.Write(remainBuffer, 0, remainLen);

                                    remainBuffer = null;
                                }
                                else
                                {
                                    //没有剩余数据
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);
                                    break;
                                }
                            }
                            else
                            {
                                //还没有收到完整包
                                break;
                            }
                        }
                    }

                    //继续接收数据
                    ReceiveMsg();
                }
                else
                {
                    Debug.Log(string.Format("服务端【{0}】断开连接了！", socket.RemoteEndPoint.ToString()));
                }
            }
            catch
            {
                Debug.Log(string.Format("服务端【{0}】断开连接了！", socket.RemoteEndPoint.ToString()));
            }
        }

        #endregion
    }
}

