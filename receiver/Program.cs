using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace receiver
{
    class Program
    {
        private const Int32 MULTICAST_PORT = 4010;
        private const String MULTICAST_GROUP = "239.255.20.200";
        private const String LOCAL_IP = "192.168.21.101";

        static void Main(string[] args)
        {
            
            Console.WriteLine(LOCAL_IP);

            var multicastEP = new IPEndPoint(IPAddress.Parse(MULTICAST_GROUP), MULTICAST_PORT);
            var localEP = new IPEndPoint(IPAddress.Parse(LOCAL_IP), MULTICAST_PORT);

            var sck = InitMulticastSocket(localEP);

            Listen(sck, localEP);

            Send(sck, multicastEP, "Hello World Too!");

            Console.ReadKey();
        }


        private static Socket InitMulticastSocket(IPEndPoint _lcl)
        {
            var ret = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            ret.Bind(_lcl);
            ret.SetSocketOption(SocketOptionLevel.IP,
                SocketOptionName.AddMembership,
                new MulticastOption(IPAddress.Parse(MULTICAST_GROUP), IPAddress.Parse(LOCAL_IP)));
            ret.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);

            return ret;
        }


        private static void Send(Socket _sck, IPEndPoint _rmt, String _msg)
        {
            var msg = Encoding.ASCII.GetBytes(_msg);
            _sck.SendTo(msg, 0, msg.Length, SocketFlags.None, _rmt);
        }


        private static void Listen(Socket _sck, EndPoint _lcl)
        {
            var msg = new Byte[8000];
            _sck.ReceiveFrom(msg, ref _lcl);

            Console.WriteLine(Encoding.ASCII.GetString(msg, 0, 20));
            Console.WriteLine();
        }
    }
}
