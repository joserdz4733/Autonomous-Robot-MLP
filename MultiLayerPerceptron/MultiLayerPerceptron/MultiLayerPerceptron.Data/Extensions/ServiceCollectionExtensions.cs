using Microsoft.Extensions.DependencyInjection;

namespace MultiLayerPerceptron.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
