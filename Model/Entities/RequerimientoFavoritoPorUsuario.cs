using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class RequerimientoFavoritoPorUsuario : BaseEntity
    {
        public virtual _VecinoVirtualUsuario User { get; set; }
        public virtual Requerimiento Requerimiento { get; set; }
    }
}
