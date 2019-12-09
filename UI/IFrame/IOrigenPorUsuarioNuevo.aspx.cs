using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using System.Web.UI;
using System.Web.Script.Serialization;
using Intranet_UI.Utils;
using Model.Entities;

namespace UI.IFrame
{
    public partial class IOrigenPorUsuarioNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);


            if (Request.Params["IdUsuario"] != null)
            {
                var idUsuario = int.Parse(Request.Params["IdUsuario"] + "");
                var resultadoConsulta = new BaseRules<_VecinoVirtualUsuario>(usuarioLogeado).GetById(idUsuario);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Add("Error", resultadoConsulta.ToStringPublico());
                    InitJs(resultado);
                    return;
                }

                if (resultadoConsulta.Return != null)
                {
                    resultado.Add("Usuario", new _Resultado_VecinoVirtualUsuario(resultadoConsulta.Return));
                }
            }

            var resultadoOrigenes = new OrigenRules(usuarioLogeado).GetAll(false);
            if (!resultadoOrigenes.Ok)
            {
                resultado.Add("Error", resultadoOrigenes.ToStringPublico());
                InitJs(resultado);
                return;
            }

            resultado.Add("Origenes", Resultado_Origen.ToList(resultadoOrigenes.Return));

            InitJs(resultado);
        }

    }
}