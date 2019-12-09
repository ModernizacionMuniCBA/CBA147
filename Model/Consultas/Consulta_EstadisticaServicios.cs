using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_EstadisticaServicios
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? Mes { get; set; }
        public int? Año { get; set; }
      
        public Consulta_EstadisticaServicios()
        {

        }
    }
}
