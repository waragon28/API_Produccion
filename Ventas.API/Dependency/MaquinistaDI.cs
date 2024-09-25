using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;

namespace InspeccionProduccion.API.Dependency
{
    public static class MaquinistaDI
    {
        public static IServiceCollection AddMaquinista(this IServiceCollection services,
          Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<ServiceLayerSettings>(configuration.GetSection(ServiceLayerSettings.SectionName));
            services.AddSingleton<IMaquinista, Dal.Maquinista>();

            return services;
        }
    }
}
