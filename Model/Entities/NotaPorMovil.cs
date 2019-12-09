using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class NotaPorMovil : BaseEntity
    {
        public virtual Movil Movil { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioVisto { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
        public virtual bool Visto{get;set;}

    }
}
