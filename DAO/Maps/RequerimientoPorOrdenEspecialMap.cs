using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RequerimientoPorOrdenEspecialMap : BaseEntityMap<RequerimientoPorOrdenEspecial>
    {
        public RequerimientoPorOrdenEspecialMap()
        {
            //Tabla
            Table("RequerimientoPorOrdenEspecial");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Orden Trabajo
            References(x => x.OrdenEspecial, "IdOrdenEspecial").Not.Nullable();
        }
    }
}
