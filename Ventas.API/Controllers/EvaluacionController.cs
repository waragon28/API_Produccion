using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InspeccionProduccion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionController : ControllerBase
    {
        private readonly IEvaluacion _Evaluacion;
        private readonly IConfiguration _configuration;
        public EvaluacionController(IEvaluacion _Evaluacion, IConfiguration config)
        {
            this._configuration = config;
            this._Evaluacion = _Evaluacion;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvaluacion(EvaluacionDomain evaluacionDomain)
        {
            Dal.Evaluacion DalAuth = new Dal.Evaluacion(_configuration);

            Response rs = new Response();
            rs = await _Evaluacion.PostEvaluacion(evaluacionDomain);
            return Ok(rs);
        }
    }
}
