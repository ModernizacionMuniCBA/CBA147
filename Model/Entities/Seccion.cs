using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Seccion : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual CerrojoArea Area { get; set; }
            public virtual IList<EmpleadoPorArea> Empleados{get;set;}
            public virtual List<EmpleadoPorArea> GetEmpleados()
            {
                if (Empleados == null) return null;
                return Empleados.Where(x => x.FechaBaja == null ).ToList();
            }
    }
}
