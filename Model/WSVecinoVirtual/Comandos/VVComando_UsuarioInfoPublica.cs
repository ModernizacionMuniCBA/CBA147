using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_UsuarioInfoPublica
    {
        public string Username { get; set; }
        public string Key { get; set; }
    }
}