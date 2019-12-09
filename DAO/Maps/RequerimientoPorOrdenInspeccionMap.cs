using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RequerimientoPorOrdenInspeccionMap : BaseEntityMap<RequerimientoPorOrdenInspeccion>
    {
        public RequerimientoPorOrdenInspeccionMap()
        {
            //Tabla
            Table("RequerimientoPorOrdenInspeccion");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Orden inspeccion
            References(x => x.OrdenInspeccion, "IdOrdenInspeccion").Not.Nullable();
        }
    }
}
