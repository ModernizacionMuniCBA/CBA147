using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
        [Serializable]
   public class Resultado_PermisoOrdenTrabajoAcceso
    {
        public virtual Enums.EstadoOrdenTrabajo EstadoOrdenTrabajo { get; set; }
        public virtual Enums.PermisoEstadoOrdenTrabajo Permiso { get; set; }
        public bool TienePermiso { get; set; }

        public Resultado_PermisoOrdenTrabajoAcceso()
            : base()
        {
            //var hola = "hola";
        }

    }
}
