using Microsoft.Extensions.DependencyInjection;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
