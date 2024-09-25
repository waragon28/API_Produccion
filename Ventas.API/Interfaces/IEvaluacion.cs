using InspeccionProduccion.API.Domain;

namespace InspeccionProduccion.API.Interfaces
{
    public interface IEvaluacion
    {
        Task<Response> PostEvaluacion(EvaluacionDomain evaluacionDomain);
    }
}
