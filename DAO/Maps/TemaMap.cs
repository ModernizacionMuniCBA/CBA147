using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class TemaMap : BaseEntityMap<Tema>
    {
        public TemaMap()
        {
            //Tabla
            Table("Tema");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //Servicio (fk)
            References(x => x.Servicio, "IdServicio").Not.Nullable();

            //Motivos (one to many)
            HasMany(x => x.Motivos).Table("Motivo").KeyColumn("IdTema").Cascade.All();
        }
    }
}
