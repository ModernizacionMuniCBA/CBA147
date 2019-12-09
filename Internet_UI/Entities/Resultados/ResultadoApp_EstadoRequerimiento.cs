using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    public class ResultadoApp_EstadoRequerimiento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public int KeyValue { get; set; }
        public int? KeyValuePublico { get; set; }
    }
}