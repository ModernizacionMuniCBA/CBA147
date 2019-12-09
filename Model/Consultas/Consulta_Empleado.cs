using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Empleado
    {
        public int? IdArea { get; set; }
        public bool? DadosDeBaja { get; set; }
        public int? IdUsuario { get; set; }
        public List<Enums.EstadoEmpleado> Estados { get; set; }
        public int? IdOT { get; set; }
        public bool? Seccion { get; set; }
        public bool? Flota { get; set; }

        public Consulta_Empleado()
        {
            DadosDeBaja = null;
        }
    }
}
