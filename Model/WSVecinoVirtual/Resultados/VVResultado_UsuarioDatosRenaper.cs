using System;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_UsuarioDatosRenaper
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Dni { get; set; }
        public string Cuil { get; set; }
        public bool SexoMasculino { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string DomicilioLegal { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
    }
}
