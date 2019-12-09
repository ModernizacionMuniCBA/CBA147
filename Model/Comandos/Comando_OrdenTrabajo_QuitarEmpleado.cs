using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenTrabajo_QuitarEmpleado
    {
        public int IdOrdenTrabajo { get; set; }
        public int IdEmpleado { get; set; }

        public Comando_OrdenTrabajo_QuitarEmpleado()
        {

        }
    }
}
