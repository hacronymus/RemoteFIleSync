using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFileSync
{
    class UDPclass
    {
        private IPAddress _retAddress;
        private Int32 _listenPort;
        private Int32 _retPort;

        public UDPclass(string retAddress, Int32 listenPort, Int32 retPort)
        {
            this.RetAddress = retAddress;  //Should come is as a string and go through parseIP routine  
            this._listenPort = listenPort;
            this._retPort = retPort;
        }

        public string RetAddress
        {
            get { return _retAddress.ToString();}
            set
            {
                if (value.Split('.').Length == 4)
                {
                    IPAddress.TryParse(value, out _retAddress);
                }
                else
                {
                    Console.WriteLine("Improper IP Address format.");
                }
            }
        }

        public void StartListener()
        {
            bool done = false;

            UdpClient listener = new UdpClient(_listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _listenPort);

            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                        groupEP.ToString(),
                        Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }

        public void RetMessage(string message)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);

           // IPAddress broadcast = IPAddress.Parse("192.168.1.255");

            byte[] sendbuf = Encoding.ASCII.GetBytes(message);
            IPEndPoint ep = new IPEndPoint(_retAddress, _retPort);

            s.SendTo(sendbuf, ep);

            Console.WriteLine("Message sent to the broadcast address");
        }
    }
}
