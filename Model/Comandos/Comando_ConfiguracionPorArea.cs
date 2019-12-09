using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Comando_ConfiguracionPorArea
    {
        public int IdArea{get;set;}
        public Enums.TipoMotivo TipoMotivoDefectoBandeja { get; set; }
        public  Enums.EstadoOrdenTrabajo EstadoCreacionOT { get; set; }

        public Comando_ConfiguracionPorArea()
        {

        }

    }
}
