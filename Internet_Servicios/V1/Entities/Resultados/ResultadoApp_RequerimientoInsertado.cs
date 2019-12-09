using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Resultados
{
    public class ResultadoApp_RequerimientoInsertado
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public string ServicioColor { get; set; }
        public string ServicioIcono { get; set; }
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
    }
}