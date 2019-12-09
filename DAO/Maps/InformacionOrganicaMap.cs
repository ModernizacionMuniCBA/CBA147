using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class InformacionOrganicaMap : BaseEntityMap<InformacionOrganica>
    {
        public InformacionOrganicaMap()
        {
            //Tabla
            Table("InformacionOrganica");

            //Nombre
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();
            References(x => x.Direccion, "IdInformacionOrganicaDireccion").Not.Nullable();
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador").Not.Nullable();


        }
    }
}
