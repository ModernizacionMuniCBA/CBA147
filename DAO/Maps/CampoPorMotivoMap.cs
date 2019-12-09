using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CampoPorMotivoMap : BaseEntityMap<CampoPorMotivo>
    {
        public CampoPorMotivoMap()
        {
            //Tabla
            Table("CampoPorMotivo");

            //Motivo
            References(x => x.Motivo, "IdMotivo").Not.Nullable();

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Grupo
            Map(x => x.Grupo, "Grupo");

            //Obligatorio
            Map(x => x.Obligatorio, "Obligatorio");

            //Orden
            Map(x => x.Orden, "Orden");

            //Tipo Campo
            References(x => x.Tipo, "IdTipoCampoPorMotivo");

            //Opciones
            Map(x => x.Opciones, "Opciones").Nullable();
        }
    }
}
