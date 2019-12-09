using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoRequerimientoHistorial : Resultado_Base<EstadoRequerimientoHistorial>
    {
        public Resultado_EstadoRequerimiento Estado { get; set; }
        public DateTime Fecha { get; set; }

        public Resultado_EstadoRequerimientoHistorial():base()
        {
        }

        public Resultado_EstadoRequerimientoHistorial(EstadoRequerimientoHistorial entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Estado = new Resultado_EstadoRequerimiento(entity.Estado);
            Fecha = entity.Fecha;
        }

        public static List<Resultado_EstadoRequerimientoHistorial> ToList(List<EstadoRequerimientoHistorial> list)
        {
            return list.Select(x => new Resultado_EstadoRequerimientoHistorial(x)).ToList();
        }
    }
}
