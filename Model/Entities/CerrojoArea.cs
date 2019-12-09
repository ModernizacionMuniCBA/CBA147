using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class CerrojoArea : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual IList<Motivo> Motivos { get; set; }
        public virtual string CodigoMunicipal { get; set; }
        public virtual bool CrearOrdenEspecial { get; set; }
        public virtual IList<CerrojoArea> Subareas { get; set; }
        public virtual CerrojoArea AreaPadre { get; set; }
        public virtual IList<ConfiguracionBandejaPorArea> TiposMotivoPorDefecto { get; set; }
        public virtual TerritorioIncumbencia TerritorioIncumbencia { get; set; }



    }
}
