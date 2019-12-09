using Model.Entities;
using System;
using System.Linq;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoApp_Ajustes
    {
        public virtual string App { get; set; }

        public ResultadoApp_Ajustes()
        {
        }

        public ResultadoApp_Ajustes(Ajustes entity)
        {
            if (entity == null)
            {
                return;
            }

            App = entity.App;
        }
    }
}
