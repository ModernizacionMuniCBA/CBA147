using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
        [Serializable]
   public class Resultado_PermisoRequerimientoAcceso
    {
        public virtual Enums.EstadoRequerimiento EstadoRequerimiento { get; set; }
        public virtual Enums.PermisoEstadoRequerimiento Permiso { get; set; }
        public bool TienePermiso { get; set; }

        public Resultado_PermisoRequerimientoAcceso()
            : base()
        {
            //var hola = "hola";
        }

    }
}
