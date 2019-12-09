using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_EstadisticaZona
    {

        public List<Enums.EstadoRequerimiento> EstadosKeyValue { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? Mes { get; set; }
        public int? Año { get; set; }
        public int IdArea { get; set; }
        public int? IdCategoria { get; set; }
        public Consulta_EstadisticaZona()
        {

        }
    }
}
