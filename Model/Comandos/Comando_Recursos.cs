using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_Recursos
    {

        //Material
        public virtual string Material { get; set; }

        //Flota
        public virtual string Flota { get; set; }

        //Personal
        public virtual string Personal { get; set; }

        //Observaciones
        public virtual string Observaciones { get; set; }


        public Comando_Recursos()
        {

        }
    }
}
