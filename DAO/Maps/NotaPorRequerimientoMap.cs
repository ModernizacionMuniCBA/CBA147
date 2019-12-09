using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class NotaPorRequerimientoMap : BaseEntityMap<NotaPorRequerimiento>
    {
        public NotaPorRequerimientoMap()
        {
            //Tabla
            Table("NotaPorRequerimiento");

            //OrdenTrabajo
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Nullable();
            References(x => x.OrdenInspeccion, "IdOrdenInspeccion").Nullable();
        }
    }
}
