using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class CampoPorMotivoPorRequerimiento : BaseEntity
    {
        public virtual string Valor { get; set; }
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual CampoPorMotivo CampoPorMotivo { get; set; }
    }
}
