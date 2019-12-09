using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_UsuarioCambiarFoto
    {
        public int Id { get; set; }
        public string Base64 { get; set; }
        public string Key { get; set; }
    }
}