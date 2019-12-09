using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Comandos
{
    public class ComandoApp_UsuarioNuevo
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }
        public int? IdEstadoCivil { get; set; }
        public ComandoApp_UsuarioDomicilio Domicilio { get; set; }

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

    }
}