using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class ConfiguracionBandejaPorAreaMap : BaseEntityMap<ConfiguracionBandejaPorArea>
    {
        public ConfiguracionBandejaPorAreaMap()
        {
            Table("ConfiguracionBandejaPorArea");

            References(x => x.Area, "IdArea");

            Map(x => x.TipoMotivoPorDefecto, "IdTipoMotivo").CustomType(typeof(Enums.TipoMotivo)).Not.Nullable();

            Map(x => x.PorDefecto);
        }
    }
}