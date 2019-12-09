using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_Archivo", Namespace = "http://example.com/schemas/Comando_Archivo")]
    public class Comando_Archivo
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Data { get; set; }
        public int? IdUsuarioCerrojoReferente { get; set; }

        public Comando_Archivo()
        {

        }
    }
}
