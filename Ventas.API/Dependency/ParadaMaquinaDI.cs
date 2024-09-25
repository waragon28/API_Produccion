using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using Produccion.API.Dal;
using Produccion.API.Interfaces;

namespace Produccion.API.Dependency
{
    public static class ParadaMaquinaDI
    {
        public static IServiceCollection AddParadaMaquina(this IServiceCollection services,
          Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<ServiceLayerSettings>(configuration.GetSection(ServiceLayerSettings.SectionName));
            services.AddSingleton<IParadaMaquina, Dal.ParadaMaquinaDAL>();

            return services;
        }
    }
}
