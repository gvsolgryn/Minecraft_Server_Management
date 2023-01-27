using Minecraft_Server_Management.Module;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Minecraft_Server_Management
{
	public partial class HostSetting : Form
	{
		readonly Module.Module module = new();
		SshClient? client;

		public HostSetting()
		{
			InitializeComponent();
		}

		static readonly DirectoryInfo configPath = new(Application.StartupPath + @"config\");
		static readonly FileInfo configFileInfo = new(configPath + @"hostConfig.json");

		private static void Config_Dir_Check()
		{
			if (!configPath.Exists)
			{
				Debug.WriteLine("Config path is not found\nCreate Directory", "HostSetting");
				configPath.Create();
			}
		}

		private static void Config_File_Check()
		{
			var hostConfig = new HostConfig
			{
				Host = "exam.host.name || 127.0.0.1",
				User = "username",
				Passwd = "password",
				Port = 22,
				PortChangeCheck = 0
			};

			if (!configFileInfo.Exists)
			{
				MessageBox.Show("Config file is not found\nCreate file", "HostSetting");

				File.WriteAllText(configFileInfo.ToString(), JsonConvert.SerializeObject(hostConfig, Formatting.Indented));
			}
		}

		private static void HostConfig_Check()
		{
			var hostConfig = JsonConvert.DeserializeObject<HostConfig>(File.ReadAllText(configFileInfo.ToString()));

			if (hostConfig is null)
			{
				MessageBox.Show("hostConfig is null", "HostSetting");

				return;
			}
		}

		private void ChangeLoginTextBox()
		{
            var hostConfig = JsonConvert.DeserializeObject<HostConfig>(File.ReadAllText(configFileInfo.ToString()));

			if (hostConfig is null)
			{
				MessageBox.Show("hostconfig is null", "HostSetting");
				return;
			}

			hostTextBox.Text = hostConfig.Host;
			userTextBox.Text = hostConfig.User;
			portTextBox.Text = hostConfig.Port.ToString();
			ChangePortBox.CheckState = hostConfig.PortChangeCheck == 0 ? CheckState.Unchecked: CheckState.Checked;
        }

		private static void ChangeJsonConfig(HostConfig hostConfig)
		{
			File.WriteAllText(configFileInfo.ToString(), JsonConvert.SerializeObject(hostConfig, Formatting.Indented));
		}

		private void HostSetting_Load(object sender, EventArgs e)
		{
			Config_Dir_Check();
			Config_File_Check();
			HostConfig_Check();
			ChangeLoginTextBox();
		}

		private void ChangePortBox_CheckedChanged(object sender, EventArgs e)
		{
			portTextBox.Enabled = ChangePortBox.Checked;
		}


		private void ViewPasswdBox_CheckedChanged(object sender, EventArgs e)
		{
			passwdTextBox.PasswordChar = viewPasswdBox.Checked ? '0' : '●';
		}

		private void LoginBtn_Click(object sender, EventArgs e)
        {
            var hostConfig = new HostConfig
            {
                Host = hostTextBox.Text,
                User = userTextBox.Text,
                Passwd = passwdTextBox.Text,
                Port = Int32.Parse(portTextBox.Text),
                PortChangeCheck = ChangePortBox.Checked ? 1 : 0
            };

            client = ChangePortBox.Checked ?
                module.Conn_SSH(hostConfig.Host, hostConfig.User, hostConfig.Passwd, hostConfig.Port)
                :
                module.Conn_SSH(hostConfig.Host, hostConfig.User, hostConfig.Passwd);

            if(client is null)
			{
				MessageBox.Show("SSH Client is null", "Connection Error");
				return;
			}

            if (!client.IsConnected)
            {
				MessageBox.Show("Connect fail", "Connection Error");
                return;
            }

            ChangeJsonConfig(hostConfig);

            var cmdResult = Module.Module.SendCMD(client, "uname -a");

			Console.WriteLine($"{cmdResult}");
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
			Application.Exit();
        }
    }
}