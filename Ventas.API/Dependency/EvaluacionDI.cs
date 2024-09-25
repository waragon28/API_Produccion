using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;

namespace InspeccionProduccion.API.Dependency
{
    public static class EvaluacionDI
    {
        public static IServiceCollection AddEvaluacion(this IServiceCollection services,
        Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.Configure<ServiceLayerSettings>(configuration.GetSection(ServiceLayerSettings.SectionName));
            services.AddSingleton<IEvaluacion, Dal.Evaluacion>();

            return services;
        }
    }
}
