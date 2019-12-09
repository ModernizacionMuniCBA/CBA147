using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_Motivo 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ResultadoTabla_Motivo()
            : base()
        {

        }

        public ResultadoTabla_Motivo(Motivo entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
        }

        public static List<ResultadoTabla_Motivo> ToList(List<Motivo> list)
        {
            return list.Select(x => new ResultadoTabla_Motivo(x)).ToList();
        }

        public static List<ResultadoTabla_Motivo> ToList(List<MotivoPorRubroMotivo> list)
        {
            return list.Select(x => new ResultadoTabla_Motivo(x.Motivo)).ToList();
        }
    }
}
