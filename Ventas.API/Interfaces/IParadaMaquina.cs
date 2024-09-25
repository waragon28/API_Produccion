using InspeccionProduccion.API.Domain;
using Produccion.API.Domain;

namespace Produccion.API.Interfaces
{
    public interface IParadaMaquina
    {
        Task<Response> RegistroParadaMaquina(ParadaMaquinaDomain paradaMaquinaDomain);
        Response GetArea();
        Response GetMaquina();
        Response GetMotivoParada(string Area);
        Response GetListaParadaMaquina(FiltroListParaMaquiDomain filtroListParaMaquiDomain);
        Response StockParadaMaquina(string DocEntry);
        
    }
}
