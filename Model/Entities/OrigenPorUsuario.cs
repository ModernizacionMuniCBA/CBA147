using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class OrigenPorUsuario : BaseEntity
    {
        //Test
        public virtual Origen Origen{ get; set; }
        public virtual _VecinoVirtualUsuario UsuarioOrigen { get; set; }
    }
}
