using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenInspeccion_Nota
    {
        public int IdOrdenInspeccion { get; set; }
        public string Observaciones { get; set; }
        public int? IdRequerimiento { get; set; }

        public Comando_OrdenInspeccion_Nota()
        {

        }
    }
}
