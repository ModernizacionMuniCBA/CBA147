using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class PersonaFisicaMap : BaseEntityMap<PersonaFisica>
    {
        public PersonaFisicaMap()
        {
            //Tabla
            Table("PersonaFisica");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Apellido
            Map(x => x.Apellido, "Apellido").Not.Nullable();

            //Tipo Documento
            References(x => x.TipoDocumento, "IdTipoDoc").Not.Nullable();

            //Numero Documento
            Map(x => x.NroDoc, "NroDoc").Not.Nullable();

            //Fecha Nacimiento
            Map(x => x.FechaNacimiento, "FechaNacimiento").Nullable();

            //Sexo
            Map(x => x.Sexo, "Sexo").CustomType(typeof(Enums.Sexo)).Not.Nullable();

            //Cuil
            Map(x => x.Cuil, "Cuil").Nullable();

            //Mail
            Map(x => x.Mail, "Mail").Nullable();

            //Telefono
            Map(x => x.Telefono, "Telefono").Nullable();

            //Domicilio
            References(x => x.Domicilio, "IdDomicilio").Cascade.All().Nullable();
            Map(x => x.DomicilioManual, "DomicilioManual").Nullable();

        }
    }
}
