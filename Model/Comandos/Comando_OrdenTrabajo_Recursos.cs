using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenTrabajo_Recursos
    {
        public int IdOrdenTrabajo { get; set; }
        public string Personal { get; set; }
        public string Material { get; set; }
        public string Observaciones { get; set; }

        public Comando_OrdenTrabajo_Recursos()
        {

        }
    }
}
