using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CategoriaMotivo : Resultado_Base<RubroMotivo>
    {
        public virtual int? Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string GrupoCategoriaNombre { get; set; }
        public virtual int GrupoCategoriaId { get; set; }
        public virtual List<ResultadoTabla_Motivo> Motivos { get; set; }

        public Resultado_CategoriaMotivo()
            : base()
        {

        }

        public Resultado_CategoriaMotivo(RubroMotivo entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            Motivos = ResultadoTabla_Motivo.ToList(entity.GetMotivos());
            GrupoCategoriaNombre = entity.Grupo.Nombre;
            GrupoCategoriaId = entity.Grupo.Id;
        }

        public static List<Resultado_CategoriaMotivo> ToList(List<RubroMotivo> list)
        {
            return list.Select(x => new Resultado_CategoriaMotivo(x)).ToList();
        }

    }
}
