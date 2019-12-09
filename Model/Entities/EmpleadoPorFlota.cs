using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class EmpleadoPorFlota : BaseEntity
    {
        public virtual EmpleadoPorArea Empleado{ get; set; }
        public virtual Flota Flota { get; set; }
        //public virtual Seccion Seccion { get; set; }

        public EmpleadoPorFlota()
            : base()
        {
            
        }

    }
}
