using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Personal : BaseEntity
    {
        public virtual PersonaFisica PersonaFisica { get; set; }
        public virtual CerrojoArea Area { get; set; }

    }
}
