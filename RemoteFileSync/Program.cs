using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Text;

namespace FileSyncRemote
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(value: "Remote File Sync V01");
            string userIn = null;
            string localAddr = null;
            string port = null;
            int portVal;
            IPAddress address;

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("\nEnter Command:");
                    userIn = Console.ReadLine();
                  //  if (userIn != "tcp")
                  //  {
                  //      goto Manual;
                  //  }
                    address = IPEntryParse(localAddr);
                    portVal = PortEntryParse(port);
                    break;

                case 1:
                    if (Int32.TryParse(args[0], out portVal)) {

                        address = IPEntryParse(localAddr);
                        Console.WriteLine("\nEnter Command:");
                        break;
                    }
                    else if (IPAddress.TryParse(args[0], out address)) {

                        //Console.WriteLine("Enter IP Address:");
                        //localAddr = Console.ReadLine();
                        portVal = PortEntryParse(port);
                        Console.WriteLine("\nEnter Command:");
                        break;
                    }
                    else
                    {
                        userIn = args[0];                        
                        address = IPEntryParse(localAddr);
                        portVal = PortEntryParse(port);
                        // Console.WriteLine("\nEnter Command:");
                        break;
                    }
                case 2:
                    bool[] result1 = new bool[3];
                    bool[] result2 = new bool[3];
                    bool[] result3 = new bool[3];

                    result1[0] = Int32.TryParse(args[0], out portVal);
                    result2[0] = IPAddress.TryParse(args[0], out address);
                    result1[1] = Int32.TryParse(args[1], out portVal);
                    result2[1] = IPAddress.TryParse(args[1], out address);

                    if (!(result2[0] || result2[1]))
                    {
                        address = IPEntryParse(localAddr);
                    }

                    if (!(result1[0] || result1[1]))
                    {
                        portVal = PortEntryParse(port);
                    }

                    if (userIn == null)
                    {
                        Console.WriteLine("\nEnter Command:");
                        
                    }

                    break;
                case 3:

                    bool[] result31 = new bool[3];
                    bool[] result32 = new bool[3];
                    //bool[] result33 = new bool[3];

                    result31[0] = Int32.TryParse(args[0], out portVal);
                    result32[0] = IPAddress.TryParse(args[0], out address);
                    result31[1] = Int32.TryParse(args[1], out portVal);
                    //result32[1] = IPAddress.TryParse(args[1], out address);
                    //result31[2] = Int32.TryParse(args[2], out portVal);
                    //result32[2] = IPAddress.TryParse(args[2], out address);
                    userIn = args[2];

                    if (!(result32[0] || result32[1]) || result32[2])
                    {
                        address = IPEntryParse(localAddr);
                    }

                    if (!(result31[0] || result31[1]) || result31[2])
                    {
                        portVal = PortEntryParse(port);
                    }
                    if (userIn == null)
                    {
                        Console.WriteLine("\nEnter Command:");

                    }

                    break;
                default:
                    Console.WriteLine("Invalid Argument Count");
                    address = IPEntryParse(localAddr);
                    portVal = PortEntryParse(port);
                    Console.WriteLine("\nEnter Command:");
                    break;



            }

            

            //Manual:

            while (userIn != "exit")
            {   //todo: fix case sensitivty

                // userIn = Console.ReadLine();  Moved to End
                string[] dataSplit = userIn.Split(',');
                string command = dataSplit[0];
               
                switch (command)
                {
                    case "run":
                        int choice = 999;
                        string choiceS = null;

                        if (dataSplit.Length == 2)
                        {
                            choiceS = dataSplit[1];
                        }

                        while (true)
                        {
                            if (Int32.TryParse(choiceS, out choice)) { break; }


                            Console.WriteLine("Enter Choice:");
                            choiceS = Console.ReadLine();

                        }

                        Console.WriteLine("Running FFS...");
                        string status = LaunchFFS(choice);
                        Console.WriteLine(status);
                        break;

                    case "tcp":

                        MyTcpListener.MainListener(address, portVal);
                        break;

                    case "test":
                        Console.WriteLine("Entering Test Mode...");
                        TestMode();
                        break;

                    default:

                        Console.WriteLine("Not a valid choice.");
                        break;
                }
                
                
                //error logging
                userIn = Console.ReadLine();
            }

        }

        private static string LaunchFFS(int choice)
        {
            var program = "C:\\Program Files\\FreeFileSync\\FreeFileSync.exe";
            var arguments = Directory.GetFiles(".\\", "*batch"); //replace with array of existing batch files in folder

            //use choice to select proper entry in array for feeding arguments
            if (choice > (arguments.Length - 1))
            {
                return "Error: Not a valid file choice";
            }
            else
            {
                Process proc = Process.Start(program, arguments[choice]);

                proc.WaitForExit();
                //possibly use OnExited() instead to call an exit event
                //use proc.exitCode to process if there were errors (might not be correct)
            }
            return "\nSync Complete";



        }

        private static IPAddress IPEntryParse(string localAddr)
        {
            IPAddress address;
            while (!(IPAddress.TryParse(localAddr, out address)))
            {
                Console.WriteLine("\nEnter Local IP Address:");
                localAddr = Console.ReadLine();
            }
            return address;
        }

        private static int PortEntryParse(string port)
        {
            int portVal;
            while (!Int32.TryParse(port, out portVal))
            {
                Console.WriteLine("\nEnter Port Number:");
                port = Console.ReadLine();
            }
            return portVal;
        }

        private static void CommandParse(string command, int option)
        {

            return;
        }

        private static void TestMode()
        {
            string userIn = null;
            Console.Clear();
            Console.WriteLine("Test Mode");

            while (userIn != "exit") {
                int count = 0;

                userIn = Console.ReadLine();

                var files = Directory.GetFiles(".\\", "*.csv");

                foreach(string file in files)
                {
                    Console.WriteLine("[{0}]   {1}", count , file);
                    count++;
                }

            }

            return;
        }

        private class MyTcpListener
        {
            public static void MainListener(IPAddress localAddr, int port)
            {
                TcpListener server = null;
                try
                {
                   
                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    String retMessage = null;
                    int choice = 999;

                    // Enter the listening loop.
                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");

                        data = null;
                        retMessage = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine("Received: {0}", data);


                            // Process the data sent by the client.
                            data = data.ToUpper();


                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);

                           
                            string[] dataSplit = data.Split(',');
                            string command = dataSplit[0];
                            if (dataSplit.Length > 1)
                            {
                                choice = Int32.Parse(dataSplit[1]);
                                Console.WriteLine("Received Command {0} with modifier {1}", command, choice);
                            }
                            else { retMessage = "No Value Sent"; }




                            if (command == "RUN" & choice != 999)
                            {
                                Console.WriteLine("Launching FFS with choice {0}", choice);

                                string launchStatus = Program.LaunchFFS(choice);

                                Console.WriteLine(launchStatus);
                                retMessage = launchStatus;

                            }

                            msg = System.Text.Encoding.ASCII.GetBytes("\n"+retMessage);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", data);

                            retMessage = null;
                            msg = null;
                        }
                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }


                Console.WriteLine("\nHit enter to continue...");
                Console.Read();
            }
        }


    }

}
