using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenTrabajoMap : BaseEntityMap<EstadoOrdenTrabajo>
    {
        public EstadoOrdenTrabajoMap()
        {
            //Tabla
            Table("EstadoOrdenTrabajo");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoOrdenTrabajo)).Not.Nullable();
        
            //Color
            Map(x => x.Color, "Color").Not.Nullable();
        }
    }
}
