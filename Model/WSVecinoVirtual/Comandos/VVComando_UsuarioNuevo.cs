using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_UsuarioNuevo
    {
        //Datos personales
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }
        public DateTime? getFechaNacimiento()
        {
            try
            {
                return DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int? IdEstadoCivil { get; set; }
        public VVComando_UsuarioActualizarDomicilio Domicilio { get; set; }

        //Datos empleado
        public bool Empleado { get; set; }
        public string Cargo { get; set; }
        public string Funcion { get; set; }
        public string FechaJubilacion { get; set; }
        public DateTime? getFechaJubilacion()
        {
            try
            {
                return DateTime.ParseExact(FechaJubilacion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        //Datos de contacto
        public string Email { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }


        //Datos de acceso
        public string Username { get; set; }
        public string Password { get; set; }


        //Fotos
        public string Base64FotoPersonal { get; set; }
    }
}