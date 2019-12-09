using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class TareaPorAreaPorRequerimientoMap : BaseEntityMap<TareaPorAreaPorRequerimiento>
    {
        public TareaPorAreaPorRequerimientoMap()
        {
            //Tabla
            Table("TareaPorAreaPorRequerimiento");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Tarea
            References(x => x.Tarea, "IdTareaPorArea").Not.Nullable();
        }
    }
}
