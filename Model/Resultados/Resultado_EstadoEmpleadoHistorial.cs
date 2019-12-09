using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoEmpleadoHistorial : Resultado_Base<EstadoEmpleadoHistorial>
    {
        public Resultado_EstadoEmpleado Estado { get; set; }
        public DateTime Fecha { get; set; }

        public Resultado_EstadoEmpleadoHistorial():base()
        {
        }

        public Resultado_EstadoEmpleadoHistorial(EstadoEmpleadoHistorial entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Estado = new Resultado_EstadoEmpleado(entity.Estado);
            Fecha = entity.Fecha;
        }

        public static List<Resultado_EstadoRequerimientoHistorial> ToList(List<EstadoRequerimientoHistorial> list)
        {
            return list.Select(x => new Resultado_EstadoRequerimientoHistorial(x)).ToList();
        }
    }
}
