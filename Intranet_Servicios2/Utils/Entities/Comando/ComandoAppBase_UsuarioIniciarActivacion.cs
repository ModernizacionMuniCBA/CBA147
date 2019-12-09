using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.Utils.Entities.Comando
{
    public class ComandoAppBase_UsuarioIniciarActivacion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UrlRetorno { get; set; }

        public ComandoAppBase_UsuarioIniciarActivacion()
        {

        }
    }
}