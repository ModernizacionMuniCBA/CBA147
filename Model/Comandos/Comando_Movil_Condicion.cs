using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Movil_Condicion
    {
        public virtual int IdMovil { get; set; }
        public virtual Enums.CondicionMovil? Condicion { get; set; }

        public Comando_Movil_Condicion()
        {
       
        }

    }
}
