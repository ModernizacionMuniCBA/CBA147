using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    public class ResultadoApp_Domicilio
    {
        public int Id { get; set; }
        public ResultadoApp_Barrio Barrio { get; set; }
        public ResultadoApp_Cpc Cpc { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Direccion { get; set; }
        public int Distancia { get; set; }
        public bool Sugerido { get; set; }
        public string Observaciones { get; set; }
    }
}