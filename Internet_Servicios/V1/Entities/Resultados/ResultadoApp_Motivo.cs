using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Resultados
{
    public class ResultadoApp_Motivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Principal { get; set; }
        public string Keywords { get; set; }
    }
}