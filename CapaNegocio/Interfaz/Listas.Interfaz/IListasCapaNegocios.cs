using CapaDTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Interfaz.Listas.Interfaz
{
    public interface IListasCapaNegocios
    {

        Task<List<OpcionDto>> TiposDocumentos();

    }
}
