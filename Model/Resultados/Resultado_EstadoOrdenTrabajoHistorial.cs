using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoOrdenTrabajoHistorial : Resultado_Base<EstadoOrdenTrabajoHistorial>
    {
        public Resultado_EstadoOrdenTrabajo Estado { get; set; }

        public DateTime Fecha { get; set; }

        public Resultado_EstadoOrdenTrabajoHistorial():base()
        {
        }

        public Resultado_EstadoOrdenTrabajoHistorial(EstadoOrdenTrabajoHistorial entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Estado = new Resultado_EstadoOrdenTrabajo(entity.Estado);
            Fecha = entity.Fecha;
        }

        public static List<Resultado_EstadoOrdenTrabajoHistorial> ToList(List<EstadoOrdenTrabajoHistorial> list)
        {
            return list.Select(x => new Resultado_EstadoOrdenTrabajoHistorial(x)).ToList();
        }
    }
}
