using Microsoft.AspNetCore.Mvc;
using InspeccionProduccion.API.Domain;

namespace InspeccionProduccion.API.Interfaces
{
    public interface IAuth
    {
        Task<Response>Authenticate(string User,string Password);
    }
}
