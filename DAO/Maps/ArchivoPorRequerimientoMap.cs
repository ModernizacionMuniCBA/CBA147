using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class ArchivoPorRequerimientoMap : BaseEntityMap<ArchivoPorRequerimiento>
    {
        public ArchivoPorRequerimientoMap()
        {
            //Tabla
            Table("ArchivoPorRequerimiento");

            Map(x => x.Nombre, "Nombre").Not.Nullable().Length(200);
            Map(x => x.Identificador, "Identificador").CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(x => x.ContentType, "ContentType");
            Map(x => x.ContentLength, "ContentLength");
            Map(x => x.Tipo, "Tipo").CustomType(typeof(Enums.TipoArchivo)).Not.Nullable();

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento");

            References(x => x.UsuarioReferente, "IdUsuarioCerrojoReferente");

            //Para foto
            Map(x => x.Width, "Width");
            Map(x => x.Height, "Height");

        }
    }
}
