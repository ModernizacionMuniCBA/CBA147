using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Resultados
{
    public class ResultadoApp_EstadoRequerimientoHistorial
    {
        public int Id { get; set; }
        public ResultadoApp_EstadoRequerimiento Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}