using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class TipoRequerimiento : BaseEntity 
    {
        public virtual string Nombre { get; set; }
        public virtual Enums.TipoRequerimiento KeyValue { get; set; }

    }
}
