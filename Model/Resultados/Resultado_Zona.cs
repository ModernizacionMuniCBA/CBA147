using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Zona : Resultado_Base<Zona>
    {
        public virtual string Nombre { get; set; }
        public virtual int AreaId { get; set; }
        public virtual string AreaNombre { get; set; }
        public virtual string Observaciones { get; set; }

        public Resultado_Zona()
            : base()
        {

        }

        public Resultado_Zona(Zona entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            if (entity.Area != null)
            {
                AreaId = entity.Area.Id;
                AreaNombre = entity.Area.Nombre;
            }
            Observaciones = entity.Observaciones;
        }

        public static List<Resultado_Zona> ToList(List<Zona> list)
        {
            return list.Select(x => new Resultado_Zona(x)).ToList();
        }
    }
}
