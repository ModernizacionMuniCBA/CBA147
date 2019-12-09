using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class NotaPorOrdenInspeccionMap : BaseEntityMap<NotaPorOrdenInspeccion>
    {
        public NotaPorOrdenInspeccionMap()
        {
            //Tabla
            Table("NotaPorOrdenInspeccion");

            //OrdenInspeccion
            References(x => x.OrdenInspeccion, "IdOrdenInspeccion").Not.Nullable();
        }
    }
}
