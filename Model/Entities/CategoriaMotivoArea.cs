using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class CategoriaMotivoArea : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual IList<MotivoPorRubroMotivo> Motivos { get; set; }
        public virtual List<MotivoPorRubroMotivo> GetMotivos (){
            return Motivos.Where(x => x.FechaBaja == null).ToList();
        }
    }
}
