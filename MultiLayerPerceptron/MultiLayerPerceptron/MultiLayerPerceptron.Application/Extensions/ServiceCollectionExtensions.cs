using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Application.Profiles;
using MultiLayerPerceptron.Application.Services;
using System.Collections.Generic;
using System.Linq;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IList<Profile> Profiles = new List<Profile>();
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INeuralNetworkRepoService, NeuralNetworkRepoService>();
            services.AddScoped<IImageProcessingConfigService, ImageProcessingConfigService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IMlpService, MlpService>();

            #region Mapping Profiles
            services.AddMappingProfile<ApplicationMappingProfile>();
            #endregion
            return services;
        }

        public static void AddMappingProfile<T>(this IServiceCollection _) where T : Profile, new()
        {
            if (Profiles.All(e => e.GetType() != typeof(T)))
            {
                Profiles.Add(new T());
            }
        }
    }
}
