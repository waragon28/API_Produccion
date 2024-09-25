using InspeccionProduccion.API.Dal;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Domain;

namespace InspeccionProduccion.API.Dependency
{
    public static class AuthDI{

        public static IServiceCollection AddAuth(this IServiceCollection services,
          Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<ServiceLayerSettings>(configuration.GetSection(ServiceLayerSettings.SectionName));
            services.AddSingleton<IAuth, Dal.Auth>();

            return services;
        }

    }
}
