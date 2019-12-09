using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_NotaPorOrdenTrabajo : Resultado_Base<NotaPorOrdenTrabajo>
    {
        public virtual int IdOrdenTrabajo { get; set; }
        public virtual int IdRequerimiento { get; set; }

        public Resultado_NotaPorOrdenTrabajo()
            : base()
        {

        }

        public Resultado_NotaPorOrdenTrabajo(NotaPorOrdenTrabajo entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            IdOrdenTrabajo = entity.OrdenTrabajo.Id;
        }

        public static List<Resultado_NotaPorOrdenTrabajo> ToList(List<NotaPorOrdenTrabajo> list)
        {
            return list.Select(x => new Resultado_NotaPorOrdenTrabajo(x)).ToList();
        }
    }
}
