using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_TipoMovil : Resultado_Base<TipoMovil>
    {
        public virtual string Nombre { get; set; }

        public Resultado_TipoMovil()
            : base()
        {

        }

        public Resultado_TipoMovil(TipoMovil entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
        }

        public static List<Resultado_TipoMovil> ToList(List<TipoMovil> list)
        {
            return list.Select(x => new Resultado_TipoMovil(x)).ToList();
        }
    }
}
