using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RecursoPorOrdenTrabajoMap : BaseEntityMap<RecursoPorOrdenTrabajo>
    {
        public RecursoPorOrdenTrabajoMap()
        {
            //Tabla
            Table("RecursoPorOrdenTrabajo");

            Map(x => x.Material, "Material");
            Map(x => x.Personal, "Personal");
            Map(x => x.Flota, "Flota");
            Map(x => x.Zona, "Zona");
            Map(x => x.Seccion, "Seccion");

            //OrdenTrabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();
        }
    }
}
