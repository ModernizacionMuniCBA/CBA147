using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_EstadoRequerimientoHistorial
    {
        public int Id { get; set; }
        public ResultadoApp_EstadoRequerimiento Estado { get; set; }
        public DateTime Fecha { get; set; }


        public ResultadoApp_EstadoRequerimientoHistorial()
        {

        }

        public ResultadoApp_EstadoRequerimientoHistorial(EstadoRequerimientoHistorial entity)
        {
            if (entity == null) { return; }

            Id = entity.Id;

            if (entity.Estado != null)
            {
                Estado = new ResultadoApp_EstadoRequerimiento(entity.Estado);
            }
            
            Fecha = entity.Fecha;
        }

        public static List<ResultadoApp_EstadoRequerimientoHistorial> ToList(List<EstadoRequerimientoHistorial> list)
        {
            return list.Select(x => new ResultadoApp_EstadoRequerimientoHistorial(x)).ToList();
        }
    }
}
