using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

public class FFS

{
    private string[] arguments;
    private string[] names;
    private readonly string program = "C:\\Program Files\\FreeFileSync\\FreeFileSync.exe";

    public FFS()
	{
        arguments = Directory.GetFiles(".\\", "*batch"); 
	    names = Directory.GetFiles(".\\", "*batch");

	    for (int i = 0; i < arguments.Length; i++)
	    {
	        names[i] = names[i].Split('.')[1];
	        names[i] = i + " " +  names[i].TrimStart('\\');

	    }

	}

    public string[] Arguments {
        get { return arguments; }
    }

    public string[] Names
    {
        get { return names; }
    }

    //public string[] Names() {

    //string[] names = arguments;

    //    for (int i = 0; i < arguments.Length; i++)
    //    {
    //        names[i] = arguments[i].Split('.')[1].TrimStart('\\');
    //    }

    //    return names;
    //}

    public string RunFFS(int choice)
    {
        if (choice > (arguments.Length - 1))
        {
            return "Error: Not a valid file choice";
        }
        else
        {
            Process proc = Process.Start(program, arguments[choice]);

            

            for (int i = 0; i < 6; i++){

                System.Threading.Thread.Sleep(1000);

                var title = proc.MainWindowTitle;

                Console.WriteLine("Title:" + title);
                Console.WriteLine(proc.MachineName);

            }

            proc.WaitForExit();
            //possibly use OnExited() instead to call an exit event
            //use proc.exitCode to process if there were errors (might not be correct)
        }
        return "\nSync Complete";


    }

    public 
}
