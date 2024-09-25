using InspeccionProduccion.API.Domain;

namespace InspeccionProduccion.API.Interfaces
{
    public interface IPalet
    {
        Response GetPaler(string NroOf, string CodBarra);
        public Response ListPalet(string FechaIn, string FechaFin);
    }
}
