using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.v1.Entities.Comandos
{
    public class ComandoApp_UsuarioIniciarActivacion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UrlRetorno { get; set; }

        public ComandoApp_UsuarioIniciarActivacion()
        {

        }
    }
}