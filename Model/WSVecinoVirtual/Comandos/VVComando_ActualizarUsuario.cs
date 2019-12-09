using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_ActualizarUsuario
    {
        public VVComando_UsuarioActualizar Comando { get; set; }
        public string KeyEdicion { get; set; }
        public string KeyEmpleado { get; set; }
    }
}