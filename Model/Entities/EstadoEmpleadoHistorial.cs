using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoEmpleadoHistorial : BaseEntity
    {
        public virtual EmpleadoPorArea Empleado{ get; set; }
        public virtual EstadoEmpleado Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual bool Ultimo { get; set; }

        public EstadoEmpleadoHistorial()
        {

        }

    }
}
