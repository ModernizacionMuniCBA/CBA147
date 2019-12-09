using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_IniciarSesion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string App { get; set; }
        public string KeyTokenSinVencimiento { get; set; }
    }
}