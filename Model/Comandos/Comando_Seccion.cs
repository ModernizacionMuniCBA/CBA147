using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Seccion
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public int? IdArea { get; set; }
        public string Observaciones { get; set; }
        public List<int> IdsEmpleados { get; set; }
        public Comando_Seccion()
        {

        }
    }
}
