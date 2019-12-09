using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_ConfiguracionPorAreaDataInicial
    {
        public List<Resultado_Enums> TiposMotivo { get; set; }
        public List<Resultado_EstadoOrdenTrabajo> EstadosCreacionOT{ get; set; }

        public Resultado_ConfiguracionPorAreaDataInicial()
        {

        }

    }
}
