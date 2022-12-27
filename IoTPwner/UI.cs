using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTPwner
{
    static public class UI
    {
        static public void coloredOutput(string text, ConsoleColor c) 
        {
            Console.ForegroundColor = c;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static public void printBanner() 
        {
            string banner =
           @"██╗ ██████╗ ████████╗██████╗ ██╗    ██╗███╗   ██╗███████╗██████╗
██║██╔═══██╗╚══██╔══╝██╔══██╗██║    ██║████╗  ██║██╔════╝██╔══██╗
██║██║   ██║   ██║   ██████╔╝██║ █╗ ██║██╔██╗ ██║█████╗  ██████╔╝
██║██║   ██║   ██║   ██╔═══╝ ██║███╗██║██║╚██╗██║██╔══╝  ██╔══██╗
██║╚██████╔╝   ██║   ██║     ╚███╔███╔╝██║ ╚████║███████╗██║  ██║
╚═╝ ╚═════╝    ╚═╝   ╚═╝      ╚══╝╚══╝ ╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝
Performs default password attacks against IoT devices that are using 'WWW-Authenticate: Basic realm='
-----------------------------------------------------------------------------------------------------";            
            coloredOutput($"{banner}\n", ConsoleColor.Cyan);   
        }

        static public string prompt(string msg)
        {
            coloredOutput($"{msg}", ConsoleColor.White);
            
            string s = Console.ReadLine();
            if(s == null || s.Length == 0) 
               throw new ArgumentException();

            return s;
        }


    }
}
