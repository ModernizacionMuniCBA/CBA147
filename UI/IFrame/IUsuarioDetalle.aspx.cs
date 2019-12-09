using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Entities;
using System.Configuration;

namespace UI.IFrame
{
    public partial class IUsuarioDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = Int32.Parse(Request.QueryString["Id"]);
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                //Usuario
                var resultUsuario = new _VecinoVirtualUsuarioRules(userLogeado).GetResultadoByIdObligatorio(id);
                if (!resultUsuario.Ok)
                {
                    resultado.Add("Error", "El usuario no existe");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Usuario", resultUsuario.Return);
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}