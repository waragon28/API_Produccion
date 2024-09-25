using InspeccionProduccion.API.Dal;
using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Produccion.API.Dal;
using Produccion.API.Domain;
using Produccion.API.Interfaces;

namespace Produccion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParadaMaquinaController : ControllerBase
    {

        private readonly IParadaMaquina _IParadaMaquina;
        private readonly IConfiguration _configuration;

        public ParadaMaquinaController(IParadaMaquina _IParadaMaquina, IConfiguration config)
        {
            this._configuration = config;
            this._IParadaMaquina = _IParadaMaquina;
        }

        [HttpPost]
        public async Task<IActionResult> PostParadaMaquina(ParadaMaquinaDomain paradaMaquinaDomain)
        {
           ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = await _IParadaMaquina.RegistroParadaMaquina(paradaMaquinaDomain);
            return Ok(rs);
        }

        [HttpGet("Area")]
        public IActionResult GetArea()
        {
            ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = _IParadaMaquina.GetArea();
            return Ok(rs);
        }

       [HttpGet("Maquina")]
        public IActionResult GetMaquina()
        {
            ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = _IParadaMaquina.GetMaquina();
            return Ok(rs);
        }


        [HttpGet("MotivoParada")]
        public IActionResult GetMotivoParada(string Area)
        {
            ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = _IParadaMaquina.GetMotivoParada(Area);
            return Ok(rs);
        }


        [HttpPost("ListaParadaMaquina")]
        public IActionResult ListaParadaMaquina(FiltroListParaMaquiDomain filtroListParaMaquiDomain)
        {
            ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = _IParadaMaquina.GetListaParadaMaquina(filtroListParaMaquiDomain);
            return Ok(rs);
        }

        [HttpPatch("StockParadaMaquina")]
        public IActionResult StockParadaMaquina(string DocEntry)
        {
            ParadaMaquinaDAL paradaMaquinaDAL = new ParadaMaquinaDAL(_configuration);

            Response rs = new Response();
            rs = _IParadaMaquina.StockParadaMaquina(DocEntry);
            return Ok(rs);
        }

    }
}
