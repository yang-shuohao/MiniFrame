using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServerApp
{
    internal class Program
    {
        private static string ip = "192.168.17.131";
        private static int port = 1011;

        private static Socket serverSocket;

        static void Main(string[] args)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

            serverSocket.Listen(3000);

            Console.WriteLine("启动监听【{0}】成功！", serverSocket.LocalEndPoint.ToString());

            Thread thread = new Thread(ListenClientCallBack);
            thread.Start();

            Console.ReadLine();
        }

        private static void ListenClientCallBack()
        {
            while (true)
            {
                Socket socket = serverSocket.Accept();

                Console.WriteLine("客户端【{0}】已经连接！",socket.RemoteEndPoint.ToString());

                Role role = new Role();
                ClientSocket clientSocket = new ClientSocket(socket, role);

                RoleMgr.Instance.AllRoles.Add(role);
            }
        }
    }
}
