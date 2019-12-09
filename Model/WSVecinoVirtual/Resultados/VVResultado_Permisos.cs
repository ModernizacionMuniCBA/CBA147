using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Permisos
    {
        public int IdCerrojo { get; set; }
        public string Rol { get; set; }

        public List<VVResultado_KeyValue> KeyValues { get; set; }

        public List<VVResultado_Objeto> Objetos { get; set; }

        public string AmbitoTrabajo { get; set; }

   
    }
}
