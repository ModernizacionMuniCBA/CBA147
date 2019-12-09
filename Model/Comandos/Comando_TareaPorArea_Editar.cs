using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_TareaPorArea_Editar
    {
        public int IdTarea { get; set; }
        public string Valor { get; set; }

        public Comando_TareaPorArea_Editar()
        {

        }
    }
}
