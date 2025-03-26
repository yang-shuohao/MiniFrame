using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameServerApp.YSHFrame;

namespace GameServerApp
{
    /// <summary>
    /// 客户端连接对象，负责和客户端通讯
    /// </summary>
    public class ClientSocket
    {
        private Role role;
        //客户端Socket
        private Socket socket;

        #region 接收数据
        //接收数据的线程
        private Thread receiveThread;
        //接收数据包的字节数组
        private byte[] receiveBuffer = new byte[10240];
        //接收数据包的缓冲数据流
        private ExtendedMemoryStream receiveMS = new ExtendedMemoryStream();
        #endregion

        #region 发送数据
        private byte[] sendBuffer = new byte[10240];
        private Queue<byte[]> sendQueue = new Queue<byte[]>();
        private Action checkSendQueue;
        #endregion

        public ClientSocket(Socket socket, Role role)
        {
            this.socket = socket;
            this.role = role;
            this.role.clientSocket = this;

            receiveThread = new Thread(ReceiveMsg);
            receiveThread.Start();

            checkSendQueue = OnCheckSendQueueCallBack;
            using (ExtendedMemoryStream ms =  new ExtendedMemoryStream())
            {
                ms.WriteUTF8String(string.Format("欢迎登陆服务器" + DateTime.Now.ToString()));
                SendMsg(ms.ToArray()); 
            }
        }

        #region 接收数据
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
                    if(receiveMS.Length > 2)
                    {
                        while(true)
                        {
                            receiveMS.Position = 0;
                            //包体长度
                            int msgLen = receiveMS.ReadUShort();
                            //完整长度，总包长度=包头长度+包体长度
                            int fullMsgLen = 2 + msgLen;
                  
                            if(receiveMS.Length >= fullMsgLen)
                            {
                                //至少收到一个完整包
                                byte[] buffer = new byte[msgLen];
                                //包体位置
                                receiveMS.Position = 2;
                                receiveMS.Read(buffer, 0, msgLen);

                                using(ExtendedMemoryStream ms = new ExtendedMemoryStream(buffer))
                                {
                                    string msg = ms.ReadUTF8String();
                                    Console.WriteLine(msg);
                                }

                                using(ExtendedMemoryStream ms =new ExtendedMemoryStream())
                                {
                                    ms.WriteUTF8String(string.Format("服务器时间" + DateTime.Now.ToString()));
                                    SendMsg(ms.ToArray());
                                }

                                //处理剩余数据
                                int remainLen = (int)receiveMS.Length - fullMsgLen;
                                if(remainLen > 0)
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
                    Console.WriteLine("客户端【{0}】断开连接了！", socket.RemoteEndPoint.ToString());

                    RoleMgr.Instance.AllRoles.Remove(role);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("客户端【{0}】断开连接了！", socket.RemoteEndPoint.ToString());

                RoleMgr.Instance.AllRoles.Remove(role);
            }
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendMsg(byte[] buffer)
        {
            byte[] sendBuffer = MakeData(buffer);

            lock (sendQueue)
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
    }
}
