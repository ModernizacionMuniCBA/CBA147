using System;
using System.Linq;
using FluentNHibernate.Mapping;
using Model;

namespace DAO.Maps
{
    class BaseEntityMap<Entity> : ClassMap<Entity> where Entity : BaseEntity
    {
        public BaseEntityMap()
        {
            //Id
            Id(x => x.Id, "Id").GeneratedBy.Identity();

            //Fecha Alta
            Map(x => x.FechaAlta, "FechaAlta").Nullable();

            //Fecha Baja
            Map(x => x.FechaBaja, "FechaBaja").Nullable();

            //Fecha Modificacion
            Map(x => x.FechaModificacion, "FechaModificacion").Nullable();

            //Usuario
            //References(x => x.Usuario, "IdUsuario").Nullable();

            //Cerrojo Usuario
            References(x => x.Usuario, "IdUsuarioCerrojo").Nullable();

            //Observaciones
            Map(x => x.Observaciones, "Observaciones").Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)");
        }
    }
}
