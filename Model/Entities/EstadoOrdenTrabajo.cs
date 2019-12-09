using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class EstadoOrdenTrabajo : BaseEntity
    {
        public virtual string Nombre { get; set; }

        public virtual string Color { get; set; }

        public virtual Enums.EstadoOrdenTrabajo KeyValue { get; set; }

    }
}
