using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class FuncionPorArea : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public FuncionPorArea()
            : base()
        {
            
        }

    }
}
