using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace InspeccionProduccion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaletController : ControllerBase
    {
        private readonly IPalet _Palet;
        private readonly IConfiguration _configuration;
        public PaletController(IPalet _Palet, IConfiguration config)
        {
            this._configuration = config;
            this._Palet = _Palet;
        }

        [HttpPost]
        public IActionResult GetPalet([FromBody] BarraDomain Barra)
        {
            Response rs = new Response();
            Dal.Palet DalPalet = new Dal.Palet(_configuration);
            try
            {
                string pattern = @"\(\d{2}\)(\d+)";
                if (Barra.Barra.Length == 9)
                {
                    rs = this._Palet.GetPaler("", Barra.Barra);
                }
                else
                {
                    MatchCollection matches = Regex.Matches(Barra.Barra ?? string.Empty, pattern);
                    string NroOF = string.Empty;
                    string CodBarra = string.Empty;
                    if (matches.Count > 1)
                    {
                        NroOF = matches[0].Groups[1].Value;
                        CodBarra = matches[1].Groups[1].Value;

                    }

                    rs = this._Palet.GetPaler(NroOF, CodBarra);
                }

            }
            catch (Exception ex)
            {
                rs.data = ex.Message.ToString();
            }
            finally
            {

            }

            return Ok(rs);
        }

        [HttpPost("ListaRegistros")]
        public IActionResult ListPalet([FromBody] FiltroFecha filtro)
        {
            Response rs = new Response();
            Dal.Palet DalPalet = new Dal.Palet(_configuration);
            try
            {
                    rs = this._Palet.ListPalet(filtro.FechaIni, filtro.FechaFin);

            }
            catch (Exception ex)
            {
                rs.data = ex.Message.ToString();
            }
            finally
            {

            }

            return Ok(rs);
        }


    }
}   

