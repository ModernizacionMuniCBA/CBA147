using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_OrdenAtencionCritica
    {
       
        public List<Enums.EstadoOrdenEspecial> EstadosKeyValue { get; set; }

        public List<int> IdsArea { get; set; }
        public int? IdArea { get; set; }
        public List<int> IdsServicio { get; set; }
        public List<int> IdsMotivo { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool? DadosDeBaja { get; set; }
  
        public Consulta_OrdenAtencionCritica()
        {
            DadosDeBaja = false;
        }

    }
}
