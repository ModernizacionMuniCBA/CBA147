using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_NotaPorRequerimiento : Resultado_Base<NotaPorRequerimiento>
    {
        public virtual int IdOrdenTrabajo { get; set; }
        public virtual int IdRequerimiento { get; set; }

        public Resultado_NotaPorRequerimiento()
            : base()
        {

        }

        public Resultado_NotaPorRequerimiento(NotaPorRequerimiento entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.OrdenTrabajo != null)
            {
                IdOrdenTrabajo = entity.OrdenTrabajo.Id;
            }

            IdRequerimiento = entity.Requerimiento.Id;
        }

        public static List<Resultado_NotaPorRequerimiento> ToList(List<NotaPorRequerimiento> list)
        {
            return list.Select(x => new Resultado_NotaPorRequerimiento(x)).ToList();
        }
    }
}
