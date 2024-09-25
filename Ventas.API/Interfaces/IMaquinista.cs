using InspeccionProduccion.API.Domain;

namespace InspeccionProduccion.API.Interfaces
{
    public interface IMaquinista
    {
        Response GetMaquinista();
        Response GetLineas();
        
    }
}
