using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Zona
    {
        public string Nombre { get; set; }
        public bool? DadosDeBaja { get; set; }
        public List<int> IdsArea { get; set; }

        public Consulta_Zona()
        {

        }
    }
}
