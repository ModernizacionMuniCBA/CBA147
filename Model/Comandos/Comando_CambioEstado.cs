using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_CambioEstado
    {
        public int Id { get; set; }
        public int EstadoKeyValue { get; set; }
        public string Observaciones { get; set; }

        public Comando_CambioEstado()
        {

        }
    }
}
