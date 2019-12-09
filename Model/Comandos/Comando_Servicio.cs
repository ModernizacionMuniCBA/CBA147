using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Servicio
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string UrlIcono { get; set; }
        public string Color { get; set; }
        public string Observaciones { get; set; }
        public bool Principal { get; set; }

        public Comando_Servicio()
        {

        }
    }
}
