using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_OrdenTrabajo
    {
        public string Numero { get; set; }
        public int? Año { get; set; }
        public List<Enums.EstadoOrdenTrabajo> EstadosKeyValue { get; set; }
        public int? IdArea { get; set; }
        public List<int> IdsArea { get; set; }

        public int? IdAmbito { get; set; }
        public int? IdZona { get; set; }
        public List<int> IdsEmpleado { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdFlota { get; set; }
        public int? Mes { get; set; }

        public int? IdSeccion { get; set; }
        public int? IdMovil { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public bool? DadosDeBaja { get; set; }

        public Consulta_OrdenTrabajo()
        {
            DadosDeBaja = false;
            EstadosKeyValue = new List<Enums.EstadoOrdenTrabajo>();
        }

    }
}
