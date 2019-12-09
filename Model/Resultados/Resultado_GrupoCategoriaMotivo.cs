using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_GrupoCategoriaMotivo : Resultado_Base<GrupoRubroMotivo>
    {
        public virtual string Nombre { get; set; }


        public Resultado_GrupoCategoriaMotivo()
            : base()
        {

        }

        public Resultado_GrupoCategoriaMotivo(GrupoRubroMotivo entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
        }

        public static List<Resultado_GrupoCategoriaMotivo> ToList(List<GrupoRubroMotivo> list)
        {
            return list.Select(x => new Resultado_GrupoCategoriaMotivo(x)).ToList();
        }
    }
}
