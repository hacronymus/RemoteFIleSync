using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace RemoteFileSync
{
    class TCPclass
    {
        //store address and port here
        //create method for setting method and port
        //store session in here
        //learn more about classes


        public static void TCPListener(IPAddress address, Int32 port, FFS FFSList)
        {
            TcpListener server = null;
            try
            {

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(address, port);

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

                    int j = 0;

                    foreach (var name in FFSList.Names)
                    {
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("\n" + name);
                        stream.Write(msg, 0, msg.Length);

                        j++;
                    }
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);


                        // Process the data sent by the client.
                        data = data.ToUpper();


                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("\n" + data);

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

                            string launchStatus = FFSList.RunFFS(choice);

                            Console.WriteLine(launchStatus);
                            retMessage = launchStatus;

                        }

                        msg = System.Text.Encoding.ASCII.GetBytes("\n" + retMessage);

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


            return;
        }
    }
}
