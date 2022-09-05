using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Minecraft_Server_Management.Module
{
    public class HostConfig
    {
        public string? Host { get; set; }
        public string? User { get; set; }
        public string? Passwd { get; set; }
        public int Port { get; set; }
        public int? PortChangeCheck { get; set; }
        public int? AutoLogin { get; set; }
    }
    class Module
    {
        public static SshClient? Conn_SSH(string host, string user, string passwd, int port = 22)
        {
            SshClient? client = new(host, port, user, passwd); ;

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
                Thread.Sleep(1000);
            }

            var result = sshCMD.EndExecute(asyncCMD);

            return result;
        }
    }
}