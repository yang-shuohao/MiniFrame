
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
        #region ��������
        private byte[] receiveBuffer = new byte[10240];
        private ExtendedMemoryStream receiveMS = new ExtendedMemoryStream();
        private Queue<byte[]> receiveQueue = new Queue<byte[]>();
        private int receiveCount;
        #endregion

        #region ��������
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
        /// ���ӵ�Socket������
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

                Debug.Log("���ӷ������ɹ���");
            }
            catch (Exception e)
            {
                Debug.Log("���ӷ�����ʧ��: " + e.Message);
            }
        }

        /// <summary>
        /// �Ͽ�����
        /// </summary>
        public void DisConnect()
        {
            if (socket != null && socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        #region ��������
        /// <summary>
        /// ��������
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
        /// ��װ���ݰ�
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
        /// �������ݶ���
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
        /// ��������
        /// </summary>
        private void Send(byte[] buffer)
        {
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, socket);
        }

        /// <summary>
        /// �������ݻص�
        /// </summary>
        private void SendCallBack(IAsyncResult ar)
        {
            socket.EndSend(ar);

            OnCheckSendQueueCallBack();
        }
        #endregion

        #region ��������

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
                                ms.WriteUTF8String(string.Format("�ͻ���ʱ��" + DateTime.Now.ToString()));
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
        /// ��������
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

                //���յ�����
                if (len > 0)
                {
                    //����д��β��
                    receiveMS.Position = receiveMS.Length;

                    receiveMS.Write(receiveBuffer, 0, len);

                    //�����и��������İ������ˣ���ͷʹ��ushort2�ֽ�
                    if (receiveMS.Length > 2)
                    {
                        while (true)
                        {
                            receiveMS.Position = 0;
                            //���峤��
                            int msgLen = receiveMS.ReadUShort();
                            //�������ȣ��ܰ�����=��ͷ����+���峤��
                            int fullMsgLen = 2 + msgLen;

                            if (receiveMS.Length >= fullMsgLen)
                            {
                                //�����յ�һ��������
                                byte[] buffer = new byte[msgLen];
                                //����λ��
                                receiveMS.Position = 2;
                                receiveMS.Read(buffer, 0, msgLen);

                                //���ݰ��������
                                lock(receiveQueue)
                                {
                                    receiveQueue.Enqueue(buffer);
                                }

                                //����ʣ������
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
                                    //û��ʣ������
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);
                                    break;
                                }
                            }
                            else
                            {
                                //��û���յ�������
                                break;
                            }
                        }
                    }

                    //������������
                    ReceiveMsg();
                }
                else
                {
                    Debug.Log(string.Format("����ˡ�{0}���Ͽ������ˣ�", socket.RemoteEndPoint.ToString()));
                }
            }
            catch
            {
                Debug.Log(string.Format("����ˡ�{0}���Ͽ������ˣ�", socket.RemoteEndPoint.ToString()));
            }
        }

        #endregion
    }
}

