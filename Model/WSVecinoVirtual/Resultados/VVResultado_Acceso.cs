using System;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Acceso
    {
        public bool Alta { get; set; }
        public bool Baja { get; set; }
        public bool Consulta { get; set; }
        public bool Modificacion { get; set; }
    }
}
