using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class RequerimientoPorOrdenInspeccion : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual OrdenInspeccion OrdenInspeccion { get; set; }


    }
}
