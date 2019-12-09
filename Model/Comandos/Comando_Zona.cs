using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Zona
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public List<int> IdsBarrios { get; set; }

        public Comando_Zona()
        {

        }
    }
}
