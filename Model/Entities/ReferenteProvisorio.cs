using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ReferenteProvisorio : BaseEntity
    {
        public virtual string Nombre{get;set;}
        public virtual string Apellido { get; set; }
        public virtual int? DNI { get; set; }
        public virtual string Telefono { get; set; }
        public virtual bool GeneroMasculino { get; set; }


        public ReferenteProvisorio()
        {

        }
      }
}
