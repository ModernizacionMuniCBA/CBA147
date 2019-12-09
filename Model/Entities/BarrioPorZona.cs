using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class BarrioPorZona : BaseEntity
    {
        public virtual Barrio Barrio { get; set; }
        public virtual Zona Zona { get; set; }
    }
}
