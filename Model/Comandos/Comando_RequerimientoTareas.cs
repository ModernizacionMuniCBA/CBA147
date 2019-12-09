using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_RequerimientoTareas
    {
        public int IdRequerimiento { get; set; }
        public List<int> IdsTareas { get; set; }
        public int IdTarea { get; set; }

        public Comando_RequerimientoTareas()
        {

        }
    }
}
