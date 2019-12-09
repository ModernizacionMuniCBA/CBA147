using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Comandos
{
    public class ComandoApp_UsuarioIniciarActivacion
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UrlRetorno { get; set; }
    }
}