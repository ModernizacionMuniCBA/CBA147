using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class RequerimientoPorOrdenEspecial : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }

        public virtual OrdenAtencionCritica OrdenEspecial { get; set; }

    }
}
