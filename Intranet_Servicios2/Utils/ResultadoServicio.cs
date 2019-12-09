using System;
using System.Linq;

namespace Intranet_Servicios2
{
    [Serializable]
    public class ResultadoServicio<Entity>
    {
        public Entity Return { get; set; }

        public string Error { get; set; }

        public bool Ok
        {
            get
            {
                return string.IsNullOrEmpty(Error);
            }
            set
            {
                Ok = value;
            }
        }

        public void SetError()
        {
            Error = "Error procesando la solicitud";
        }
    }
}