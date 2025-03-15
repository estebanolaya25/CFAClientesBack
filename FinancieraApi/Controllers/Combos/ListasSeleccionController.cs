using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.Listas.Interfaz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancieraApi.Controllers.Combos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListasSeleccionController : ControllerBase
    {

        protected readonly IListasCapaNegocios _IListasCapaNegocios;
        private readonly IConfiguration _configuration;


        public ListasSeleccionController(IConfiguration _configuration, IListasCapaNegocios IListasCapaNegocios)
        {
            this._IListasCapaNegocios = IListasCapaNegocios;
            this._configuration = _configuration;
        }



        [HttpGet]
        [Authorize]
        [Route("listaTiposDocumentos")]
        public async Task<IActionResult> listaTiposDocumentos()
        {
            List<OpcionDto> lstTiposDocumentos = new List<OpcionDto>();


            lstTiposDocumentos = await _IListasCapaNegocios.TiposDocumentos();

            return Ok(lstTiposDocumentos);
        }


    }
}
