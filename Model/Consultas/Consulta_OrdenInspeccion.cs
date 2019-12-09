using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_OrdenInspeccion
    {
        public string Numero { get; set; }
        public int? Año { get; set; }
        public List<Enums.EstadoOrdenInspeccion> EstadosKeyValue { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? IdArea { get; set; }
        public List<int> IdsArea { get; set; }
        public int? IdZona { get; set; }
        public bool? Marcado { get; set; }

        public bool? DadosDeBaja { get; set; }

        public Consulta_OrdenInspeccion() { }
        public Consulta_OrdenInspeccion(bool esCpc)
        {
            Marcado = esCpc;
            DadosDeBaja = false;
            EstadosKeyValue = new List<Enums.EstadoOrdenInspeccion>();
        }

    }
}
