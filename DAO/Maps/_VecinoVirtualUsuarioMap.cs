using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class _VecinoVirtualUsuarioMap : BaseEntityMap<_VecinoVirtualUsuario>
    {
        public _VecinoVirtualUsuarioMap()
        {
            //Tabla
            Table("VecinoVirtualUsuario");
            ReadOnly();

            Map(x => x.Nombre, "Nombre");
            Map(x => x.Apellido, "Apellido");
            Map(x => x.SexoMasculino, "SexoMasculino");
            Map(x => x.Dni, "Dni");
            Map(x => x.Cuil, "Cuil");
            Map(x => x.FechaNacimiento, "FechaNacimiento");
            Map(x => x.DomicilioLegal, "DomicilioLegal");

            Map(x => x.EstadoCivilId);
            Map(x => x.EstadoCivilNombre);
            Map(x => x.DomicilioDireccion);
            Map(x => x.DomicilioAltura);
            Map(x => x.DomicilioTorre);
            Map(x => x.DomicilioPiso);
            Map(x => x.DomicilioDepto);
            Map(x => x.DomicilioCodigoPostal);
            Map(x => x.DomicilioBarrioId);
            Map(x => x.DomicilioBarrioNombre);
            Map(x => x.DomicilioCiudadId);
            Map(x => x.DomicilioCiudadNombre);
            Map(x => x.DomicilioProvinciaId);
            Map(x => x.DomicilioProvinciaNombre);


            //Datos de contacto
            Map(x => x.Email, "Email");
            Map(x => x.TelefonoFijo, "TelefonoFijo");
            Map(x => x.TelefonoCelular, "TelefonoCelular");
            Map(x => x.Facebook);
            Map(x => x.Twitter);
            Map(x => x.Instagram);
            Map(x => x.LinkedIn);

            //Datos de acceso
            Map(x => x.Username, "Username");
            Map(x => x.Password, "Password");


            //Datos empleado
            Map(x => x.Empleado, "Empleado");
            Map(x => x.Funcion, "Funcion");
            Map(x => x.Cargo, "Cargo");
            Map(x => x.FechaJubilacion, "FechaJubilacion");

            //Fotos
            Map(x => x.IdentificadorFotoPersonal, "IdentificadorFotoPersonal");
            Map(x => x.IdentificadorFotoRegistroCivil, "IdentificadorFotoRegistroCivil");


            //Validaciones
            Map(x => x.FechaValidacionRenaper, "FechaValidacionRenaper");
            Map(x => x.FechaValidacionMail, "FechaValidacionMail");
            Map(x => x.FechaValidacionDomicilio, "FechaValidacionDomicilio");
            Map(x => x.FechaValidacionPersonal, "FechaValidacionPersonal");
        }
    }
}
