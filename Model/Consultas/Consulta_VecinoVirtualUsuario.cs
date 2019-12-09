using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_VecinoVirtualUsuario
    {
        public int? IdVecinoVirtual { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int? Dni { get; set; }
        public string Username { get; set; }
        public bool? SoloEmpleados { get; set; }

        public bool? DadosDeBaja { get; set; }
        
        public Consulta_VecinoVirtualUsuario()
        {
            DadosDeBaja = false;
        }
    }
}
