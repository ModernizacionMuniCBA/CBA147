using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoMovil : Resultado_Base<EstadoMovil >
    {
        public string Nombre { get; set; }
        public int KeyValue { get; set; }
        public string Color { get; set; }
        public Resultado_EstadoMovil():base()
        {
        }

        public Resultado_EstadoMovil(EstadoMovil entity)
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

        public static List<Resultado_EstadoMovil> ToList(List<EstadoMovil> list)
        {
            return list.Select(x => new Resultado_EstadoMovil(x)).ToList();
        }
    }
}
