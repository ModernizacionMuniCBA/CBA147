using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class OrdenInspeccionMap : BaseEntityMap<OrdenInspeccion>
    {

        public OrdenInspeccionMap()
        {
            //Tabla
            Table("OrdenInspeccion");

            //Numero
            Map(x => x.Numero, "Numero").Not.Nullable();
            Map(x => x.Año, "Anio").Not.Nullable();
            Map(x=>x.Marcado, "Marcado");

            //Descripcion
            Map(x => x.Descripcion, "Descripcion").Nullable();

            //TipoDispositivo
            Map(x => x.TipoDispositivo, "TipoDispositivo").CustomType(typeof(Enums.TipoDispositivo)).Not.Nullable();

            //User Agent
            Map(x => x.UserAgent, "UserAgent").Not.Nullable();

            //Usuario creador
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador").Nullable();

            //Fecha creacion
            Map(x => x.FechaCreacion, "FechaCreacion").Nullable();

            //Estados
            HasMany(x => x.Estados).Table("EstadoOrdenInspeccion")
                .KeyColumn("IdOrdenInspeccion")
                .Cascade.All();

            //Requerimientos
            HasMany(x => x.RequerimientosPorOrdenInspeccion).Table("RequerimientoPorOrdenInspeccion")
                .KeyColumn("IdOrdenInspeccion")
                //.Cascade.All()
                //le quito el cascade, porq tengo que manejar yo manualmente los datos
                ;

            ////Notas
            //HasMany(x => x.Notas).Table("NotaPorOrdenInspeccion")
            //    .KeyColumn("IdOrdenInspeccion")
            //    .Cascade.All();
        
        }
    }
}
