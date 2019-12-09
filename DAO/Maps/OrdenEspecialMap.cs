using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class OrdenEspecialMap : BaseEntityMap<OrdenAtencionCritica>
    {

        public OrdenEspecialMap()
        {
            //Tabla
            Table("OrdenEspecial");

            //Descripcion
            Map(x => x.Descripcion, "Descripcion").Nullable();

            //Usuario
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador").Nullable();

              //Estados
            HasMany(x => x.Estados).Table("EstadoOrdenEspecial")
                .KeyColumn("IdOrdenEspecial")
                .Cascade.All();

            //Requerimientos
            HasMany(x => x.RequerimientosPorOrdenEspecial).Table("RequerimientoPorOrdenEspecial")
                .KeyColumn("IdOrdenEspecial")
                //.Cascade.All()
                //le quito el cascade, porq tengo que manejar yo manualmente los datos
                ;

        }
    }
}
