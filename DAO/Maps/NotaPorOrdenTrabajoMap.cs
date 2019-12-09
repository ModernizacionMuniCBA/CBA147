using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class NotaPorOrdenTrabajoMap : BaseEntityMap<NotaPorOrdenTrabajo>
    {
        public NotaPorOrdenTrabajoMap()
        {
            //Tabla
            Table("NotaPorOrdenTrabajo");

            //OrdenTrabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();
        }
    }
}
