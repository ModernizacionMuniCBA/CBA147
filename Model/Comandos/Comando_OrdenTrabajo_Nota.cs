using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_OrdenTrabajo_Nota", Namespace = "http://example.com/schemas/Comando_Archivo")]
    public class Comando_OrdenTrabajo_Nota
    {
        public int IdOrdenTrabajo { get; set; }
        public string Observaciones { get; set; }
        public int? IdRequerimiento { get; set; }

        public Comando_OrdenTrabajo_Nota()
        {

        }
    }
}
