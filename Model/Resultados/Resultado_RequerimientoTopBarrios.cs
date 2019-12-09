using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_RequerimientoTopBarrios
    {
        public int BarrioId { get; set; }
        public string BarrioNombre { get; set; }
        public int ZonaId { get; set; }
        public string ZonaNombre { get; set; }
        public int AreaId { get; set; }
        public string AreaNombre { get; set; }
        public int Cantidad { get; set; }

    }
}
