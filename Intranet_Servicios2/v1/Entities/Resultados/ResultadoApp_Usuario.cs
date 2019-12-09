using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
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


        public ResultadoApp_Usuario()
        {

        }

        public ResultadoApp_Usuario(_VecinoVirtualUsuario entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            Apellido = entity.Apellido;
            SexoMasculino = entity.SexoMasculino;
            Dni = entity.Dni;
            Cuil = entity.Cuil;
            FechaNacimiento = entity.FechaNacimiento;
            DomicilioLegal = entity.DomicilioLegal;

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

            IdentificadorFotoPersonal = entity.IdentificadorFotoPersonal;

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
            ValidadoRenaper = entity.FechaValidacionRenaper != null;
            ValidadoPersonal = entity.FechaValidacionPersonal != null;
            ValidadoDomicilio = entity.FechaValidacionDomicilio != null;
        }

        public static List<_Resultado_VecinoVirtualUsuario> ToList(List<_VecinoVirtualUsuario> list)
        {
            return list.Select(x => new _Resultado_VecinoVirtualUsuario(x)).ToList();
        }
    }
}
