using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class OrdenTrabajoMap : BaseEntityMap<OrdenTrabajo>
    {

        public OrdenTrabajoMap()
        {
            //Tabla
            Table("OrdenTrabajo");

            //Numero
            Map(x => x.Numero, "Numero").Not.Nullable();

            Map(x => x.Año, "Anio").Not.Nullable();

            //Descripcion
            Map(x => x.Descripcion, "Descripcion").Nullable();

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();
           
            //Ambito
            References(x => x.Ambito, "IdAmbito").Nullable();

            //Zona
            References(x => x.Zona, "IdZona").Nullable();

            //Sección
            References(x => x.Seccion, "IdSeccion").Nullable();

            //Estados
            HasMany(x => x.Estados).Table("EstadoOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                .Cascade.All();

            //Requerimientos
            HasMany(x => x.RequerimientosPorOrdenTrabajo).Table("RequerimientoPorOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                //.Cascade.All()
                //le quito el cascade, porq tengo que manejar yo manualmente los datos
                ;

            //Notas
            HasMany(x => x.Notas).Table("NotaPorOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                .Cascade.All()
                ;

            //Flotas
            HasMany(x => x.FlotasPorOrdenTrabajo).Table("FlotaPorOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                .Cascade.All();

            //Moviles
            HasMany(x => x.MovilesPorOrdenTrabajo).Table("MovilPorOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                .Cascade.All();

            //Empleados
            HasMany(x => x.EmpleadosPorOrdenTrabajo).Table("EmpleadoPorOrdenTrabajo")
                .KeyColumn("IdOrdenTrabajo")
                .Cascade.All();

            //User Agent
            Map(x => x.UserAgent, "UserAgent").Not.Nullable();

            //TipoDispositivo
            Map(x => x.TipoDispositivo, "TipoDispositivo").CustomType(typeof(Enums.TipoDispositivo)).Not.Nullable();

            //Fecha creacion
            Map(x => x.FechaCreacion, "FechaCreacion").Nullable();

            //Usuario creador
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador").Nullable();
        }
    }
}
