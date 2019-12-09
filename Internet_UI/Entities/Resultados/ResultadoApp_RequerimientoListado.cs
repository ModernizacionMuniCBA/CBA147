using System;
using System.Linq;

namespace InternetUI_Entities.Resultados
{
    public class ResultadoApp_RequerimientoListado
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public DateTime FechaAlta { get; set; }
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public string ServicioColor { get; set; }
        public string ServicioIcono { get; set; }
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public int EstadoId { get; set; }
        public int EstadoKeyValue { get; set; }
        public int EstadoKeyValuePublico { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public DateTime EstadoFecha { get; set; }
        public int EstadoPublicoId { get; set; }
        public int EstadoPublicoKeyValue { get; set; }
        public string EstadoPublicoNombre { get; set; }
        public string EstadoPublicoColor { get; set; }
        public int BarrioId { get; set; }
        public string BarrioNombre { get; set; }
        public int CpcId { get; set; }
        public string CpcNombre { get; set; }
        public int CpcNumero { get; set; }
    }
}