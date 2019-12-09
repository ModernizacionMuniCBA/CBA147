using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_DataInicialControlMotivos 
    {
        public virtual List<Resultado_Servicio> Servicios { get; set; }
        public virtual List<Resultado_Area> Areas { get; set; }

        public Resultado_DataInicialControlMotivos()
            : base()
        {
        }

    }
}
