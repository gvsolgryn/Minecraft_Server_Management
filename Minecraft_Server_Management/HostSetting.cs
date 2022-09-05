using Minecraft_Server_Management.Module;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private static void Config_Dir_Check()
        {
            var configPath = new DirectoryInfo(Application.StartupPath + @"config\");

            if (!configPath.Exists)
            {
                Debug.WriteLine("[HostSetting] Config path is not found\nCreate Directory");
                configPath.Create();
            }
        }

        private static void Config_File_Check()
        {
            var configPath = new DirectoryInfo(Application.StartupPath + @"config\");

            var configFileInfo = new FileInfo(configPath + @"hostConfig.json");

            var hostConfig = new HostConfig
            {
                Host = "exam.host.name || 127.0.0.1",
                User = "username",
                Passwd = "password",
                Port = 22,
                PortChangeCheck = 0,
                AutoLogin = 0
            };

            var Config2Json = new JObject(
                new JProperty("Host", hostConfig.Host),
                new JProperty("User", hostConfig.User),
                new JProperty("Passwd", hostConfig.Passwd),
                new JProperty("Port", hostConfig.Port),
                new JProperty("PortChangeCheck", hostConfig.PortChangeCheck),
                new JProperty("AutoLogin", hostConfig.AutoLogin)
                );

            if (!configFileInfo.Exists)
            {
                Debug.WriteLine("[HostSetting] Config file is not found\nCreate file");

                File.WriteAllText(configFileInfo.ToString(), Config2Json.ToString());
            }
        }

        private void HostConfig_Check()
        {
            var configPath = new DirectoryInfo(Application.StartupPath + @"config\");
            var configFileInfo = new FileInfo(configPath + @"hostConfig.json");

            var hostConfig = JsonConvert.DeserializeObject<HostConfig>(File.ReadAllText(configFileInfo.ToString()));

            if (hostConfig is null)
            {
                Debug.WriteLine("[HostSetting] hostConfig is null");

                return;
            }
        }

        private void HostSetting_Load(object sender, EventArgs e)
        {
            Config_Dir_Check();
            Config_File_Check();
            HostConfig_Check();
        }

        private void ChangePortBox_CheckedChanged(object sender, EventArgs e)
        {
            portTextBox.Enabled = ChangePortBox.Checked;
        }
    }
}