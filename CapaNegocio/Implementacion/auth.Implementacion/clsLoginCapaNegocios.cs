using CapaDatos.Interza.auth.Interfaz;
using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.auth.Interfaz;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementacion.auth.Implementacion
{
    public class clsLoginCapaNegocios : IloginCapaNegocios
    {

        protected readonly IloginCapaDatos ilogincapadatos;
        private readonly IConfiguration _configuration;
     

        public clsLoginCapaNegocios(IloginCapaDatos ilogincapadatos, IConfiguration configuration)
        {
            this.ilogincapadatos = ilogincapadatos;
            _configuration = configuration;
        }

        public async Task<DataTable> autenticarUsuario(string Usuario, string Contrasena)
        {
            return await ilogincapadatos.autenticarUsuario(Usuario, Contrasena);

        }

        public async Task<CurrentUserInfo> Isadmin(string username)
        {
            return await ilogincapadatos.Isadmin(username);
        }




        }
}
