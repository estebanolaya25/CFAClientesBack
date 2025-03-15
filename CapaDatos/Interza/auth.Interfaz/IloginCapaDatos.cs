using CapaDTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Interza.auth.Interfaz
{
    public interface IloginCapaDatos
    {
        Task<DataTable> autenticarUsuario(string Usuario, string Contrasena);

        Task<CurrentUserInfo> Isadmin(string username);
    }
}
