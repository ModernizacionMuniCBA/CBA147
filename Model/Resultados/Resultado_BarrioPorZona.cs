using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_BarrioPorZona : Resultado_Base<BarrioPorZona>
    {
        public virtual int IdBarrio{ get; set; }
        public virtual int IdZona { get; set; }
        public virtual int IdSubZona { get; set; }

        public Resultado_BarrioPorZona()
            : base()
        {

        }

        public Resultado_BarrioPorZona(BarrioPorZona entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            if (entity.Barrio != null)
            {
                IdBarrio = entity.Barrio.Id;
            }

            if (entity.Zona != null)
            {
                IdZona = entity.Zona.Id;
            }
        }

        public static List<Resultado_BarrioPorZona> ToList(List<BarrioPorZona> list)
        {
            return list.Select(x => new Resultado_BarrioPorZona(x)).ToList();
        }
    }
}
