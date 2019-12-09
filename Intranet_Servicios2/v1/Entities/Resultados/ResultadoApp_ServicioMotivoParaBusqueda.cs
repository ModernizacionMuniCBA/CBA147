using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_ServicioMotivoParaBusqueda
    {
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }

        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public string MotivoKeywords { get; set; }

        public ResultadoApp_ServicioMotivoParaBusqueda()
        {

        }

    }
}
