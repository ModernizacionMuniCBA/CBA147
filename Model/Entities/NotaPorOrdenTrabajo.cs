using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class NotaPorOrdenTrabajo : BaseEntity
    {
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }

    }
}
