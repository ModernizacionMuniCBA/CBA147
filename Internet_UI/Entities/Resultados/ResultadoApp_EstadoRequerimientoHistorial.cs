using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    public class ResultadoApp_EstadoRequerimientoHistorial
    {
        public int Id { get; set; }
        public ResultadoApp_EstadoRequerimiento Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}