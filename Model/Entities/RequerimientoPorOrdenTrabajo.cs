using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class RequerimientoPorOrdenTrabajo : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }


    }
}
