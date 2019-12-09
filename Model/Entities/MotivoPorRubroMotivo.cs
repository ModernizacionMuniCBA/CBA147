using System;
using System.Linq;


namespace Model.Entities
{
    public class MotivoPorRubroMotivo : BaseEntity
    {
        public virtual RubroMotivo CategoriaMotivo { get; set; }
        public virtual Motivo Motivo { get; set; }
    }
}
