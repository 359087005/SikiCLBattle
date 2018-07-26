using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            string host = "127.0.0.1";
            int port = 8860;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host),port);
            clientSocket.Connect(endPoint);

            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data,0,count);
            Console.WriteLine(msg);

            while (true)
            {
                string s = Console.ReadLine();
                clientSocket.Send(Encoding.UTF8.GetBytes(s));
            }
            Console.ReadKey();
            clientSocket.Close();
        }
    }
}
