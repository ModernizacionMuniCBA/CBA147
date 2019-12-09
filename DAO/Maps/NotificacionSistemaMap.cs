using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class NotificacionSistemaMap : BaseEntityMap<NotificacionSistema>
    {
        public NotificacionSistemaMap()
        {
            //Tabla
            Table("NotificacionSistema");

            //Titulo
            Map(x => x.Titulo, "Titulo").Not.Nullable();

            //Contenido
            Map(x => x.Contenido, "Contenido").Not.Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)"); ;

            //Notificar
            Map(x => x.Notificar, "Notificar");
        }
    }
}
