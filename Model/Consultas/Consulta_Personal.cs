using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Personal
    {
        public int? idArea { get; set; }
        public int? idPersonaFisica { get; set; }
        public int? idFuncion { get; set; }
        public bool? dadosDeBaja { get; set; }

        public Consulta_Personal()
        {

        }
    }
}
