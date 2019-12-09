using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_BarrioPorZona
    {
        public List<int> IdsBarrio { get; set; }
        public List<int> IdsZona { get; set; }
        public List<int> IdsArea { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Consulta_BarrioPorZona()
        {

        }
    }
}
