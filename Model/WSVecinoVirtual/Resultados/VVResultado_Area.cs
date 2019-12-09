using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Area
    {
        public int IdCerrojo { get; set; }
        public string Nombre { get; set; }
        public string NombreComun { get; set; }
        public string Codigo { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string Tipo { get; set; }
        public string Ambito { get; set; }
        public string Mesa { get; set; }

    }
}
