using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class DescripcionPorRequerimientoMap : BaseEntityMap<DescripcionPorRequerimiento>
    {
        public DescripcionPorRequerimientoMap()
        {
            //Tabla
            Table("DescripcionPorRequerimiento");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Usuario Referente
            References(x => x.UsuarioReferente, "IdUsuarioCerrojoReferente");

            //Descripcion
            Map(x => x.Descripcion, "Descripcion");
        }
    }
}
