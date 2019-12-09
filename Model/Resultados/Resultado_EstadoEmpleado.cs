using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoEmpleado : Resultado_Base<EstadoEmpleado >
    {
        public string Nombre { get; set; }
        public int KeyValue { get; set; }
        public string Color { get; set; }
        public Resultado_EstadoEmpleado():base()
        {
        }

        public Resultado_EstadoEmpleado(EstadoEmpleado entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            KeyValue = (int)entity.KeyValue;
            Color = entity.Color;
        }

        public static List<Resultado_EstadoEmpleado> ToList(List<EstadoEmpleado> list)
        {
            return list.Select(x => new Resultado_EstadoEmpleado(x)).ToList();
        }
    }
}
