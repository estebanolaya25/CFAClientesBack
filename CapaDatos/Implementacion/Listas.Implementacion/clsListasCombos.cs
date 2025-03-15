using CapaDatos.Interza.Listas.Interfaz;
using CapaDatos.util;
using CapaDTO.Respuestas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Implementacion.Listas.Implementacion
{
    public class clsListasCombos : IListasCombosCapaDatos
    {
        private cDataBase cDataBase;
        private readonly IConfiguration _configuration;

        public clsListasCombos(IConfiguration configuration)
        {
            _configuration = configuration;
            cDataBase = new cDataBase(configuration);
        }

        public async Task<List<OpcionDto>> TiposDocumentos()
        {
            List<OpcionDto> listaAreasDtos = new List<OpcionDto>();

            DataTable dtInformacion = ConsultaTiposDocumentos();
            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {
                    OpcionDto objlista = new OpcionDto();
                    objlista.Id = dtInformacion.Rows[rows]["Id"].ToString();
                    objlista.Nombre = dtInformacion.Rows[rows]["Nombre"].ToString().Trim();

                    listaAreasDtos.Add(objlista);
                }
            }
            return listaAreasDtos;

        }


        private DataTable ConsultaTiposDocumentos()
        {
            DataTable dtInformacion = new DataTable();
            try
            {
                string Consulta = string.Format("SELECT Id,TipoDocumento as Nombre FROM [dbo].[tbl_TiposDocumentos]  ");
                cDataBase.conectar();
                dtInformacion = cDataBase.ejecutarConsulta(Consulta);
                cDataBase.desconectar();
            }
            catch (Exception ex)
            {
                cDataBase.desconectar();
                dtInformacion.Rows.Clear();
                dtInformacion.Columns.Clear();
                throw new Exception(ex.Message);
            }
            return dtInformacion;
        }



    }
}
