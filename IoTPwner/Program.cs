using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;




namespace IoTPwner
{
    internal class Program
    {
        static string targetsPath = "C:\\Users\\johan\\Desktop\\targets.txt";
        static string username = "admin";
        static string password = "admin";
        static void Main(string[] args)
        {
            var pwner = new IOTPWNER(targetsPath, username, password);
            pwner.start();
            
            Console.ReadLine();
        }
    }
}
