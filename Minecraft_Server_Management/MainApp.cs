namespace Minecraft_Server_Management
{
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();

            var hostSettingForm = new HostSetting();

            hostSettingForm.ShowDialog();
        }
    }
}