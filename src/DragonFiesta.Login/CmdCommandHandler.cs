using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFiesta.Login
{
    public class CmdCommandHandler
    {
        public static void GetCommand(string[] Cmd)
        {
            try
            {
                switch (Cmd[0])
                {
                    case "help":
                        switch (Cmd[1])
                        {
                            case "account":
                                Console.WriteLine("use Account create <Username> <Password> for Create Account");
                                break;
                            case "Server":
                                //todo commands
                                break;
                        }
                        break;
                    case "Account":
                        if (Cmd.Length > 1)
                        {
                            switch (Cmd[1])
                            {
                                case "create":
                                    if (Cmd.Length == 5)
                                    {
                                        Login.Database.Account.CreateAccount(Cmd[2], Cmd[3]);
                                    }
                                    break;
                                case "delete":
                                    break;

                            }
                            Console.WriteLine("Check Syntax for command account Use Help Account for more Information");
                        }
                        break;
                    case "Shutdown":
                        //todo
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Use Help for Commandhelp");
            }
        }
    }
}
