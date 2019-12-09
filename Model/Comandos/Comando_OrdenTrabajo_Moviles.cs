using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    [XmlRoot("Comando_OrdenTrabajo_Moviles", Namespace = "http://example.com/schemas/Comando_OrdenTrabajo_Moviles")]
    public class Comando_OrdenTrabajo_Moviles
    {
        public int IdOrdenTrabajo { get; set; }
        public List<int> IdMoviles { get; set; }

        public Comando_OrdenTrabajo_Moviles()
        {

        }
    }
}
