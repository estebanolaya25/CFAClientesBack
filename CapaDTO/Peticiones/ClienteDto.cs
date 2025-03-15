using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO.Peticiones
{
   public class ClienteDto
    {
        public int Codigo { get; set; }
        public int IdTipoDocumento { get; set; }

        public string TipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellido1{ get; set; }
        public string Apellido2 { get; set; }
        public string Genero { get; set; }
        public string FechaNacimiento { get; set; }

        public List<Direcciones> direcciones { get; set; }

        public List<Telefonos> telefonos { get; set; }

        public string Email { get; set; }


    }
}
