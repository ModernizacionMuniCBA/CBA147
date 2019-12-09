using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EmpleadoPorAreaMap : BaseEntityMap<EmpleadoPorArea>
    {
        public EmpleadoPorAreaMap()
        {
            //Tabla
            Table("EmpleadoPorArea");

            //Usuario VV
            References(x => x.UsuarioEmpleado, "IdUsuarioCerrojoEmpleado").Not.Nullable();

            //Area 
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();

            //Funciones
            HasMany(x => x.FuncionesPorEmpleado).Table("FuncionPorEmpleado").KeyColumn("IdEmpleado")
                .Cascade.All();

            //Estados
            HasMany(x => x.Estados).Table("EstadoEmpleado").KeyColumn("IdEmpleado")
                .Cascade.All();

            //Ordenes de trabajo
            HasMany(x => x.OrdenesTrabajo).Table("EmpleadoPorOrdenTrabajo")
                .KeyColumn("IdEmpleado")
                .Cascade.All();

            //Seccion
            References(x => x.Seccion, "IdSeccion");

            //Flota
            HasMany(x => x.Flotas).Table("EmpleadoPorFlota").KeyColumn("IdEmpleado")
                .Cascade.All(); ;


            References(x => x.FlotaActiva, "IdFlotaActiva").Nullable();
        }
    }
}
