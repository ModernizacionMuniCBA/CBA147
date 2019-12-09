using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Servicio : Resultado_Base<Servicio>
    {
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public List<int> IdsAreas { get; set; }
        public bool Principal { get; set; }
        public string Icono { get; set; }
        public string UrlIcono { get; set; }
        public string Color { get; set; }

        public Resultado_Servicio()
            : base()
        {

        }

        public Resultado_Servicio(Servicio entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
             }

            Nombre = entity.Nombre;
            Principal = entity.Principal;
            Icono = entity.Icono;
            Color = entity.Color;
            UrlIcono = entity.UrlIcono;
        }

        public static List<Resultado_Servicio> ToList(List<Servicio> list)
        {
            return list.Select(x => new Resultado_Servicio(x)).ToList();
        }
    }
}
