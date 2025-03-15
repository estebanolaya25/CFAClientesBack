using CapaDatos.Interza.Cliente.Interfaze;
using CapaDatos.util;
using CapaDTO.Peticiones;
using CapaDTO.Respuestas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Implementacion.Cliente.Implementacion
{
    public class clsClienteCapaDatos : IClienteCapaDatos
    {

        private cDataBase cDataBase;
        private readonly IConfiguration _configuration;



        public clsClienteCapaDatos(IConfiguration configuration)
        {
            _configuration = configuration;
            cDataBase = new cDataBase(_configuration);
        }




        public async Task<bool> GuardaDatosClientes(ClienteDto objRegistro)
        {
            bool Respeusta = false;

            if (objRegistro.Codigo == 0)
            {

                if (ExisteCliente(objRegistro))
                {
                    throw new Exception("Cliente Existe");
                    return Respeusta;
                }
                else
                {
                    int codigo = await GuardaDatosCliente(objRegistro);
                    await GuardaDirecciones(objRegistro.direcciones, codigo);
                    await GuardaTelefonos(objRegistro.telefonos, codigo);
                    Respeusta = true;
                }
            }
            else {
               await   EditaDatosCliente(objRegistro);
                await GuardaDirecciones(objRegistro.direcciones, objRegistro.Codigo);
                await GuardaTelefonos(objRegistro.telefonos, objRegistro.Codigo);

                Respeusta = true;

            }

            return Respeusta;
        }



        public async Task<List<ClienteDto>> ListaClientes()
        {

            List<ClienteDto> ListaClientes = new List<ClienteDto>();
            DataTable dtInformacion = ConsultaListaClientes();

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    ClienteDto objForm = new ClienteDto();
                    objForm.Codigo = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.IdTipoDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["IdTipoDocumento"]);
                    objForm.TipoDocumento = dtInformacion.Rows[rows]["TipoDocumento"].ToString();
                    objForm.NumeroDocumento= Convert.ToInt32(dtInformacion.Rows[rows]["NumeroDocumento"]);
                    objForm.Nombres = dtInformacion.Rows[rows]["Nombres"].ToString();
                    objForm.Apellido1 = dtInformacion.Rows[rows]["Apellido1"].ToString();
                    objForm.Apellido2 = dtInformacion.Rows[rows]["Apellido2"].ToString();
                    objForm.Genero = dtInformacion.Rows[rows]["Genero"].ToString();
                    // Obtén la fecha como string desde el DataTable
                    string fechaOriginal = dtInformacion.Rows[rows]["FechaNacimiento"].ToString();
                    // Convierte la fecha a un objeto DateTime
                    DateTime fechaConvertida = DateTime.Parse(fechaOriginal);
                    // Formatea la fecha al formato "yyyy-MM-dd"
                    objForm.FechaNacimiento = fechaConvertida.ToString("yyyy-MM-dd");

                    objForm.Email = dtInformacion.Rows[rows]["Email"].ToString();

                    objForm.direcciones = await ListaDireccionesxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    objForm.telefonos = await ListaTelefornosxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }
        private DataTable ConsultaListaClientes()
        {


            string Consulta = string.Empty;

            Consulta = string.Format("select a.Id,a.IdTipoDocumento,b.TipoDocumento,a.NumeroDocumento, a.Nombres,a.Apellido1,a.Apellido2,a.Genero,a.FechaNacimiento,a.Email from [dbo].[tbl_Clientes] as a  inner join [dbo].[tbl_TiposDocumentos] as b on (a.IdTipoDocumento=b.Id) order by a.Nombres asc");

            DataTable dtInformacion = new DataTable();
            try
            {
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


        public async Task<List<Direcciones>> ListaDireccionesxCliente(int Codigo)
        {

            List<Direcciones> ListasDirecciones = new List<Direcciones>();
            DataTable dtInformacion = consultaDireccionesxcliente(Codigo);

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    Direcciones objForm = new Direcciones();
                    objForm.Id = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.Direccion =dtInformacion.Rows[rows]["Direccion"].ToString();
                    objForm.Clase = dtInformacion.Rows[rows]["Clase"].ToString();
                    objForm.IdCliente = Codigo;
                    ListasDirecciones.Add(objForm);
                }
            }

            return ListasDirecciones;
        }



        private DataTable consultaDireccionesxcliente(int codigo)
        {
            string Consulta = string.Empty;
            Consulta = string.Format("SELECT [Id] ,[Direccion]  ,[Clase] ,[IdCliente]  FROM [FCC_Financ].[dbo].[tbl_Direcciones] where IdCliente={0}",codigo);
            DataTable dtInformacion = new DataTable();
            try
            {
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




        public async Task<List<Telefonos>> ListaTelefornosxCliente(int Codigo)
        {

            List<Telefonos> ListasTeleforno = new List<Telefonos>();
            DataTable dtInformacion = consultaTelefonossxcliente(Codigo);

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    Telefonos objForm = new Telefonos();
                    objForm.Id = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.Telefono = dtInformacion.Rows[rows]["Telefono"].ToString();
                    objForm.Clase = dtInformacion.Rows[rows]["Clase"].ToString();
                    objForm.IdCliente = Codigo;
                    ListasTeleforno.Add(objForm);
                }
            }

            return ListasTeleforno;
        }



        private DataTable consultaTelefonossxcliente(int codigo)
        {
            string Consulta = string.Empty;
            Consulta = string.Format("SELECT [Id] ,[Telefono] ,[Clase] ,[IdCliente] FROM [FCC_Financ].[dbo].[tbl_Telefonos] where IdCliente={0}", codigo);
            DataTable dtInformacion = new DataTable();
            try
            {
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







        private bool ExisteCliente(ClienteDto objCliente)
        {


            string Consulta = string.Empty;

            Consulta = string.Format("select * from [dbo].[tbl_Clientes] where [IdTipoDocumento]={0} and [NumeroDocumento]={1}", objCliente.IdTipoDocumento, objCliente.NumeroDocumento);

            DataTable dtInformacion = new DataTable();
            try
            {
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
            if (dtInformacion.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private async Task<int> GuardaDatosCliente(ClienteDto objCliente)
        {
            bool respuesta = false;
            int codigo=0; ;
            string connString = _configuration.GetConnectionString("SQLConnectionString");

            string query = "INSERT INTO [dbo].[tbl_Clientes] " +
                                  "VALUES (@IdTipoDocumento, @NumeroDocumento, @Nombres, @Apellido1, @Apellido2, @Genero, @FechaNacimiento, @Email); " +
                                  "SELECT SCOPE_IDENTITY();";

            DateTime FechaCreacion = DateTime.Now;
            DateTime fecha = DateTime.Parse(objCliente.FechaNacimiento);

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    // Abrir la conexión
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar valores a los parámetros
                        command.Parameters.AddWithValue("@IdTipoDocumento", objCliente.IdTipoDocumento);
                        command.Parameters.AddWithValue("@NumeroDocumento", objCliente.NumeroDocumento);
                        command.Parameters.AddWithValue("@Nombres", objCliente.Nombres);
                        command.Parameters.AddWithValue("@Apellido1", objCliente.Apellido1);
                        command.Parameters.AddWithValue("@Apellido2", objCliente.Apellido2);
                        command.Parameters.AddWithValue("@Genero", objCliente.Genero);
                        command.Parameters.AddWithValue("@FechaNacimiento", fecha.ToString("yyyy-dd-MM"));
                        command.Parameters.AddWithValue("@Email", objCliente.Email);

                        // Ejecutar la consulta y obtener el valor de SCOPE_IDENTITY()
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {



                           return  Convert.ToInt32(result);
                        }
                        else
                        {
                            throw new Exception("Error al crear Cliente");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al crear Cliente");
            }

            return 0;


        }


        private async Task EditaDatosCliente(ClienteDto objCliente)
        {
            bool respuesta = false;

            string connString = _configuration.GetConnectionString("SQLConnectionString");

            string query = "UPDATE [dbo].[tbl_Clientes] SET IdTipoDocumento=@IdTipoDocumento,NumeroDocumento=@NumeroDocumento,Nombres=@Nombres,Apellido1=@Apellido1,Apellido2=@Apellido2, Genero=@Genero,FechaNacimiento=@FechaNacimiento,Email=@Email where Id=@Codigo";

            DateTime FechaCreacion = DateTime.Now;
            DateTime fecha = DateTime.Parse(objCliente.FechaNacimiento);

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    // Abrir la conexión
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar valores a los parámetros
                        command.Parameters.AddWithValue("@Codigo", objCliente.Codigo);
                        command.Parameters.AddWithValue("@IdTipoDocumento", objCliente.IdTipoDocumento);
                        command.Parameters.AddWithValue("@NumeroDocumento", objCliente.NumeroDocumento);
                        command.Parameters.AddWithValue("@Nombres", objCliente.Nombres);
                        command.Parameters.AddWithValue("@Apellido1", objCliente.Apellido1);
                        command.Parameters.AddWithValue("@Apellido2", objCliente.Apellido2);
                        command.Parameters.AddWithValue("@Genero", objCliente.Genero);
                        command.Parameters.AddWithValue("@FechaNacimiento", fecha.ToString("yyyy-dd-MM"));
                        command.Parameters.AddWithValue("@Email", objCliente.Email);
                        command.ExecuteNonQuery(); 
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al Actualizar Cliente");
            }



        }




        private async Task GuardaDirecciones(List<Direcciones> Direcciones, int CodigoCliente)
        {
            string connString = _configuration.GetConnectionString("SQLConnectionString");

            // Consulta para eliminar las direcciones existentes por CodigoCliente
            string deleteQuery = "DELETE FROM [dbo].[tbl_Direcciones] WHERE IdCliente = @CodigoCliente";

            // Consulta para insertar nuevas direcciones
            string insertQuery = "INSERT INTO [dbo].[tbl_Direcciones] (Direccion, Clase, IdCliente) VALUES (@Direcciono, @Clase, @CodigoCliente)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    // Abrir la conexión
                    await connection.OpenAsync();

                    // 1. Eliminar las direcciones existentes
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
                        await deleteCommand.ExecuteNonQueryAsync(); // Ejecutar eliminación
                    }

                    // 2. Insertar las nuevas direcciones
                    foreach (var direccion in Direcciones)
                    {
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            // Asignar valores a los parámetros
                            insertCommand.Parameters.AddWithValue("@Direcciono", direccion.Direccion);
                            insertCommand.Parameters.AddWithValue("@Clase", direccion.Clase);
                            insertCommand.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);

                            await insertCommand.ExecuteNonQueryAsync(); // Ejecutar inserción
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al procesar las direcciones: " + ex.Message, ex);
            }
        }


        private async Task GuardaTelefonos(List<Telefonos> Telefonos, int CodigoCliente)
        {
            string connString = _configuration.GetConnectionString("SQLConnectionString");

            // Consulta para eliminar los teléfonos existentes por CódigoCliente
            string deleteQuery = "DELETE FROM [tbl_Telefonos] WHERE IdCliente = @IdCliente";

            // Consulta para insertar los nuevos teléfonos
            string insertQuery = "INSERT INTO [tbl_Telefonos] (Telefono, Clase, IdCliente) VALUES (@Telefono, @Clase, @IdCliente)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    // Abrir la conexión
                    await connection.OpenAsync();

                    // 1. Eliminar los teléfonos existentes
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@IdCliente", CodigoCliente);
                        await deleteCommand.ExecuteNonQueryAsync(); // Ejecutar la eliminación
                    }

                    // 2. Insertar los nuevos teléfonos
                    foreach (var tel in Telefonos)
                    {
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            // Asignar valores a los parámetros
                            insertCommand.Parameters.AddWithValue("@Telefono", tel.Telefono);
                            insertCommand.Parameters.AddWithValue("@Clase", tel.Clase);
                            insertCommand.Parameters.AddWithValue("@IdCliente", CodigoCliente);

                            await insertCommand.ExecuteNonQueryAsync(); // Ejecutar la inserción
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al guardar los teléfonos: " + ex.Message, ex);
            }
        }







        public async Task EliminarCliente(int CodigoCliente)
        {
            string connString = _configuration.GetConnectionString("SQLConnectionString");

            // Consulta para eliminar los teléfonos existentes por CódigoCliente
            string deletetelefronoQuery = "DELETE FROM [tbl_Telefonos] WHERE IdCliente = @IdCliente";

            string deletedireccionoQuery = "DELETE FROM [dbo].[tbl_Direcciones] WHERE IdCliente = @IdCliente";
            // Consulta para insertar los nuevos teléfonos
            string DeleteClientequery = "DELETE FROM [dbo].[tbl_Clientes] WHERE Id = @IdCliente";

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand deleteCommand = new SqlCommand(DeleteClientequery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@IdCliente", CodigoCliente);
                        await deleteCommand.ExecuteNonQueryAsync(); 
                    }

                    using (SqlCommand deleteCommand = new SqlCommand(deletetelefronoQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@IdCliente", CodigoCliente);
                        await deleteCommand.ExecuteNonQueryAsync();
                    }

                    using (SqlCommand deleteCommand = new SqlCommand(deletedireccionoQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@IdCliente", CodigoCliente);
                        await deleteCommand.ExecuteNonQueryAsync(); 
                    }
                 
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al aliminar cliente: " + ex.Message, ex);
            }
        }


        public async Task<List<ClienteDto>> ReporteListaClientesXNombre(string Nombre)
        {

            List<ClienteDto> ListaClientes = new List<ClienteDto>();
            DataTable dtInformacion = ConsultaListaClientesxNombre(Nombre);

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    ClienteDto objForm = new ClienteDto();
                    objForm.Codigo = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.IdTipoDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["IdTipoDocumento"]);
                    objForm.TipoDocumento = dtInformacion.Rows[rows]["TipoDocumento"].ToString();
                    objForm.NumeroDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["NumeroDocumento"]);
                    objForm.Nombres = dtInformacion.Rows[rows]["Nombres"].ToString();
                    objForm.Apellido1 = dtInformacion.Rows[rows]["Apellido1"].ToString();
                    objForm.Apellido2 = dtInformacion.Rows[rows]["Apellido2"].ToString();
                    objForm.Genero = dtInformacion.Rows[rows]["Genero"].ToString();
                    // Obtén la fecha como string desde el DataTable
                    string fechaOriginal = dtInformacion.Rows[rows]["FechaNacimiento"].ToString();
                    // Convierte la fecha a un objeto DateTime
                    DateTime fechaConvertida = DateTime.Parse(fechaOriginal);
                    // Formatea la fecha al formato "yyyy-MM-dd"
                    objForm.FechaNacimiento = fechaConvertida.ToString("yyyy-MM-dd");

                    objForm.Email = dtInformacion.Rows[rows]["Email"].ToString();

                    objForm.direcciones = await ListaDireccionesxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    objForm.telefonos = await ListaTelefornosxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }
        private DataTable ConsultaListaClientesxNombre(string Nombre)
        {

            DataTable resultado = new DataTable();

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
              new SqlParameter() { ParameterName = "@Nombre", SqlDbType = SqlDbType.VarChar, Value = Nombre },
            };
                resultado = cDataBase.EjecutarSPParametrosReturnDatatable("[dbo].[BuscarClientesPorNombre]", parametros);

            return resultado;

        }



        public async Task<List<ClienteDto>> ReporteListaClientesXNumeroDocumento(string NumeroDocumento)
        {

            List<ClienteDto> ListaClientes = new List<ClienteDto>();
            DataTable dtInformacion = ConsultaListaClientesxNumeroDocumento(NumeroDocumento);

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    ClienteDto objForm = new ClienteDto();
                    objForm.Codigo = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.IdTipoDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["IdTipoDocumento"]);
                    objForm.TipoDocumento = dtInformacion.Rows[rows]["TipoDocumento"].ToString();
                    objForm.NumeroDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["NumeroDocumento"]);
                    objForm.Nombres = dtInformacion.Rows[rows]["Nombres"].ToString();
                    objForm.Apellido1 = dtInformacion.Rows[rows]["Apellido1"].ToString();
                    objForm.Apellido2 = dtInformacion.Rows[rows]["Apellido2"].ToString();
                    objForm.Genero = dtInformacion.Rows[rows]["Genero"].ToString();
                    // Obtén la fecha como string desde el DataTable
                    string fechaOriginal = dtInformacion.Rows[rows]["FechaNacimiento"].ToString();
                    // Convierte la fecha a un objeto DateTime
                    DateTime fechaConvertida = DateTime.Parse(fechaOriginal);
                    // Formatea la fecha al formato "yyyy-MM-dd"
                    objForm.FechaNacimiento = fechaConvertida.ToString("yyyy-MM-dd");

                    objForm.Email = dtInformacion.Rows[rows]["Email"].ToString();

                    objForm.direcciones = await ListaDireccionesxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    objForm.telefonos = await ListaTelefornosxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }



        private DataTable ConsultaListaClientesxNumeroDocumento(string NumeroDocumento)
        {

            DataTable resultado = new DataTable();

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
              new SqlParameter() { ParameterName = "@NumeroDocumento", SqlDbType = SqlDbType.VarChar, Value = NumeroDocumento },
            };
            resultado = cDataBase.EjecutarSPParametrosReturnDatatable("[dbo].[BuscarClientesPorNumeroDocumento]", parametros);

            return resultado;

        }





        public async Task<List<ClienteDto>> ReporteListaClientesXFechas(string fechade, string fechahasta)
        {

            List<ClienteDto> ListaClientes = new List<ClienteDto>();
            DataTable dtInformacion = ConsultaListaClientesxfechas(fechade, fechahasta);

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    ClienteDto objForm = new ClienteDto();
                    objForm.Codigo = Convert.ToInt32(dtInformacion.Rows[rows]["Id"]);
                    objForm.IdTipoDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["IdTipoDocumento"]);
                    objForm.TipoDocumento = dtInformacion.Rows[rows]["TipoDocumento"].ToString();
                    objForm.NumeroDocumento = Convert.ToInt32(dtInformacion.Rows[rows]["NumeroDocumento"]);
                    objForm.Nombres = dtInformacion.Rows[rows]["Nombres"].ToString();
                    objForm.Apellido1 = dtInformacion.Rows[rows]["Apellido1"].ToString();
                    objForm.Apellido2 = dtInformacion.Rows[rows]["Apellido2"].ToString();
                    objForm.Genero = dtInformacion.Rows[rows]["Genero"].ToString();
                    // Obtén la fecha como string desde el DataTable
                    string fechaOriginal = dtInformacion.Rows[rows]["FechaNacimiento"].ToString();
                    // Convierte la fecha a un objeto DateTime
                    DateTime fechaConvertida = DateTime.Parse(fechaOriginal);
                    // Formatea la fecha al formato "yyyy-MM-dd"
                    objForm.FechaNacimiento = fechaConvertida.ToString("yyyy-MM-dd");

                    objForm.Email = dtInformacion.Rows[rows]["Email"].ToString();

                    objForm.direcciones = await ListaDireccionesxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    objForm.telefonos = await ListaTelefornosxCliente(Convert.ToInt32(dtInformacion.Rows[rows]["Id"]));
                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }
        private DataTable ConsultaListaClientesxfechas(string fechade, string fechahasta)
        {

            DataTable resultado = new DataTable();

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
              new SqlParameter() { ParameterName = "@FechaDesde", SqlDbType = SqlDbType.VarChar, Value = fechade },
               new SqlParameter() { ParameterName = "@FechaHasta ", SqlDbType = SqlDbType.VarChar, Value = fechahasta },
            };
            resultado = cDataBase.EjecutarSPParametrosReturnDatatable("[dbo].[BuscarClientesPorRangoFecha]", parametros);

            return resultado;

        }




        public async Task<List<CantidadesDTO>> Reportemas1telefono()
        {

            List<CantidadesDTO> ListaClientes = new List<CantidadesDTO>();
            DataTable dtInformacion = Consultamas1telefono();

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    CantidadesDTO objForm = new CantidadesDTO();
                    objForm.NombreCliente = dtInformacion.Rows[rows]["NombreCompleto"].ToString(); 
                    objForm.cantidad = Convert.ToInt32(dtInformacion.Rows[rows]["CantidadTelefonos"]);
                 
                 
                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }
        private DataTable Consultamas1telefono()
        {

            string Consulta = string.Empty;
            Consulta = string.Format("SELECT \r\n    c.Nombres + ' ' + c.Apellido1 + ' ' + ISNULL(c.Apellido2, '') AS NombreCompleto,\r\n    COUNT(t.Id) AS CantidadTelefonos\r\nFROM \r\n    tbl_clientes AS c\r\nINNER JOIN \r\n    tbl_telefonos AS t\r\n    ON c.Id = t.IdCliente\r\nGROUP BY \r\n    c.IdTipoDocumento,\r\n    c.NumeroDocumento, \r\n    c.Nombres,\r\n    c.Apellido1,\r\n    c.Apellido2\r\nHAVING \r\n    COUNT(t.Id) > 1\r\nORDER BY \r\n    CantidadTelefonos ");
            DataTable dtInformacion = new DataTable();
            try
            {
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


        public async Task<List<CantidadesDTO>> Reportemas1Direcciones()
        {

            List<CantidadesDTO> ListaClientes = new List<CantidadesDTO>();
            DataTable dtInformacion = Consultamas1Direcciones();

            if (dtInformacion.Rows.Count > 0)
            {
                for (int rows = 0; rows < dtInformacion.Rows.Count; rows++)
                {

                    CantidadesDTO objForm = new CantidadesDTO();
                    objForm.NombreCliente = dtInformacion.Rows[rows]["NombreCompleto"].ToString();
                    objForm.cantidad = Convert.ToInt32(dtInformacion.Rows[rows]["CantidadDirecciones"]);


                    ListaClientes.Add(objForm);
                }
            }

            return ListaClientes;
        }
        private DataTable Consultamas1Direcciones()
        {

            string Consulta = string.Empty;
            Consulta = string.Format("SELECT \r\n    c.Nombres + ' ' + c.Apellido1 + ' ' + ISNULL(c.Apellido2, '') AS NombreCompleto,\r\n    COUNT(t.Id) AS CantidadDirecciones\r\nFROM \r\n    tbl_clientes AS c\r\nINNER JOIN \r\n    tbl_Direcciones AS t\r\n    ON c.Id = t.IdCliente\r\nGROUP BY \r\n    c.IdTipoDocumento,\r\n    c.NumeroDocumento,\r\n    c.Nombres,\r\n    c.Apellido1,\r\n    c.Apellido2\r\nHAVING \r\n    COUNT(t.Id) > 1\r\nORDER BY \r\n    CantidadDirecciones  ");
            DataTable dtInformacion = new DataTable();
            try
            {
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
