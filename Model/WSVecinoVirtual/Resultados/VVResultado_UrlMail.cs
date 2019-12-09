using System;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_UrlMail
    {
        public int IdCerrojo { get; set; }
        public string Dominio { get; set; }
        public string Url { get; set; }
   
    }
}
