using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class FuncionPorEmpleado : BaseEntity
    {
        public virtual FuncionPorArea Funcion { get; set; }
        public virtual EmpleadoPorArea Empleado { get; set; }

        public FuncionPorEmpleado()
            : base()
        {
            
        }

    }
}
