using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Objeto
    {
        public int IdCerrojo { get; set; }
        public string Titulo { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Codigo { get; set; }
        public VVResultado_Acceso Acceso { get; set; }
        public List<VVResultado_KeyValue> KeyValues { get; set; }


    }
}
