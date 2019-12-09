using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class _VecinoVirtualUsuario : BaseEntity
    {
        //Datos Personales
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual bool SexoMasculino { get; set; }
        public virtual int Dni { get; set; }
        public virtual string Cuil { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual string DomicilioLegal { get; set; }

        public virtual int? EstadoCivilId { get; set; }
        public virtual string EstadoCivilNombre { get; set; }
        public virtual string DomicilioDireccion { get; set; }
        public virtual  string DomicilioAltura { get; set; }
        public virtual string DomicilioPiso { get; set; }
        public virtual string DomicilioDepto { get; set; }
        public virtual string DomicilioTorre { get; set; }
        public virtual string DomicilioCodigoPostal { get; set; }
        public virtual string DomicilioBarrioNombre { get; set; }
        public virtual int? DomicilioBarrioId { get; set; }
        public virtual string DomicilioCiudadNombre { get; set; }
        public virtual int? DomicilioCiudadId { get; set; }
        public virtual string DomicilioProvinciaNombre { get; set; }
        public virtual int? DomicilioProvinciaId { get; set; }


        public virtual string IdentificadorFotoPersonal { get; set; }
        public virtual string IdentificadorFotoRegistroCivil { get; set; }


        //Datos de acceso
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }


        //Datos de contacto
        public virtual string TelefonoFijo { get; set; }
        public virtual string TelefonoCelular { get; set; }
        public virtual string Email { get; set; }
        public virtual string Facebook { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Instagram { get; set; }
        public virtual string LinkedIn { get; set; }


        //Datos de empleado
        public virtual bool Empleado { get; set; }
        public virtual string Funcion { get; set; }
        public virtual string Cargo { get; set; }
        public virtual DateTime? FechaJubilacion { get; set; }


        //Validaciones
        public virtual DateTime? FechaValidacionMail { get; set; }
        public virtual DateTime? FechaValidacionRenaper { get; set; }
        public virtual DateTime? FechaValidacionPersonal { get; set; }
        public virtual DateTime? FechaValidacionDomicilio { get; set; }


        public _VecinoVirtualUsuario()
        {

        }
    }
}
