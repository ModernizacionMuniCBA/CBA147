using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class NotaPorRequerimiento : BaseEntity
    {
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual OrdenInspeccion OrdenInspeccion { get; set; }
        public virtual Requerimiento Requerimiento { get; set; }

    }
}
