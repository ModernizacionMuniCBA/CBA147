using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_NotaMovil", Namespace = "http://example.com/schemas/Comando_Nota")]
    public class Comando_Movil_Nota
    {
        public int? Id { get; set; }
        public int IdMovil { get; set; }
        public string Contenido { get; set; }
        public Comando_Movil_Nota()
        {

        }
    }
}
