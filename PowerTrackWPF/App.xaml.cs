using System.Configuration;
using System.Data;
using System.Windows;
using PowerTrackWPF.Views;
using System.Runtime.InteropServices;

namespace PowerTrackWPF
{
    public partial class App : Application
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AllocConsole();
            Console.WriteLine("🟢 Consola iniciada correctamente");

            var login = new LoginView();
            login.Show();
        }
    }
}
