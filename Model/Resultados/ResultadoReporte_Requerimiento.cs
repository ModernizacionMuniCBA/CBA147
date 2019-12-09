using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoReporte_Requerimiento
    {

        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }

        public Enums.PrioridadRequerimiento Prioridad { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }

        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }

        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public int MotivoTipo { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        public int AreaId { get; set; }
        public string AreaNombre { get; set; }

        public int? OrdenEspecialId { get; set; }
        public int EstadoOrdenEspecialId { get; set; }
        public string EstadoOrdenEspecialNombre { get; set; }
        public string EstadoOrdenEspecialColor { get; set; }
        public int EstadoOrdenEspecialKeyValue { get; set; }

        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public Enums.EstadoRequerimiento EstadoKeyValue { get; set; }
        
        public string Descripcion { get; set; }
        public string BarrioNombre { get; set; }
        public string CpcNombre { get; set; }
        public int CpcNumero { get; set; }

        public bool Marcado { get; set; }
        public bool Urgente { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string DomicilioDireccion { get; set; }
        public string DomicilioLatitud { get; set; }
        public string DomicilioLongitud { get; set; }

        public int Dias { get; set; }

        public bool? Favorito { get; set; }
        public bool? Inspeccionado { get; set; }

        public ResultadoReporte_Requerimiento():base() { }

        public ResultadoReporte_Requerimiento(Requerimiento requerimiento)
        {
            
        }

    }
}
