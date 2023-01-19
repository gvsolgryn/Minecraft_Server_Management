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
				Debug.WriteLine("[HostSetting] Config path is not found\nCreate Directory");
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
				PortChangeCheck = 0,
				AutoLogin = 0
			};

			var firstDataList = new List<Object>
			{
				hostConfig
			};

			if (!configFileInfo.Exists)
			{
				MessageBox.Show("[HostSetting] Config file is not found\nCreate file");

				File.WriteAllText(configFileInfo.ToString(), JsonConvert.SerializeObject(firstDataList, Formatting.Indented));
			}
		}

		private static void HostConfig_Check()
		{
			var hostConfig = JsonConvert.DeserializeObject<List<HostConfig>>(File.ReadAllText(configFileInfo.ToString()));

			if (hostConfig is null)
			{
				MessageBox.Show("[HostSetting] hostConfig is null");

				return;
			}
		}

		private void AutoLogin_Check()
		{
			var hostConfig = JsonConvert.DeserializeObject<List<HostConfig>>(File.ReadAllText(configFileInfo.ToString()));

			if (hostConfig is null)
			{
				MessageBox.Show("[HostSetting] hostConfig is null");

				return;
			}

			var thisConfig = hostConfig.FirstOrDefault();

			if (thisConfig is not null && thisConfig.AutoLogin == 0)
			{
				MessageBox.Show("[HostSetting] Auto Login OFF");
			}
		}

		private void HostSetting_Load(object sender, EventArgs e)
		{
			Config_Dir_Check();
			Config_File_Check();
			HostConfig_Check();
			AutoLogin_Check();
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
				PortChangeCheck = ChangePortBox.Checked ? 1 : 0,
				AutoLogin = AutoLoginBox.Checked ? 1 : 0
            };
			
			SshClient? client = ChangePortBox.Checked ?
				Module.Module.Conn_SSH(hostConfig.Host, hostConfig.User, hostConfig.Passwd, hostConfig.Port)
				:
				Module.Module.Conn_SSH(hostConfig.Host, hostConfig.User, hostConfig.Passwd);
		}

        private void CancelBtn_Click(object sender, EventArgs e)
        {
			Application.Exit();
        }
    }
}