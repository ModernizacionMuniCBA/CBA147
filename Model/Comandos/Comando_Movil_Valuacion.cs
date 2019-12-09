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
    public class Comando_Movil_Valuacion
    {
        public virtual int IdMovil { get; set; }
        public virtual int Valuacion { get; set; }
        public virtual DateTime? FechaValuacion { get; set; }
        public virtual string Observaciones { get; set; }

        public Comando_Movil_Valuacion()
        {
       
        }

    }
}
