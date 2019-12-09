using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class AjustesMap : BaseEntityMap<Ajustes>
    {
        public AjustesMap()
        {
            Table("Ajustes");

            Map(x => x.App, "App");
        }
    }
}