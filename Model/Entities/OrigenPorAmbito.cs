using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class OrigenPorAmbito : BaseEntity
    {
        public virtual Origen Origen { get; set; }
        public virtual CerrojoAmbito Ambito { get; set; }
    }
}
