using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_ConfiguracionPorArea
    {
        public Resultado_Enums TipoMotivoDefectoBandeja { get; set; }
        public Resultado_EstadoOrdenTrabajo EstadoCreacionOT { get; set; }

        public Resultado_ConfiguracionPorArea()
        {

        }

    }
}
