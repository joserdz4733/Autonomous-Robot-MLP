using Microsoft.Extensions.DependencyInjection;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Data.Extensions;
using System;
using System.Configuration;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices()
                .AddDataServices(ConfigurationManager.ConnectionStrings["MlpConnection"].ConnectionString);
            services.AddSingleton<MainWindow>();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
