using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class DescripcionPorRequerimiento : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioReferente { get; set; }

    }
}
