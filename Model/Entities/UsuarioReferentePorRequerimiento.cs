using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class UsuarioReferentePorRequerimiento : BaseEntity
    {
    
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioReferente{ get; set; }

    

    }
}
