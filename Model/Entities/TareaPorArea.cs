using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class TareaPorArea : BaseEntity
    {
        public virtual CerrojoArea Area { get; set; }
        public virtual string Nombre { get; set; }
    

    }
}
