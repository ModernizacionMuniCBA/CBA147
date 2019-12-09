using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
        [Serializable]
   public class Resultado_PermisoOrdenInspeccionAcceso
    {
        public virtual Enums.EstadoOrdenInspeccion EstadoOrdenInspeccion{ get; set; }
        public virtual Enums.PermisoEstadoOrdenInspeccion Permiso { get; set; }
        public bool TienePermiso { get; set; }

        public Resultado_PermisoOrdenInspeccionAcceso()
            : base()
        {
            //var hola = "hola";
        }

    }
}
