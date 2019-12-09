using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class LimiteRequerimientosPorUsuarioMap : BaseEntityMap<LimiteRequerimientosPorUsuario>
    {
        public LimiteRequerimientosPorUsuarioMap()
        {
            //Tabla
            Table("LimiteRequerimientosPorUsuario");

            //Nombre
            Map(x => x.Contador, "Contador");
            Map(x => x.Fecha, "Fecha");
            Map(x => x.IdUsuarioCreador, "IdUsuarioCerrojoCreador");
        }
    }
}
