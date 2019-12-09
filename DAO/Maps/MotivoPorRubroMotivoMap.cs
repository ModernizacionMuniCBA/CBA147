using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class MotivoPorRubroMotivoMap : BaseEntityMap<MotivoPorRubroMotivo>
    {
        public MotivoPorRubroMotivoMap()
        {
            Table("MotivoPorRubroMotivo");

            References(x => x.CategoriaMotivo, "IdRubroMotivo");

            References(x => x.Motivo, "IdMotivo");
        }
    }
}