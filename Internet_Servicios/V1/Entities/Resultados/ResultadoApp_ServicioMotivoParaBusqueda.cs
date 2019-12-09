using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Resultados
{
    public class ResultadoApp_ServicioMotivoParaBusqueda
    {
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public string MotivoKeywords { get; set; }
    }
}