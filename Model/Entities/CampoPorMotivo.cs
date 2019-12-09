using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class CampoPorMotivo : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Grupo { get; set; }
        public virtual bool Obligatorio { get; set; }
        public virtual int? Orden { get; set; }
        public virtual TipoCampo Tipo { get; set; }
        public virtual Motivo Motivo { get; set; }
        public virtual string Opciones { get; set; }
    }
}
