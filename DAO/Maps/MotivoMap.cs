using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class MotivoMap : BaseEntityMap<Motivo>
    {
        public MotivoMap()
        {
            //Tabla
            Table("Motivo");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //Keywords
            Map(x => x.Keywords, "Keywords");

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();

            //Categoria
            References(x => x.Categoria, "IdCategoriaMotivoArea").Nullable();

            //Tema (fk)
            References(x => x.Tema, "IdTema").Not.Nullable();

            //Urgente
            Map(x => x.Urgente, "Urgente");

            //Interno
            Map(x => x.Tipo, "Tipo").CustomType(typeof(Enums.TipoMotivo)).Not.Nullable(); ;

            //Principal
            Map(x => x.Principal, "Principal");

            //Prioridad
            Map(x => x.Prioridad, "Prioridad").CustomType(typeof(Enums.PrioridadRequerimiento)).Not.Nullable();

            //Esfuerzo
            Map(x => x.Esfuerzo, "Esfuerzo").CustomType(typeof(Enums.EsfuerzoMotivo)).Not.Nullable();

            //Campos
            HasMany(x => x.Campos).Table("CampoPorMotivo").KeyColumn("IdMotivo")
                .Cascade.All();

            //Etiquetas
            HasMany(x => x.Etiquetas).Table("EtiquetaPorMotivo").KeyColumn("IdMotivo")
                .Cascade.All();

        }
    }
}
