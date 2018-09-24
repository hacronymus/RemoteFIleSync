using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteFileSync
{
    class InputHandler
    {

        private readonly string[] _commands = new string[] {"RUN","TCP","UDP","LOCAL","EXIT","INFO"};
        private Int32 _modifier;
        private string _command;

        public string Input(string command, FFS ffsList)
        {
            if (command.Contains(","))
            {
                command = SplitInput(command);
            }

            _command = command.ToUpper();

            switch (_command)
            {
                case "RUN":
                    return ffsList.RunFFS(_modifier);
                   
                case "TCP":
                    //TCPclass.TCPListener(address, port, FFSList);

                    return "Command Not Yet Supported";
                case "UDP":
                    return "Command Not Yet Supported";

                case "LOCAL":

                    return "Command Not Yet Supported";
                case "EXIT":

                    return "Command Not Yet Supported";
                case "INFO":
                    string commandList = null;
                    foreach (string c in _commands)
                    {
                        if (commandList == null)
                        {
                            commandList = c;
                        }
                        else
                        {
                            commandList = commandList + "," + c;
                        }
                    }

                    return commandList;

            }
            

            return null;
        }

        private string SplitInput(string input)
        {
            try
            {
                var sInput = input.Split(',');
                _modifier = Convert.ToInt32(sInput[1]);


                return sInput[0];
            }
            catch
            {
                return null;
            }
        }

    }
}
