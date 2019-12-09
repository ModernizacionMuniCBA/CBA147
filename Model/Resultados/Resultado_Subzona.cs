using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Subzona : Resultado_Base<Subzona>
    {
        public virtual string Nombre { get; set; }
        public virtual int ZonaId { get; set; }
        public virtual string ZonaNombre { get; set; }
        public virtual int AreaId { get; set; }
        public virtual string AreaNombre { get; set; }
        public virtual string Observaciones { get; set; }

        public Resultado_Subzona()
            : base()
        {

        }

        public Resultado_Subzona(Subzona entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            Observaciones = entity.Observaciones;
            if (entity.Zona != null)
            {
                ZonaId = entity.Zona.Id;
                ZonaNombre = entity.Zona.Nombre;

                if (entity.Zona.Area != null)
                {
                    AreaId = entity.Zona.Area.Id;
                    AreaNombre = entity.Zona.Area.Nombre;
                }
            }
        }

        public static List<Resultado_Subzona> ToList(List<Subzona> list)
        {
            return list.Select(x => new Resultado_Subzona(x)).ToList();
        }
    }
}
