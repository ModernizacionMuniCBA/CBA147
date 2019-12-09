using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Subzona
    {
        public string Nombre { get; set; }
        public int? AreaId { get; set; }
        public int? ZonaId { get; set; }
        public bool? DadosDeBaja { get; set; }
        public Consulta_Subzona()
        {

        }
    }
}
