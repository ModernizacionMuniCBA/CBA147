using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_MovilPorOrdenTrabajo : Resultado_Base<MovilPorOrdenTrabajo>
    {
        public virtual Resultado_Movil Movil { get; set; }

        public Resultado_MovilPorOrdenTrabajo()
            : base()
        {

        }
        public Resultado_MovilPorOrdenTrabajo(MovilPorOrdenTrabajo entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Movil = new Resultado_Movil(entity.Movil);
        }

        public static List<Resultado_MovilPorOrdenTrabajo> ToList(List<MovilPorOrdenTrabajo> list)
        {
            return list.Select(x => new Resultado_MovilPorOrdenTrabajo(x)).ToList();
        }
    }
}
