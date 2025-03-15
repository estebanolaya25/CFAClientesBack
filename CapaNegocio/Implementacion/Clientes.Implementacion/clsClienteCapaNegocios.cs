using CapaDatos.Interza.auth.Interfaz;
using CapaDatos.Interza.Cliente.Interfaze;
using CapaDatos.Interza.Listas.Interfaz;
using CapaDTO.Peticiones;
using CapaDTO.Respuestas;
using CapaNegocio.Interfaz.Clientes.Interfaze;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementacion.Clientes.Implementacion
{
    public class clsClienteCapaNegocios : IClienteCapaNegocios
    {
        private readonly IConfiguration _configuration;

        private readonly IClienteCapaDatos _Cliente;


        public clsClienteCapaNegocios(IConfiguration configuration, IClienteCapaDatos Cliente)
        {
            this._Cliente = Cliente;
            _configuration = configuration;

        }


        public async Task<bool> GuardaDatosClientes(ClienteDto objRegistro)
        {
            int diferenciaAnios = CalcularDiferenciaAnios(objRegistro.FechaNacimiento);
            if ((diferenciaAnios >= -1 && diferenciaAnios <= 7) && objRegistro.IdTipoDocumento != 3)
            {
                throw new Exception("La Edad No concuerda Con el tipo De Documento");
            }
            else if ((diferenciaAnios >= 8 && diferenciaAnios <= 17) && objRegistro.IdTipoDocumento != 2)
            {
                throw new Exception("La Edad No concuerda Con el tipo De Documento");
            }
            else if (diferenciaAnios >= 18 && objRegistro.IdTipoDocumento != 1)
            {
                throw new Exception("La Edad No concuerda Con el tipo De Documento");
            }
            else
            {
                return await _Cliente.GuardaDatosClientes(objRegistro);
            }  
        }




        private int CalcularDiferenciaAnios(string fechaNacimiento)
        {

            DateTime fechaNac = DateTime.ParseExact(fechaNacimiento, "yyyy-MM-dd", null);
            DateTime fechaActual = DateTime.Now;
            int anios = fechaActual.Year - fechaNac.Year;  
            if (fechaActual < fechaNac.AddYears(anios))
            {
                anios--;
            }

            return anios;
        }


        public async Task<List<ClienteDto>> ListaClientes()
        { 
        return await _Cliente.ListaClientes();
        
        }

        public async Task EliminarCliente(int CodigoCliente)
        {
             await _Cliente.EliminarCliente(CodigoCliente);
        }


        public async Task<MemoryStream> ReportexNombre(string Nombre)
        {
            var workbook = new XLWorkbook();
            var Formularios = workbook.Worksheets.Add("ClientesxNombres");

            await AddInformaciongenerarlSheet(Formularios, Nombre);



            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;


        }

        private async Task AddInformaciongenerarlSheet(IXLWorksheet worksheet, string Nombre)
        {
           List<ClienteDto> listaClientes = new List<ClienteDto>();
            listaClientes = await _Cliente.ReporteListaClientesXNombre(Nombre);


            worksheet.Cell(1, 1).Value = "Codigo Cliente";
            worksheet.Cell(1, 2).Value = "Nombre ";
            worksheet.Cell(1, 3).Value = "Apellido1";
            worksheet.Cell(1, 4).Value = "Apellido2";
            worksheet.Cell(1, 5).Value = "Tipo Documento";
            worksheet.Cell(1, 6).Value = "Numero Documento";
            worksheet.Cell(1, 7).Value = "Genero";
            worksheet.Cell(1, 8).Value = "Fecha Nacimiento";
            worksheet.Cell(1, 9).Value = "Email";

            for (int i = 1; i <= 9; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Blue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }



            int row = 2;
            foreach (ClienteDto obj in listaClientes)
            {
                worksheet.Cell(row, 1).Value = obj.Codigo;
                worksheet.Cell(row, 2).Value = obj.Nombres;
                worksheet.Cell(row, 3).Value = obj.Apellido1;
                worksheet.Cell(row, 4).Value = obj.Apellido2;

                worksheet.Cell(row, 5).Value = obj.TipoDocumento;
                worksheet.Cell(row, 6).Value = obj.NumeroDocumento;
                worksheet.Cell(row, 7).Value = obj.Genero;

                worksheet.Cell(row, 8).Value = obj.FechaNacimiento;
                worksheet.Cell(row, 9).Value = obj.Email;

                row++;
            }


        }




        public async Task<MemoryStream> ReportexNumeroDocumento(string NumeroDocumento)
        {
            var workbook = new XLWorkbook();
            var Formularios = workbook.Worksheets.Add("ClientesxNumeroDocumento");

            await AddInformacionxNumeroDocumentolSheet(Formularios, NumeroDocumento);



            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;


        }

        private async Task AddInformacionxNumeroDocumentolSheet(IXLWorksheet worksheet, string NumeroDocumento)
        {
            List<ClienteDto> listaClientes = new List<ClienteDto>();
            listaClientes = await _Cliente.ReporteListaClientesXNumeroDocumento(NumeroDocumento);


            worksheet.Cell(1, 1).Value = "Codigo Cliente";
            worksheet.Cell(1, 2).Value = "Nombre ";
            worksheet.Cell(1, 3).Value = "Apellido1";
            worksheet.Cell(1, 4).Value = "Apellido2";
            worksheet.Cell(1, 5).Value = "Tipo Documento";
            worksheet.Cell(1, 6).Value = "Numero Documento";
            worksheet.Cell(1, 7).Value = "Genero";
            worksheet.Cell(1, 8).Value = "Fecha Nacimiento";
            worksheet.Cell(1, 9).Value = "Email";

            for (int i = 1; i <= 9; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Blue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }



            int row = 2;
            foreach (ClienteDto obj in listaClientes)
            {
                worksheet.Cell(row, 1).Value = obj.Codigo;
                worksheet.Cell(row, 2).Value = obj.Nombres;
                worksheet.Cell(row, 3).Value = obj.Apellido1;
                worksheet.Cell(row, 4).Value = obj.Apellido2;

                worksheet.Cell(row, 5).Value = obj.TipoDocumento;
                worksheet.Cell(row, 6).Value = obj.NumeroDocumento;
                worksheet.Cell(row, 7).Value = obj.Genero;

                worksheet.Cell(row, 8).Value = obj.FechaNacimiento;
                worksheet.Cell(row, 9).Value = obj.Email;

                row++;
            }


        }





        public async Task<MemoryStream> ReportexFechas(string fechade, string fechahasta)
        {
            var workbook = new XLWorkbook();
            var Formularios = workbook.Worksheets.Add("ClientesxNumeroDocumento");

            await AddInformacionReportexFechaslSheet(Formularios, fechade, fechahasta);



            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;


        }

        private async Task AddInformacionReportexFechaslSheet(IXLWorksheet worksheet, string fechade, string fechahasta)
        {
            List<ClienteDto> listaClientes = new List<ClienteDto>();
            listaClientes = await _Cliente.ReporteListaClientesXFechas(fechade,fechahasta);


            worksheet.Cell(1, 1).Value = "Codigo Cliente";
            worksheet.Cell(1, 2).Value = "Nombre ";
            worksheet.Cell(1, 3).Value = "Apellido1";
            worksheet.Cell(1, 4).Value = "Apellido2";
            worksheet.Cell(1, 5).Value = "Tipo Documento";
            worksheet.Cell(1, 6).Value = "Numero Documento";
            worksheet.Cell(1, 7).Value = "Genero";
            worksheet.Cell(1, 8).Value = "Fecha Nacimiento";
            worksheet.Cell(1, 9).Value = "Email";

            for (int i = 1; i <= 9; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Blue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }



            int row = 2;
            foreach (ClienteDto obj in listaClientes)
            {
                worksheet.Cell(row, 1).Value = obj.Codigo;
                worksheet.Cell(row, 2).Value = obj.Nombres;
                worksheet.Cell(row, 3).Value = obj.Apellido1;
                worksheet.Cell(row, 4).Value = obj.Apellido2;

                worksheet.Cell(row, 5).Value = obj.TipoDocumento;
                worksheet.Cell(row, 6).Value = obj.NumeroDocumento;
                worksheet.Cell(row, 7).Value = obj.Genero;

                worksheet.Cell(row, 8).Value = obj.FechaNacimiento;
                worksheet.Cell(row, 9).Value = obj.Email;

                row++;
            }


        }



        public async Task<MemoryStream> Reportemas1telefono()
        {
            var workbook = new XLWorkbook();
            var Formularios = workbook.Worksheets.Add("ClientesxNumeroDocumento");

            await AddInformacionReportexFechaslSheet(Formularios);



            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;


        }

        private async Task AddInformacionReportexFechaslSheet(IXLWorksheet worksheet)
        {
            List<CantidadesDTO> listaClientes = new List<CantidadesDTO>();
            listaClientes = await _Cliente.Reportemas1telefono();


            worksheet.Cell(1, 1).Value = "Nombre Cliente";
            worksheet.Cell(1, 2).Value = "Cantidad Telefonos ";
           

            for (int i = 1; i <= 2; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Blue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }



            int row = 2;
            foreach (CantidadesDTO obj in listaClientes)
            {
                worksheet.Cell(row, 1).Value = obj.NombreCliente;
                worksheet.Cell(row, 2).Value = obj.cantidad;
               
                row++;
            }


        }



        public async Task<MemoryStream> Reportemas1Direccioneso()
        {
            var workbook = new XLWorkbook();
            var Formularios = workbook.Worksheets.Add("ClientesxNumeroDocumento");

            await AddInformacionDireccioneslSheet(Formularios);



            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;


        }

        private async Task AddInformacionDireccioneslSheet(IXLWorksheet worksheet)
        {
            List<CantidadesDTO> listaClientes = new List<CantidadesDTO>();
            listaClientes = await _Cliente.Reportemas1Direcciones();


            worksheet.Cell(1, 1).Value = "Nombre Cliente";
            worksheet.Cell(1, 2).Value = "Cantidad Direcciones ";


            for (int i = 1; i <= 2; i++)
            {
                var cell = worksheet.Cell(1, i);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Blue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }



            int row = 2;
            foreach (CantidadesDTO obj in listaClientes)
            {
                worksheet.Cell(row, 1).Value = obj.NombreCliente;
                worksheet.Cell(row, 2).Value = obj.cantidad;

                row++;
            }


        }




    }
}
