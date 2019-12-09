using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_OrdenTrabajoMail", Namespace = "http://example.com/schemas/Comando_OrdenTrabajoMail")]
    public class Comando_OrdenTrabajoMail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Comando_OrdenTrabajoMail()
        {

        }
    }
}
