using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_RequerimientoPorOrdenTrabajo
    {
        public int Id{get;set;}
        public IList<Comando_Nota> Notas { get; set; }
        public Comando_RequerimientoPorOrdenTrabajo()
        {

        }
    }
}
