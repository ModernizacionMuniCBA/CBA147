using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CategoriaMotivoArea : Resultado_Base<CategoriaMotivoArea>
    {
        public virtual string Nombre { get; set; }

        public Resultado_CategoriaMotivoArea()
            : base()
        {

        }

        public Resultado_CategoriaMotivoArea(CategoriaMotivoArea entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
        }

        public static List<Resultado_CategoriaMotivoArea> ToList(List<CategoriaMotivoArea> list)
        {
            return list.Select(x => new Resultado_CategoriaMotivoArea(x)).ToList();
        }
    }
}
