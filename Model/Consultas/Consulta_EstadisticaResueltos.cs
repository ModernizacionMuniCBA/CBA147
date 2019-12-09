using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_EstadisticaResueltos
    {
        

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? Mes { get; set; }
        public int? Año { get; set; }
        public List<Consulta_AreaConSubarea> Areas { get; set; }

        public Consulta_EstadisticaResueltos()
        {

        }
    }
}
