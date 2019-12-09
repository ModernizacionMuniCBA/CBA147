using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Motivo : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Tema Tema { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual string Keywords { get; set; }
        public virtual bool Urgente { get; set; }

        public virtual bool Principal { get; set; }
        public virtual Enums.PrioridadRequerimiento Prioridad { get; set; }
        public virtual Enums.EsfuerzoMotivo Esfuerzo { get; set; }
        public virtual Enums.TipoMotivo Tipo { get; set; }
        public virtual CategoriaMotivoArea Categoria { get; set; }
        public virtual IList<CampoPorMotivo> Campos { get; set; }
        public virtual IList<CampoPorMotivo> getCampos()
        {
            if (Campos == null || Campos.Count == 0) return new List<CampoPorMotivo>();
            return Campos.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual IList<MotivoPorRubroMotivo> Etiquetas { get; set; }
        public virtual IList<MotivoPorRubroMotivo> getEtiquetas()
        {
            if (Etiquetas == null || Etiquetas.Count == 0) return new List<MotivoPorRubroMotivo>();
            return Etiquetas.Where(x => x.FechaBaja == null).ToList();
        }
    }
}


