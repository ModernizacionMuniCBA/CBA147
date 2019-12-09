using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrigenPorArea : Resultado_Base<OrigenPorArea>
    {
        public virtual int AreaId { get; set; }
        public virtual string AreaNombre { get; set; }
        public virtual int OrigenId { get; set; }
        public virtual string OrigenNombre { get; set; }
        
        public Resultado_OrigenPorArea()
            : base()
        {

        }

        public Resultado_OrigenPorArea(OrigenPorArea entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            if (entity.Area != null)
            {
                AreaId = entity.Area.Id;
                AreaNombre = entity.Area.Nombre;
            }
            if (entity.Origen != null)
            {
                OrigenId = entity.Origen.Id;
                OrigenNombre = entity.Origen.Nombre;
            }
        }

        public static List<Resultado_OrigenPorArea> ToList(List<OrigenPorArea> list)
        {
            return list.Select(x => new Resultado_OrigenPorArea(x)).ToList();
        }
    }
}
