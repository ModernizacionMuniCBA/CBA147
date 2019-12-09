using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class NotaPorMovilMap : BaseEntityMap<NotaPorMovil>
    {
        public NotaPorMovilMap()
        {
            //Tabla
            Table("NotaPorMovil");

            //OrdenTrabajo
            References(x => x.Movil, "IdMovil").Not.Nullable();
            Map(x => x.Visto, "Visto");
            References(x => x.UsuarioVisto, "IdUsuarioCerrojoVisto");
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador");
        }
    }
}
