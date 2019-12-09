using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_ProcesarRecuperacionCuenta
    {
        public string Codigo { get; set; }
        public string Password{ get; set; }
    }
}