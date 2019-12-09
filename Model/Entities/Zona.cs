using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class Zona : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual Color Color { get; set; }

        public virtual IList<BarrioPorZona> BarriosPorZona { get; set; }
    }
}
