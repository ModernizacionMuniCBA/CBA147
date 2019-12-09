using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using System.Web;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IUsuarioCerrojoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            //Cargo el servicio
            if (Request.Params["Id"] != null)
            {
                var id = int.Parse("" + Request.Params["Id"]);
                var resultUsuario = new _VecinoVirtualUsuarioRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
                if (!resultUsuario.Ok || resultUsuario.Return == null)
                {
                    resultado.Add("Error", resultUsuario.ToStringPublico());
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Usuario", new _Resultado_VecinoVirtualUsuario(resultUsuario.Return));
            }

            //veo si doy de alta un empleado o no
            if (Request.Params["Empleado"] != null)
            {
                resultado.Add("Empleado", true);
            }


            InitJs(resultado);
        }

    }
}