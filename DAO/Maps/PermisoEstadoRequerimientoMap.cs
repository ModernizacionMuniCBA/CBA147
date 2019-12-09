using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class PermisoEstadoRequerimientoMap : BaseEntityMap<PermisoEstadoRequerimiento>
    {
        public PermisoEstadoRequerimientoMap()
        {
            //Tabla
            Table("PermisoEstadoRequerimiento");
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.PermisoEstadoRequerimiento));
            Map(x => x.Nombre, "Nombre");
            Map(x => x.Posicion, "Posicion");
        }
    }
}
