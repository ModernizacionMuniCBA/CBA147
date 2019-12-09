using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ServicioMap : BaseEntityMap<Servicio>
    {
        public ServicioMap()
        {
            //Tabla
            Table("Servicio");

            //Nombre
            Map(x => x.Nombre, "Nombre");
            Map(x => x.Icono, "Icono");
            Map(x => x.UrlIcono, "UrlIcono");
            Map(x => x.Color, "Color");
            Map(x => x.Principal, "Principal");

            //Temas (one to many)
            HasMany(x => x.Temas).Table("Tema").KeyColumn("IdServicio").Cascade.All();
        }
    }
}
