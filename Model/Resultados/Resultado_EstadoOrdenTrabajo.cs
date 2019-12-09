using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoOrdenTrabajo : Resultado_Base<EstadoOrdenTrabajo >
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int KeyValue { get; set; }
        public int? KeyValuePublico { get; set; }
        public bool Vigente { get; set; }

        public Resultado_EstadoOrdenTrabajo():base()
        {
        }

        public Resultado_EstadoOrdenTrabajo(EstadoOrdenTrabajo entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            Color = entity.Color;
            KeyValue = (int)entity.KeyValue;
        }

        public static List<Resultado_EstadoOrdenTrabajo> ToList(List<EstadoOrdenTrabajo> list)
        {
            return list.Select(x => new Resultado_EstadoOrdenTrabajo(x)).OrderBy(z=>z.KeyValue).ToList();
        }
    }
}
