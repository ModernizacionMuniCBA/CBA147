using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_OrigenPorArea
    {
        public int? AreaId { get; set; }
        public int? OrigenId { get; set; }
        public bool? DadosDeBaja { get; set; }
        public Consulta_OrigenPorArea()
        {

        }
    }
}
