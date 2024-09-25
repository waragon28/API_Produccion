using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using InspeccionProduccion.API.Dal;

namespace InspeccionProduccion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _Auth;
        private readonly IConfiguration _configuration;
        public AuthController(IAuth _Auth, IConfiguration config)
        {
            this._configuration = config;
            this._Auth = _Auth;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(LoginRequestDto loginRequestDto)//([FromBody] LoginRequestDto loginRequestDto)//(string User,string PassWord)
        {
            Dal.Auth DalAuth = new Dal.Auth(_configuration);

            Response rs = new Response();
            rs = await _Auth.Authenticate(loginRequestDto.User, loginRequestDto.PassWord);//(User, PassWord);//;(loginRequestDto.User, loginRequestDto.PassWord);
            return Ok(rs);
        }

    }
}
