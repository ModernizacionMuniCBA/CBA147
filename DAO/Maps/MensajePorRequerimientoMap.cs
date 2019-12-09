using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class MensajePorRequerimientoMap : BaseEntityMap<MensajePorRequerimiento>
    {

        public MensajePorRequerimientoMap()
        {
            //Tabla
            Table("MensajePorRequerimiento");

            //Nombre
            Map(x => x.EmailReceptor).Not.Nullable();
            Map(x => x.Texto).Not.Nullable();
            Map(x => x.Enviado);
            References(x => x.RequerimientoAsociado, "IdRequerimientoAsociado").Not.Nullable();
            References(x => x.UsuarioEmisor, "IdUsuarioCerrojoEmisor").Not.Nullable();
            References(x => x.UsuarioReceptor, "IdUsuarioCerrojoReceptor").Nullable();
        }
    }
}
