using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_InitData
    {

        public virtual DataRequerimiento Requerimiento { get; set; }
        public virtual DataOrdenTrabajo OrdenTrabajo { get; set; }
        public virtual DataOrdenInspeccion OrdenInspeccion { get; set; }

        public Resultado_InitData()
            : base()
        {
            this.Requerimiento = new DataRequerimiento();
            this.OrdenTrabajo = new DataOrdenTrabajo();
            this.OrdenInspeccion = new DataOrdenInspeccion();
        }
    }

    public class DataRequerimiento
    {

        public virtual List<Resultado_PermisoRequerimientoAcceso> Permisos { get; set; }
        public virtual List<Resultado_EstadoRequerimiento> Estados { get; set; }

        public DataRequerimiento()
        {
        }
    }

    public class DataOrdenTrabajo
    {

        public virtual List<Resultado_PermisoOrdenTrabajoAcceso> Permisos { get; set; }
        public virtual List<Resultado_EstadoOrdenTrabajo> Estados { get; set; }

        public DataOrdenTrabajo()
        {
        }
    }

    public class DataOrdenInspeccion
    {

        public virtual List<Resultado_PermisoOrdenInspeccionAcceso> Permisos { get; set; }
        public virtual List<Resultado_EstadoOrdenInspeccion> Estados { get; set; }

        public DataOrdenInspeccion()
        {
        }
    }
}
