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
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string address = "127.0.0.1";
            int port = 8860;
            IPEndPoint point = new IPEndPoint(IPAddress.Parse(address), port);
            serverSocket.Bind(point);
            serverSocket.Listen(10); //socket放置与侦听状态 设置监听最大值

            Socket clientSocket = serverSocket.Accept(); //接收一个客户端连接
            string msg = "Hello Client!this is Server";
            clientSocket.Send(Encoding.UTF8.GetBytes(msg));


            //
            byte[] byteBuffer = new byte[1024];
            int length =  clientSocket.Receive(byteBuffer);
            string message = Encoding.UTF8.GetString(byteBuffer,0,length);
            Console.WriteLine(message);

            clientSocket.Close();
            serverSocket.Close();
        }
    }
}
