using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class FlotaMap : BaseEntityMap<Flota>
    {
        public FlotaMap()
        {
            //Tabla
            Table("Flota");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Area
            References(x => x.Area, "IdArea").Not.Nullable();

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();
                         
            //Empleados
            HasMany(x => x.Empleados).Table("EmpleadoPorFlota").KeyColumn("IdFlota")
                                .Cascade.All();

            //Estados
            HasMany(x => x.Estados).Table("EstadoFlota").KeyColumn("IdFlota")
                .Cascade.All();

            //Ordenes de trabajo
            HasMany(x => x.OrdenesTrabajo).Table("FlotaPorOrdenTrabajo")
                .KeyColumn("IdFlota")
                .Cascade.All();
        }
    }
}
