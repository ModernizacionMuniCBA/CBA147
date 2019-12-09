using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoRequerimiento : Resultado_Base<EstadoRequerimiento>
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int KeyValue { get; set; }
        public int? KeyValuePublico { get; set; }
        public bool Vigente { get; set; }
        public int? Orden { get; set; }

        public Resultado_EstadoRequerimiento():base()
        {
        }

        public Resultado_EstadoRequerimiento(EstadoRequerimiento entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            Color = entity.Color;
            Orden = entity.Orden;
            KeyValue = (int)entity.KeyValue;

            if (!(entity.KeyValue == Enums.EstadoRequerimiento.CANCELADO || entity.KeyValue == Enums.EstadoRequerimiento.COMPLETADO || entity.KeyValue == Enums.EstadoRequerimiento.CERRADO))
            {
                Vigente = true;
            }

            if (entity.KeyValuePublico.HasValue)
            {
                KeyValuePublico = (int)entity.KeyValuePublico.Value;
            }
            else
            {
                KeyValuePublico = null;
            }
        }

        public static List<Resultado_EstadoRequerimiento> ToList(List<EstadoRequerimiento> list)
        {
            return list.Select(x => new Resultado_EstadoRequerimiento(x)).ToList();
        }
    }
}
