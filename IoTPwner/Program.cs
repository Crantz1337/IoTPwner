using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using System.Transactions;

namespace IoTPwner
{
    internal class Program
    {

        static string targetsPath = "";
        static string username = "";
        static string password = "";
        
        static void Main(string[] args)
        {
            UI.printBanner();

            try
            {
                targetsPath = UI.prompt("Enter path of targets : ");
                username = UI.prompt("Enter default username : ");
                password = UI.prompt("Enter default password : ");

                var pwner = new IOTPWNER(targetsPath, username, password);
                pwner.start();
                UI.coloredOutput("[!] Done\n", ConsoleColor.Yellow);
            }
            catch (Exception e)
            {
                UI.coloredOutput($"ERROR : {e.TargetSite}, {e.Message}\nPress ENTER to exit...", ConsoleColor.Red);
            }
            finally
            {
                Console.ReadLine();
            }
          
        }
    }
}
