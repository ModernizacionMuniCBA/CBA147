using System;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_CambioClave
    {
        public int IdUser { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Url { get; set; }
        public bool Completado { get; set; }
      
    }
}
