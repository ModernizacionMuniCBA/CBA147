using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class _Resultado_VecinoVirtualUsuario : Resultado_Base<_VecinoVirtualUsuario>
    {
        //Datos Personales
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual bool SexoMasculino { get; set; }
        public virtual int Dni { get; set; }
        public virtual string Cuil { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual string DomicilioLegal { get; set; }
        public virtual string IdentificadorFotoPersonal { get; set; }
        public virtual string IdentificadorFotoRegistroCivil { get; set; }

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


        //Datos de acceso
        public virtual string Username { get; set; }
        //public virtual string Password { get; set; }


        //Datos de contacto
        public virtual string TelefonoFijo { get; set; }
        public virtual string TelefonoCelular { get; set; }
        public virtual string Email { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }


        //Datos de empleado
        public virtual bool Empleado { get; set; }
        public virtual string Funcion { get; set; }
        public virtual string Cargo { get; set; }
        public virtual DateTime? FechaJubilacion { get; set; }


        //Validaciones
        public virtual bool ValidadoEmail { get; set; }
        public virtual bool ValidadoRenaper { get; set; }
        public virtual bool ValidadoPersonal { get; set; }
        public virtual bool ValidadoDomicilio { get; set; }

        public _Resultado_VecinoVirtualUsuario()
        {

        }

        public _Resultado_VecinoVirtualUsuario(_VecinoVirtualUsuario entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            Apellido = entity.Apellido;
            SexoMasculino = entity.SexoMasculino;
            Dni = entity.Dni;
            Cuil= entity.Cuil;
            FechaNacimiento = entity.FechaNacimiento;
            DomicilioLegal = entity.DomicilioLegal;
            IdentificadorFotoPersonal = entity.IdentificadorFotoPersonal;
            IdentificadorFotoRegistroCivil = entity.IdentificadorFotoRegistroCivil;

            EstadoCivilId = entity.EstadoCivilId;
            EstadoCivilNombre = entity.EstadoCivilNombre;
            DomicilioDireccion = entity.DomicilioDireccion;
            DomicilioAltura = entity.DomicilioAltura;
            DomicilioTorre = entity.DomicilioTorre;
            DomicilioPiso = entity.DomicilioPiso;
            DomicilioDepto = entity.DomicilioDepto;
            DomicilioCodigoPostal = entity.DomicilioCodigoPostal;
            DomicilioBarrioId = entity.DomicilioBarrioId;
            DomicilioBarrioNombre = entity.DomicilioBarrioNombre;
            DomicilioCiudadId = entity.DomicilioCiudadId;
            DomicilioCiudadNombre = entity.DomicilioCiudadNombre;
            DomicilioProvinciaId = entity.DomicilioProvinciaId;
            DomicilioProvinciaNombre = entity.DomicilioProvinciaNombre;

            Username = entity.Username;

            TelefonoFijo = entity.TelefonoFijo;
            TelefonoCelular = entity.TelefonoCelular;
            Email = entity.Email;
            Facebook = entity.Facebook;
            Twitter = entity.Twitter;
            Instagram = entity.Instagram;
            LinkedIn = entity.LinkedIn;

            Empleado = entity.Empleado;
            Funcion = entity.Funcion;
            Cargo = entity.Cargo;
            FechaJubilacion = entity.FechaJubilacion;

            ValidadoEmail = entity.FechaValidacionMail != null;
            ValidadoRenaper= entity.FechaValidacionRenaper != null;
            ValidadoPersonal = entity.FechaValidacionPersonal != null;
            ValidadoDomicilio = entity.FechaValidacionDomicilio != null;
        }

        public static List<_Resultado_VecinoVirtualUsuario> ToList(List<_VecinoVirtualUsuario> list)
        {
            return list.Select(x => new _Resultado_VecinoVirtualUsuario(x)).ToList();
        }
    }
}
