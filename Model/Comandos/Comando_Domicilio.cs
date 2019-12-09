using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Domicilio
    {
        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        
        public Comando_Domicilio()
        {

        }
    }
}
