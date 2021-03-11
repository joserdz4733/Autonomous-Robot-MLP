using Microsoft.Extensions.DependencyInjection;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Services;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INeuralNetworkRepoService, NeuralNetworkRepoService>();
            services.AddScoped<IImageProcessingConfigService, ImageProcessingConfigService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IMlpService, MlpService>();
            return services;
        }
    }
}
