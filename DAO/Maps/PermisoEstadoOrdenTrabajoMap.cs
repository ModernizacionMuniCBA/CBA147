using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class PermisoEstadoOrdenTrabajoMap : BaseEntityMap<PermisoEstadoOrdenTrabajo>
    {
        public PermisoEstadoOrdenTrabajoMap()
        {
            //Tabla
            Table("PermisoEstadoOrdenTrabajo");
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.PermisoEstadoOrdenTrabajo));
            Map(x => x.Nombre, "Nombre");
            Map(x => x.Posicion, "Posicion");
        }
    }
}
