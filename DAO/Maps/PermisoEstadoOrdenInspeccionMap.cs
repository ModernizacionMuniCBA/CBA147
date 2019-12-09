using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class PermisoEstadoOrdenInspeccionMap : BaseEntityMap<PermisoEstadoOrdenInspeccion>
    {
        public PermisoEstadoOrdenInspeccionMap()
        {
            //Tabla
            Table("PermisoEstadoOrdenInspeccion");
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.PermisoEstadoOrdenInspeccion));
            Map(x => x.Nombre, "Nombre");
            Map(x => x.Posicion, "Posicion");
        }
    }
}
