using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Principal { get; set; }
        public string Color { get; set; }
        public string Icono { get; set; }
        public string UrlIcono { get; set; }

        public ResultadoApp_Servicio(Servicio entity)
        {
            this.Id = entity.Id;
            this.Nombre = entity.Nombre;
            this.Principal = entity.Principal;
            this.Color = entity.Color;
            this.Icono = entity.Icono;
            this.UrlIcono = entity.UrlIcono;
        }

        public static List<ResultadoApp_Servicio> ToList(List<Servicio> entities)
        {
            try
            {
                return entities.Select(x => new ResultadoApp_Servicio(x)).ToList();
            }
            catch (Exception e)
            {
                return new List<ResultadoApp_Servicio>();
            }
        }
    }
}
