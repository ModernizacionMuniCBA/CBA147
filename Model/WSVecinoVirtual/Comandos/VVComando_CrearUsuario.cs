using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
        [Serializable]
    public class VVComando_CrearUsuario
    {
        public VVComando_UsuarioNuevo Comando{ get; set; }
        public bool PasswordDefault { get; set; }
        public string UrlServidor { get; set; }
        public string UrlRetorno { get; set; }
        public string KeyActivacion { get; set; }
        public string KeyEmpleado { get; set; }
    }
}