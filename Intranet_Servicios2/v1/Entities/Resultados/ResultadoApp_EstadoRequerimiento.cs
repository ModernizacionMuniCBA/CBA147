using Model;
using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_EstadoRequerimiento
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int KeyValue { get; set; }
        public int? KeyValuePublico { get; set; }

        public ResultadoApp_EstadoRequerimiento()
        {

        }

        public ResultadoApp_EstadoRequerimiento(EstadoRequerimiento entity)
        {
            if (entity == null) { return; }

            Id = entity.Id;
            Nombre = entity.Nombre;
            Color = entity.Color;
            KeyValue = (int)entity.KeyValue;

            if (entity.KeyValuePublico.HasValue)
            {
                KeyValuePublico = (int)entity.KeyValuePublico.Value;
            }
            else
            {
                KeyValuePublico = null;
            }
        }

        public static List<ResultadoApp_EstadoRequerimiento> ToList(List<EstadoRequerimiento> list)
        {
            return list.Select(x => new ResultadoApp_EstadoRequerimiento(x)).ToList();
        }
    }
}
