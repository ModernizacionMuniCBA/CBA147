using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoEmpleadoMap : BaseEntityMap<EstadoEmpleado>
    {
        public EstadoEmpleadoMap()
        {
            //Tabla
            Table("EstadoEmpleado");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoEmpleado)).Not.Nullable();

            //Color
            Map(x => x.Color, "Color").Not.Nullable();
        }
    }
}
