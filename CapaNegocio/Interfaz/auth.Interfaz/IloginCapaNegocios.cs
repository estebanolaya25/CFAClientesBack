using CapaDatos.Interza.auth.Interfaz;
using CapaDTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Interfaz.auth.Interfaz
{
    public interface IloginCapaNegocios
    {
        Task<DataTable> autenticarUsuario(string Usuario, string Contrasena);

         Task<CurrentUserInfo> Isadmin(string username);

    }
}
