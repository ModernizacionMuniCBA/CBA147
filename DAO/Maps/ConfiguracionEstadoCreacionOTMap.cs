using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class ConfiguracionEstadoCreacionOTMap : BaseEntityMap<ConfiguracionEstadoCreacionOT>
    {
        public ConfiguracionEstadoCreacionOTMap()
        {
            Table("ConfiguracionEstadoCreacionOTPorArea");

            References(x => x.Area, "IdArea");

            References(x => x.EstadoCreacionOT, "IdEstadoCreacionOT");
        }
    }
}