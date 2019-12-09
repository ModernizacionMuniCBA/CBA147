using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_Movil_VisarNota
    {
        public int IdNota { get; set; }
        public int IdMovil { get; set; }
        public bool Visto { get; set; }

        public Comando_Movil_VisarNota()
        {

        }
    }
}
