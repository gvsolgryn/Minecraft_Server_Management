using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Renci.SshNet;

namespace Minecraft_Server_Management.Module
{
    public class HostConfig
    {
        [JsonProperty]
        public string? Host { get; set; }
        [JsonProperty]
        public string? User { get; set; }
        [JsonProperty]
        public string? Passwd { get; set; }
        [JsonProperty]
        public int Port { get; set; }
        [JsonProperty]
        public int? PortChangeCheck { get; set; }
    }   
    class Module
    {
        public SshClient? client;
        public SshClient? Conn_SSH(string host, string user, string passwd, int port = 22)
        {
            client = new(host, port, user, passwd);

            try
            {
                client.Connect();

                return client;
            }

            catch(Exception e)
            {
                MessageBox.Show($"[Error Connect] {e}");
            }
            
            return client;
        }

        public static string SendCMD(SshClient client, string data)
        {
            SshCommand sshCMD = client.CreateCommand(data);

            var asyncCMD = sshCMD.BeginExecute();

            while(!asyncCMD.IsCompleted)
            {
                Thread.Sleep(100);
            }

            var result = sshCMD.EndExecute(asyncCMD);

            return result;
        }
    }
}