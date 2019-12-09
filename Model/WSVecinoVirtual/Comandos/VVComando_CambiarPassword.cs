using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
        [Serializable]
    public class VVComando_CambiarPassword
    {
        public int Id { get; set; }
        public string PasswordAnterior { get; set; }
        public string PasswordNueva { get; set; }
        public string Key { get; set; }
    }
}