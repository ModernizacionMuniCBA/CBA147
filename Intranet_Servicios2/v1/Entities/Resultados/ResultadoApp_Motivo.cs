using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_Motivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Principal { get; set; }
        public string Keywords { get; set; }

        public ResultadoApp_Motivo(Motivo motivo)
        {
            this.Id = motivo.Id;
            this.Nombre = motivo.Nombre;
            this.Principal = motivo.Principal;
            this.Keywords = motivo.Keywords;
        }

        public static List<ResultadoApp_Motivo> ToList(List<Motivo> entities)
        {
            try
            {
                return entities.Select(x => new ResultadoApp_Motivo(x)).ToList();
            }
            catch (Exception e)
            {
                return new List<ResultadoApp_Motivo>();
            }
        }
    }
}
