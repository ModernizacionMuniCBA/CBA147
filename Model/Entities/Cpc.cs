using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Cpc : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual int IdCatastro { get; set; }

        public virtual int Numero { get; set; }


    }
}
