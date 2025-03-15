using CapaDTO.Peticiones;
using CapaDTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Interza.Cliente.Interfaze
{
    public interface IClienteCapaDatos
    {
        Task<bool> GuardaDatosClientes(ClienteDto objRegistro);

        Task<List<ClienteDto>> ListaClientes();

        Task EliminarCliente(int CodigoCliente);

        Task<List<ClienteDto>> ReporteListaClientesXNombre(string Nombre);

        Task<List<ClienteDto>> ReporteListaClientesXNumeroDocumento(string NumeroDocumento);

        Task<List<ClienteDto>> ReporteListaClientesXFechas(string fechade, string fechahasta);
        Task<List<CantidadesDTO>> Reportemas1telefono();
        Task<List<CantidadesDTO>> Reportemas1Direcciones();
    }
}
