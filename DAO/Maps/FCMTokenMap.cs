using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class FCMTokenMap : BaseEntityMap<FCMToken>
    {
        public FCMTokenMap()
        {
            //Tabla
            Table("FCMToken");

            //Usuario token
            References(x => x.UsuarioToken, "IdUsuarioCerrojoToken").Not.Nullable();

        }
    }
}
