using System;
using System.Linq;


namespace Model.Entities
{
    public class ConfiguracionBandejaPorArea : BaseEntity
    {
        public virtual CerrojoArea Area { get; set; }
        public virtual Enums.TipoMotivo TipoMotivoPorDefecto { get; set; }
        public virtual bool PorDefecto { get; set; }
    }
}
