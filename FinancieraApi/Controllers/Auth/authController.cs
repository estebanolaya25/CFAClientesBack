using CapaDTO.Peticiones;
using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.auth.Interfaz;
using FinancieraApi.jtw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinancieraApi.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {

        protected readonly IloginCapaNegocios _iloginCapaNegocios;
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _itogengenerator;

        public authController(IloginCapaNegocios iloginCapaNegocios, TokenGenerator itogengenerator, IConfiguration configuration)
        {
            this._iloginCapaNegocios = iloginCapaNegocios;
            _configuration = configuration;
            _itogengenerator = itogengenerator;

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Authenticate(LoginRequest login)
        {
            if (login == null)
            {
                return BadRequest(ModelState);
            }
            DataTable dtInformacion = await _iloginCapaNegocios.autenticarUsuario(login.email, login.password);
            if (dtInformacion.Rows.Count > 0)
            {
                var token = _itogengenerator.GenerateTokenJwt(login, dtInformacion);
                return Ok(new { token = token });
            }
            else
            {
                return Unauthorized();
            }
        }




        [HttpGet]
        [Authorize]
        [Route("current")]
        public async Task<IActionResult> userinformartion2()
        {
            string UserName = "";

            if (User.Identity.IsAuthenticated)
            {
                UserName = User.Identity.Name;

            }
            else
            {
                return Unauthorized();
            }

            CurrentUserInfo userinfo = new CurrentUserInfo();
            userinfo = await _iloginCapaNegocios.Isadmin(UserName);
            return Ok(userinfo);
        }





    }
}
