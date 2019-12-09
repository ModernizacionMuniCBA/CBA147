using System;
using System.Linq;


namespace Model.Entities
{
    public class ConfiguracionEstadoCreacionOT : BaseEntity
    {
        public virtual CerrojoArea Area { get; set; }
        public virtual EstadoOrdenTrabajo EstadoCreacionOT  { get; set; }
    }
}
