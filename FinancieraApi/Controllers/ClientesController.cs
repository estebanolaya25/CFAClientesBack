using CapaDTO.Peticiones;
using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.auth.Interfaz;
using CapaNegocio.Interfaz.Clientes.Interfaze;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancieraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {


        private readonly IConfiguration _configuration;


        protected readonly IClienteCapaNegocios _Clientes;




        public ClientesController(IConfiguration _configuration, IClienteCapaNegocios Clientes)
        {
       
            this._configuration = _configuration;

            this._Clientes = Clientes;
  
        }



        [HttpPost]
        [Authorize]
        [Route("GuardaDatosCliente")]
        public async Task<IActionResult> GuardaDatosGenerales(ClienteDto objRegistro)
        {
            try
            {

                bool respuesya = await _Clientes.GuardaDatosClientes(objRegistro);
                return Ok(respuesya);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



         [HttpGet]
        [Authorize]
        [Route("ListaClientes")]
        public async Task<IActionResult> ListaClientes()
        {
           string UserName = "";
            if (User.Identity.IsAuthenticated)
            {
                UserName = User.Identity.Name;
            }
            else
            {
                return BadRequest();
            }
            try
            {
                      
                List<ClienteDto> LstFormualrios = new List<ClienteDto>();
                    LstFormualrios = await _Clientes.ListaClientes();
                return Ok(LstFormualrios);
              

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet]
        [Authorize]
        [Route("EliminarCliente")]
        public async Task<IActionResult> EliminarCliente(int Codigo)
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
            try {
                await _Clientes.EliminarCliente(Codigo);
                     return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }




        [HttpGet]
        [Authorize]
        [Route("GenerarReportexNombre")]
        public async Task<IActionResult> GenerarReportexNombre(string Nombre)
        {
            // Obtén los datos desde la capa de datos o negocios
            // Genera el archivo Exce
            MemoryStream excelStream = await _Clientes.ReportexNombre(Nombre);
            // Devuelve el archivo Excel al cliente
            return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteFormulario.xlsx");
        }




        [HttpGet]
        [Authorize]
        [Route("GenerarReportexNumeroDocumento")]
        public async Task<IActionResult> GenerarReportexNumeroDocumento(string NumeroDocumento)
        {
            // Obtén los datos desde la capa de datos o negocios
            // Genera el archivo Exce
            MemoryStream excelStream = await _Clientes.ReportexNumeroDocumento(NumeroDocumento);
            // Devuelve el archivo Excel al cliente
            return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteFormulario.xlsx");
        }


        [HttpGet]
        [Authorize]
        [Route("GenerarReportexFechas")]
        public async Task<IActionResult> GenerarReportexFechas(string Fechade, string Fechahasta )
        {
            // Obtén los datos desde la capa de datos o negocios
            // Genera el archivo Exce
            MemoryStream excelStream = await _Clientes.ReportexFechas(Fechade, Fechahasta);
            // Devuelve el archivo Excel al cliente
            return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteFormulario.xlsx");
        }



        [HttpGet]
        [Authorize]
        [Route("GenerarReportemasuntelefrono")]
        public async Task<IActionResult> GenerarReportemasuntelefrono()
        {
            // Obtén los datos desde la capa de datos o negocios
            // Genera el archivo Exce
            MemoryStream excelStream = await _Clientes.Reportemas1telefono();
            // Devuelve el archivo Excel al cliente
            return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteFormulario.xlsx");
        }




        [HttpGet]
        [Authorize]
        [Route("GenerarReportemasunDireccion")]
        public async Task<IActionResult> GenerarReportemasunDireccion()
        {
            // Obtén los datos desde la capa de datos o negocios
            // Genera el archivo Exce
            MemoryStream excelStream = await _Clientes.Reportemas1Direccioneso();
            // Devuelve el archivo Excel al cliente
            return File(excelStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteFormulario.xlsx");
        }





    }







}

