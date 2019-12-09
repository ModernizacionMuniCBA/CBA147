using System;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_UsuarioPublico
    {

        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
        public bool SexoMasculino { get; set; }
        public bool ActivadoEmail { get; set; }
    }
}
