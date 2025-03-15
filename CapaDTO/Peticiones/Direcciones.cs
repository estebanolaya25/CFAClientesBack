using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO.Peticiones
{
    public class Direcciones
    {
        public int Id { get; set; }

        public string Direccion { get; set; }

        public string Clase { get; set; }
        public int IdCliente { get; set; }
    }
}
