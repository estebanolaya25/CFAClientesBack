using CapaDatos.Interza.Listas.Interfaz;
using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.Listas.Interfaz;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementacion.Listas.Implementacion
{



    public class clsListasCombosCapaNegocios : IListasCapaNegocios
    {



        private readonly IConfiguration _configuration;

        private readonly IListasCombosCapaDatos _ListasCombos;


        public clsListasCombosCapaNegocios(IConfiguration configuration, IListasCombosCapaDatos ListasCombos)
        {
            this._ListasCombos = ListasCombos;
            _configuration = configuration;

        }


        public async Task<List<OpcionDto>> TiposDocumentos()
        {
            return await _ListasCombos.TiposDocumentos();
        }



        }
}
