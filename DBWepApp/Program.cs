using System;
using System.Net;
using System.Net.Sockets;

namespace DBWepApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = host.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 14300);

            //creo socket sull'indirzzo
            Socket listener = new Socket(ipAddr.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

        }
    }
}
