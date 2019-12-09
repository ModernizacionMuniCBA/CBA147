using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_TareaPorArea 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public string AreaNombre { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Observaciones {get; set; }

        public ResultadoTabla_TareaPorArea()
            : base()
        {

        }

        public ResultadoTabla_TareaPorArea(TareaPorArea entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            IdArea = entity.Area.Id;
            AreaNombre = entity.Area.Nombre;
            FechaBaja = entity.FechaBaja;
            Observaciones = entity.Observaciones;
        }

        public static List<ResultadoTabla_TareaPorArea> ToList(List<TareaPorArea> list)
        {
            return list.Select(x => new ResultadoTabla_TareaPorArea(x)).ToList();
        }
    }
}
