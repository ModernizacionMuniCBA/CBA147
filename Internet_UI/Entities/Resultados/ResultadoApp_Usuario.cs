using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    public class ResultadoApp_Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool SexoMasculino { get; set; }
        public int Dni { get; set; }
        public string Cuil { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public string DomicilioLegal { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
        
        public int? EstadoCivilId { get; set; }
        public string EstadoCivilNombre { get; set; }
        public string DomicilioDireccion { get; set; }
        public string DomicilioAltura { get; set; }
        public string DomicilioPiso { get; set; }
        public string DomicilioDepto { get; set; }
        public string DomicilioTorre { get; set; }
        public string DomicilioCodigoPostal { get; set; }
        public string DomicilioBarrioNombre { get; set; }
        public int? DomicilioBarrioId { get; set; }
        public string DomicilioCiudadNombre { get; set; }
        public int? DomicilioCiudadId { get; set; }
        public string DomicilioProvinciaNombre { get; set; }
        public int? DomicilioProvinciaId { get; set; }

        
        //Acceso
        public string Username { get; set; }

        //Contacto
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }

        //Empleado
        public bool Empleado { get; set; }
        public string Funcion { get; set; }
        public string Cargo { get; set; }
        public DateTime? FechaJubilacion { get; set; }

        //Validaciones
        public bool ValidadoEmail { get; set; }
        public bool ValidadoRenaper { get; set; }
        public bool ValidadoPersonal { get; set; }
        public bool ValidadoDomicilio { get; set; }
    }
}