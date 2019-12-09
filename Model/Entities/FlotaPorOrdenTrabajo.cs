using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class FlotaPorOrdenTrabajo : BaseEntity
    {
        public virtual Flota Flota{ get; set; }
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }

        public FlotaPorOrdenTrabajo()
            : base()
        {
            
        }

    }
}
