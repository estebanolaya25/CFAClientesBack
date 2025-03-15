using CapaDTO.Peticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Interfaz.Clientes.Interfaze
{
    public interface IClienteCapaNegocios
    {
        Task<bool> GuardaDatosClientes(ClienteDto objRegistro);

        Task<List<ClienteDto>> ListaClientes();

        Task EliminarCliente(int CodigoCliente);


        Task<MemoryStream> ReportexNombre(string Nombre);
        Task<MemoryStream> ReportexNumeroDocumento(string NumeroDocumento);

        Task<MemoryStream> ReportexFechas(string fechade, string fechahasta);
        Task<MemoryStream> Reportemas1telefono();
        Task<MemoryStream> Reportemas1Direccioneso();
    }
}
