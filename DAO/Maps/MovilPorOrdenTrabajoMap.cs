using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class MovilPorOrdenTrabajoMap : BaseEntityMap<MovilPorOrdenTrabajo>
    {
        public MovilPorOrdenTrabajoMap()
        {
            //Tabla
            Table("MovilPorOrdenTrabajo");

            //OrdenTrabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();
        }
    }
}
