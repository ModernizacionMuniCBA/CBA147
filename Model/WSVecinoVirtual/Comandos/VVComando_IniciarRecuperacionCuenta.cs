using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_IniciarRecuperacionCuenta
    {
        public string Username { get; set; }
        public string UrlServidor { get; set; }
        public string UrlRetorno { get; set; }
    }
}