using System;
using System.Linq;
using Model;
using Model.Entities;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Newtonsoft.Json.Linq;
using Intranet_Servicios2.Utils.Entities.Comando;
using System.Configuration;
using Newtonsoft.Json;

namespace Intranet_Servicios2.Utils.MisRules
{
    public class _WSRules_BaseAjustes : _WSRules_Base<BaseEntity>
    {
        private readonly AjustesRules rules;

        public _WSRules_BaseAjustes(UsuarioLogueado data)
            : base(data)
        {
            rules = new AjustesRules(data);
        }

        public ResultadoServicio<Ajustes> Get()
        {
            var resultado = new ResultadoServicio<Ajustes>();

            //Llamo al rules
            var resultadoAjustes = rules.Get();
            if (!resultadoAjustes.Ok)
            {
                resultado.Error = resultadoAjustes.Errores.ToStringPublico();
                return resultado;
            }

            if (resultadoAjustes.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            //Agrego resultado
            resultado.Return = resultadoAjustes.Return;
            return resultado;
        }

        public ResultadoServicio<JObject> GetAppData()
        {
            var resultado = new ResultadoServicio<JObject>();

            try
            {
                var resultadoAjustes = Get();
                if (!resultadoAjustes.Ok)
                {
                    resultado.Error = resultadoAjustes.Error;
                    return resultado;
                }
                resultado.Return = JsonConvert.DeserializeObject<JObject>(resultadoAjustes.Return.App);

            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
            }


            return resultado;
        }
    }
}