using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Reflection;


namespace RemoteFileSync
{
    class Program_v2
    {
        static void Main(string[] args)
        {

            IPAddress address = IPAddress.Parse("127.0.0.1");
            Int32 port = 0;
            FFS FFSList = new FFS();
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Console.WriteLine("RemoteFileSync {0}",version);
            Console.WriteLine("Available syncs:");
            foreach (string name in FFSList.Names())
            {
                Console.WriteLine(name);
            }

            if (args.Length == 2)
            {
                string[] dataSplit = args[0].Split('.');
                bool test = false;
                bool test2 = false;

                if (dataSplit.Length == 4)
                {
                    test = IPAddress.TryParse(args[0], out address);
                }                
                test2 = Int32.TryParse(args[1], out port);

                if (test && test2)
                    TCPclass.TCPListener(address, port, FFSList); //run tcp application if both are true
                else
                {
                    UserInteract();
                }
                
            }
            else
            {
                UserInteract();
            }
            

        }

        static void UserInteract()
        {
            string input = null;
            FFS ffsList = new FFS();

            while (true)
            {
                input = Console.ReadLine();

                Console.WriteLine("Enter Command:");
                
                switch (input)
                {
                    case "tcp":

                
                        //poll for ip and port
                        Console.WriteLine("Enter IP Address:");
                        input = Console.ReadLine();
                        IPAddress address = ParseIP(input);

                        Console.WriteLine("Enter Port Number:");
                        input = Console.ReadLine();
                        Int32 port = ParsePort(input);

                        Console.WriteLine("Opening TCP Port at {0}:{1}", address, port);

                        //run tcp application
                        //TCPListener(address, port);

                        TCPclass.TCPListener(address, port, ffsList);

                        break;

                    case "local":

                        //run local application

                        break;

                    case "exit":

                        return;


                    case "test":

                        //run test application

                        break;

                    default:

                        Console.WriteLine("Not a valid entry. \nOptions are:\n-tcp\n-local\n-exit");

                        break;

                }

            }
        }

        private static IPAddress ParseIP(string input)
        {
            IPAddress address = IPAddress.Parse("0.0.0.0");
            bool test = false;
            string[] dataSplit = input.Split('.');

            if (dataSplit.Length == 4)
            {

               test = IPAddress.TryParse(input, out address);
            }
            //while loop using test
            while (!test)
            {
                Console.WriteLine("Invalid Entry.  Please use format xxx.xxx.xxx.xxx\nEnter IP Address:");
                input = Console.ReadLine();

                dataSplit = input.Split('.');

                if (dataSplit.Length == 4)
                {

                    test = IPAddress.TryParse(input, out address);
                }

            }
            
                return address;                               
        }

        private static Int32 ParsePort(string input)
        {
            Int32 port;

            bool test = Int32.TryParse(input, out port);

            while (!test)
            {
                Console.WriteLine("Invalid Entry. Please enter a single number.\nEnter Port:");
                input = Console.ReadLine();

                test = Int32.TryParse(input, out port);
            }


            return port;
        }
    }
}
