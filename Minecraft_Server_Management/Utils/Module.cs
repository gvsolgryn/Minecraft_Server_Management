using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Minecraft_Server_Management.Module
{
    internal class Module
    {
        public static SshClient? Conn_SSH(string host, string user, string passwd, int port = 22)
        {
            SshClient? client;

            try
            {
                client = new(host, port, user, passwd);

                client.Connect();

                return client;
            }

            catch(Exception e)
            {
                client = null;
                MessageBox.Show($"[Error Connect] {e}");
            }

            return client;
        }

        public static void SendCMD(SshClient client, string data)
        {
            SshCommand sshCMD = client.CreateCommand(data);

            sshCMD.BeginExecute();
        }
    }
}
