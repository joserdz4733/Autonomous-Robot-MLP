using ClientApp.BL;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {   
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FnInitializeApp();
        }

        private void FnInitializeApp()
        {
            Config.ApiBaseAddress = System.Configuration.ConfigurationSettings.AppSettings["API_BASE_ADDRESS"];
            Config.DefaultRequestHearders = System.Configuration.ConfigurationSettings.AppSettings["DEFAULT_REQUEST_HEADERS"];
        }

    }
}
