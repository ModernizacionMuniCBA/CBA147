using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoFlota : Resultado_Base<EstadoFlota>
    {
        public string Nombre { get; set; }
        public int KeyValue { get; set; }
        public string Color { get; set; }
        public Resultado_EstadoFlota():base()
        {
        }

        public Resultado_EstadoFlota(EstadoFlota entity)
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

        public static List<Resultado_EstadoFlota> ToList(List<EstadoFlota> list)
        {
            return list.Select(x => new Resultado_EstadoFlota(x)).ToList();
        }
    }
}
