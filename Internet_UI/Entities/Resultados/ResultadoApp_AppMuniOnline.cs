using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    [Serializable]
    public class ResultadoApp_AppMuniOnline
    {
        public virtual string nombre { get; set; }
        public virtual string url { get; set; }
        public virtual string urlIcono { get; set; }
        public virtual string urlToken { get; set; }

    }
}
