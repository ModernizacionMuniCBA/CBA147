using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class SeccionMap : BaseEntityMap<Seccion>
    {
        public SeccionMap()
        {
            //Tabla
            Table("Seccion");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable().Length(100);

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();

            //Empleados
            HasMany(x => x.Empleados).Table("EmpleadoPorArea").KeyColumn("IdSeccion")
                .Cascade.All();
        }
    }
}
