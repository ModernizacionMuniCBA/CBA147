using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Origen
    {
        public string Nombre { get; set; }
        public string KeyAlias { get; set; }
        public string KeySecret { get; set; }

        public bool? DadosDeBaja { get; set; }
        public Consulta_Origen()
        {

        }
    }
}
