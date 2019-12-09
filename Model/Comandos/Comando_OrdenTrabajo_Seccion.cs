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
    [XmlRoot("Comando_OrdenTrabajo_Seccion", Namespace = "http://example.com/schemas/Comando_OrdenTrabajo_Seccion")]
    public class Comando_OrdenTrabajo_Seccion
    {
        public int IdOrdenTrabajo { get; set; }
        public int IdSeccion{get;set;}

        public Comando_OrdenTrabajo_Seccion()
        {

        }


    }
}
