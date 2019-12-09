using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_Empleado_CambioEstado
    {
        public int IdEmpleado { get; set; }
        public Enums.EstadoEmpleado EstadoKeyValue { get; set; }
        public string Observaciones { get; set; }

        public Comando_Empleado_CambioEstado()
        {

        }
    }
}
