using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;

namespace InspeccionProduccion.API.Dependency
{
    public static class Palet
    {
        public static IServiceCollection AddPalet(this IServiceCollection services,
       Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<ServiceLayerSettings>(configuration.GetSection(ServiceLayerSettings.SectionName));
            services.AddSingleton<IPalet, Dal.Palet>();

            return services;
        }

    }
}
