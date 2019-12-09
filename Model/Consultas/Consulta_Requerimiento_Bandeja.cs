using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_Requerimiento_Bandeja
    {
        public bool OrdenInspeccion { get; set; }
        public string Numero { get; set; }
        public int? Año { get; set; }

        public List<Enums.PrioridadRequerimiento> Prioridades { get; set; }
        public List<Enums.TipoMotivo> Tipos{ get; set; }

        public List<Enums.EstadoRequerimiento> EstadosKeyValue { get; set; }
        public int IdATI { get; set; }

        public int? KeyValueCPC { get; set; }
        public int IdArea { get; set; }
        public List<int> IdsArea { get; set; }
        public List<int> IdsServicio { get; set; }
        public List<int> IdsMotivo { get; set; }

        public List<int> IdsPersonaFisica { get; set; }
        public List<int> IdsUsuarioReferente { get; set; }
        public List<int> IdsUsuarioCreador { get; set; }
        public List<int> IdsBarrio { get; set; }
        public List<int> IdsBarrioCatastro { get; set; }
        public string Domicilio { get; set; }
        public List<int> IdsOrigen { get; set; }

        public int? Altura { get; set; }

        public List<int> IdsZona { get; set; }
        public List<int> IdsCategoria { get; set; }
        public List<int> IdsSubzona { get; set; }

        public bool? RelevamientoDeOficio { get; set; }
        public bool? OrdenAtencionCritica { get; set; }
        public bool? Inspeccionado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool? Urgente { get; set; }
        public bool? DadosDeBaja { get; set; }
        public int? Limite { get; set; }

        public Consulta_Requerimiento_Bandeja()
        {
            DadosDeBaja = false;

        }
    }
}
