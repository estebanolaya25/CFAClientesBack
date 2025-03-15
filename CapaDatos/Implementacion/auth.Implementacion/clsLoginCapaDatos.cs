
using CapaDatos.Interza.auth.Interfaz;
using CapaDatos.util;
using CapaDTO.Respuestas;
using Microsoft.Extensions.Configuration;


using System.Data;


namespace CapaDatos.Implementacion.auth.Implementacion
{
    public class clsLoginCapaDatos : IloginCapaDatos
    {

        private cEncriptacion cEncrypt = new cEncriptacion();
        private cDataBase cDataBase;
        private readonly IConfiguration _configuration;


        public clsLoginCapaDatos(IConfiguration configuration)
        {
            _configuration = configuration;
            cDataBase = new cDataBase(configuration);
        }

        public async Task<DataTable> autenticarUsuario(string Usuario, string Contrasena)
        {

            DataTable dtInformacion = new DataTable();
            string strConsulta = string.Empty, strEncryptPass = string.Empty;

            try
            {
                strEncryptPass = cEncriptacion.CifradoData(Contrasena);//cEncriptacion.Base64_Encode(Contrasena);//mtdEncriptarContrasena(Contrasena);
                //06/11/2014
                strConsulta = "select Id,Nombre,Apellidos FROM [dbo].[tbl_Usuarios] where Usuario ='" + Usuario + "' and Password='" + strEncryptPass + "' and Activo=1";
                // strConsulta = "  Select Nombres,Apellidos, Id, TipoUsuario,Email from [Listas].[Usuarios] where Usuario = '" + Usuario + "'  and Contrasena = '" + strEncryptPass + "'";
                cDataBase.conectar();
                dtInformacion = cDataBase.ejecutarConsulta(strConsulta);
                cDataBase.desconectar();
            }
            catch (Exception ex)
            {
                cDataBase.desconectar();
                dtInformacion.Rows.Clear();
                dtInformacion.Columns.Clear();

            }

            return dtInformacion;
        }



        public async Task<CurrentUserInfo> Isadmin(string username)
        {
            DataTable dtInformacion = new DataTable();
            string strConsulta;
            try
            {
                strConsulta = String.Format("SELECT u.id,u.Nombre,u.Apellidos,u.Email,b.Nombre as Rol,u.ActualizarPass FROM tbl_Usuarios u  inner join [dbo].[tbl_TipoUsuario] b on (u.TipoUsuario=b.Id)  where u.Usuario='{0}' GROUP BY u.Id, u.Nombre, u.Apellidos,u.Email,b.Nombre,u.IdArea,u.ActualizarPass", username);
                cDataBase.conectar();
                dtInformacion = cDataBase.ejecutarConsulta(strConsulta);
                cDataBase.desconectar();
            }
            catch (Exception ex)
            {
                cDataBase.desconectar();
                dtInformacion.Rows.Clear();
                dtInformacion.Columns.Clear();
            }

            if (dtInformacion.Rows.Count > 0)
            {
                CurrentUserInfo userinfo = new CurrentUserInfo();
                userinfo.Nombre = dtInformacion.Rows[0]["Nombre"].ToString().Trim();
                userinfo.Apellido = dtInformacion.Rows[0]["Apellidos"].ToString().Trim();
                userinfo.Usuario = dtInformacion.Rows[0]["Email"].ToString().Trim();
                userinfo.Rol = dtInformacion.Rows[0]["Rol"].ToString().Trim();
                userinfo.ActualizarPass = string.IsNullOrEmpty(dtInformacion.Rows[0]["ActualizarPass"].ToString()) ? true : Convert.ToBoolean(dtInformacion.Rows[0]["ActualizarPass"]);

                return userinfo;

            }
            else
            {
                return null;
            }

        }




    }
}
