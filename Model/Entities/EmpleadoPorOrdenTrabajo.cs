using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class EmpleadoPorOrdenTrabajo : BaseEntity
    {
        public virtual EmpleadoPorArea Empleado{ get; set; }
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual Seccion Seccion { get; set; }

        public EmpleadoPorOrdenTrabajo()
            : base()
        {
            
        }

    }
}
