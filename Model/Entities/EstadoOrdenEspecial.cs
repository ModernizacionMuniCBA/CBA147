using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class EstadoOrdenEspecial : BaseEntity
    {
        public virtual string Nombre { get; set; }

        public virtual string Color { get; set; }

        public virtual Enums.EstadoOrdenEspecial KeyValue { get; set; }

    }
}
