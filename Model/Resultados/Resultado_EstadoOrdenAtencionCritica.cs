using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EstadoOrdenAtencionCritica : Resultado_Base<EstadoOrdenEspecial>
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int KeyValue { get; set; }
        //public int? KeyValuePublico { get; set; }
        //public bool Vigente { get; set; }

        public Resultado_EstadoOrdenAtencionCritica():base()
        {
        }

        public Resultado_EstadoOrdenAtencionCritica(EstadoOrdenEspecial entity)
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

        public static List<Resultado_EstadoOrdenAtencionCritica> ToList(List<EstadoOrdenEspecial> list)
        {
            return list.Select(x => new Resultado_EstadoOrdenAtencionCritica(x)).ToList();
        }
    }
}
