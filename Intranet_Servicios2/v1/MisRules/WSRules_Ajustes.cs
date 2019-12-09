using System;
using System.Linq;
using Model.Entities;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Intranet_Servicios2.Utils.MisRules;
using Intranet_Servicios2.v1.Entities.Resultados;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Newtonsoft.Json;

namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Ajustes : _WSRules_Base<Ajustes>
    {
        private readonly _WSRules_BaseAjustes rulesBase;

        public WSRules_Ajustes(UsuarioLogueado data)
            : base(data)
        {
            rulesBase = new _WSRules_BaseAjustes(data);
        }

        public ResultadoServicio<ResultadoApp_Ajustes> Get()
        {
            var resultado = new ResultadoServicio<ResultadoApp_Ajustes>();

            //Llamo al rules base
            var resultadoAjustes = rulesBase.Get();
            if (!resultadoAjustes.Ok)
            {
                resultado.Error = resultadoAjustes.Error;
                return resultado;
            }

            if (resultadoAjustes.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            //Agrego resultado
            resultado.Return = new ResultadoApp_Ajustes(resultadoAjustes.Return);
            return resultado;
        }

        public ResultadoServicio<JObject> GetAppData()
        {
            return rulesBase.GetAppData();
        }
    }
}