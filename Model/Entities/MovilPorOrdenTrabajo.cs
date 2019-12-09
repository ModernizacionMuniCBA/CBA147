using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class MovilPorOrdenTrabajo : BaseEntity
    {
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual Movil Movil { get; set; }


    }
}
