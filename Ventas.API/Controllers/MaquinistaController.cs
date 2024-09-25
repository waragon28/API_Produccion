using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InspeccionProduccion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaquinistaController : ControllerBase
    {
        private readonly IMaquinista _Maquinista;
        private readonly IConfiguration _configuration;
        public MaquinistaController(IMaquinista _Maquinista, IConfiguration config)
        {
            this._configuration = config;
            this._Maquinista = _Maquinista;
        }

        [HttpGet]
        public IActionResult GetMaquinista()
        {
           Dal.Maquinista DalMaquinista = new Dal.Maquinista(_configuration);

            Response rs = new Response();
            rs = this._Maquinista.GetMaquinista();
            return Ok(rs);
        }

        [HttpGet("Line")]
        public IActionResult GetLineas()
        {
            Dal.Maquinista DalMaquinista = new Dal.Maquinista(_configuration);
            Response rs = new Response();
            rs = this._Maquinista.GetLineas();
            return Ok(rs);
        }


    }
}
