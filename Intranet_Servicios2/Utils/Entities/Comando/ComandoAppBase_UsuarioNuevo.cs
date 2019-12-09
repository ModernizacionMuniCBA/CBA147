using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.Utils.Entities.Comando
{
    public class ComandoAppBase_UsuarioNuevo
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }
        public int? IdEstadoCivil { get; set; }
        public ComandoAppBase_UsuarioDomicilio Domicilio { get; set; }

        //Datos de acceso
        public string Username { get; set; }
        public string Password { get; set; }


        //Datos de contacto
        public string Email { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }

        public ComandoAppBase_UsuarioNuevo()
        {

        }
    }
}