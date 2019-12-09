using Model.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class RecursoPorOrdenTrabajo : BaseEntity
    {
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual string Personal { get; set; }
        public virtual string Flota { get; set; }
        public virtual string Material { get; set; }
        public virtual string Zona { get; set; }
        public virtual string Seccion { get; set; }
        public RecursoPorOrdenTrabajo(Comando_Recursos comando)
        {
            Personal = comando.Personal;
            Flota = comando.Flota;
            Material = comando.Material;
            Observaciones = comando.Observaciones;
        }

        public RecursoPorOrdenTrabajo() { }

    }
}
