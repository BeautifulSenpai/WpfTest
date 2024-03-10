using Microsoft.Win32;
using System.Windows;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!IsAutoStartEnabled())
            {
                EnableAutoStart();
            }
        }

        private bool IsAutoStartEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            return key.GetValue("Healthy Kids") != null;
        }

        private void EnableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("Healthy Kids", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }
}

