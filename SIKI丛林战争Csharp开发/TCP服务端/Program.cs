using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP服务端
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServerASync();
            Console.ReadKey();
        }

        static void StartServerASync()
        {

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string address = "127.0.0.1";
            int port = 8860;
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(address), port);
            serverSocket.Bind(point);
            serverSocket.Listen(10); //socket放置与侦听状态 设置监听最大值

            //Socket clientSocket = serverSocket.Accept(); //接收一个客户端连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            string msg = "Hello Client!this is Server";
            clientSocket.Send(Encoding.UTF8.GetBytes(msg));

            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static byte[] dataBuffer = new byte[1024];
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                string msg = Encoding.UTF8.GetString(dataBuffer, 0, count);
                Console.WriteLine("从客户端接收的数据:" + msg);
                clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }
    }

}
