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
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IImageProcessingConfigService, ImageProcessingConfigService>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IMlpService, MlpService>();

            #region Mapping Profiles
            services.AddMappingProfile<ApplicationMappingProfile>();
            services.AddMappingService();
            #endregion
            return services;
        }

        public static void AddMappingService(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfiles(Profiles); });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
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
