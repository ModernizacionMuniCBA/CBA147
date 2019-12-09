using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_RubroMotivo
    {
        public virtual int? Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual DateTime? FechaBaja {get;set;}
        public virtual string GrupoRubroNombre { get; set; }
        public virtual int GrupoRubroId { get; set; }
        public virtual string Observaciones { get; set; }
        public ResultadoTabla_RubroMotivo()
        {

        }

        public ResultadoTabla_RubroMotivo(RubroMotivo entity)
        {

            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            GrupoRubroNombre = entity.Grupo.Nombre;
            GrupoRubroId = entity.Grupo.Id;
            FechaBaja = entity.FechaBaja;
            Observaciones = entity.Observaciones;
        }

        public static List<ResultadoTabla_RubroMotivo> ToList(List<RubroMotivo> list)
        {
            return list.Select(x => new ResultadoTabla_RubroMotivo(x)).ToList();
        }

    }
}
