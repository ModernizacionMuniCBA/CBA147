using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Token
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Token { get; set; }
        public int IdUsuario { get; set; }
        public int? IdAplicacion { get; set; }
        public bool TieneVencimiento { get; set; }
    }
}
