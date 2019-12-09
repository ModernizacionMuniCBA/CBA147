using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class TareaPorAreaPorRequerimiento : BaseEntity
    {
    
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual TareaPorArea Tarea { get; set; }

    

    }
}
