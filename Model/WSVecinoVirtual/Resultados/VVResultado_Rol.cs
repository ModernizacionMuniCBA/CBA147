using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
        [Serializable]
    public class VVResultado_Rol
    {
        public int IdCerrojo { get; set; }
        public string Nombre { get; set; }
        public bool Empleado { get; set; }

    }
}
