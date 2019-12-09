using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_Nota", Namespace = "http://example.com/schemas/Comando_Nota")]
    public class Comando_NotaOrdenTrabajo
    {
        public int? Id{get;set;}
        public string Observaciones { get; set; }
        public Comando_NotaOrdenTrabajo()
        {

        }
    }
}
