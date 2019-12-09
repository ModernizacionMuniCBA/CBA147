using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    public class Resultado_ServicioAreaMotivo
    {

        //Servicio
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public string ServicioObservaciones { get; set; }
        public bool ServicioPrincipal { get; set; }
        public string ServicioIcono { get; set; }
        public string ServicioColor { get; set; }

        //Area
        public int AreaId { get; set; }
        public int AreaIdCerrojo { get; set; }
        public string AreaNombre { get; set; }

        //Categoria
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }

        //Motivo
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public string MotivoObservaciones { get; set; }
        public string MotivoKeywords { get; set; }
        public bool MotivoUrgente { get; set; }
        public Enums.TipoMotivo MotivoTipo { get; set; }
        public bool MotivoPrincipal { get; set; }
        public Enums.PrioridadRequerimiento MotivoPrioridad { get; set; }

     

    }
}
