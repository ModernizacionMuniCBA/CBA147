using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_Flota_CambioEstado
    {
        public int IdFlota { get; set; }
        public Enums.EstadoFlota EstadoKeyValue { get; set; }
        public string Observaciones { get; set; }

        public Comando_Flota_CambioEstado()
        {

        }
    }
}
