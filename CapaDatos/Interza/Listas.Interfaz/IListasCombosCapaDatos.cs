using CapaDTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Interza.Listas.Interfaz
{
    public interface IListasCombosCapaDatos
    {
        Task<List<OpcionDto>> TiposDocumentos();
    }
}
