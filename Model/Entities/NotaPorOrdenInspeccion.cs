using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class NotaPorOrdenInspeccion : BaseEntity
    {
        public virtual OrdenInspeccion OrdenInspeccion { get; set; }

    }
}
