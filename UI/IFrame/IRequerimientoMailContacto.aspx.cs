using Intranet_UI.Utils;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI.IFrame
{
    public partial class IRequerimientoMailContacto : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = int.Parse(Request.Params["id"] as string);

                var user = SessionKey.getUsuarioLogueado(Session);
                var resultMail = new RequerimientoRules(user).GetEmail(id);
                if (!resultMail.Ok)
                {
                    resultado.Add("Error", "Error inicializando la pagina");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Email", resultMail.Return);
                resultado.Add("IdRequerimiento", id);
            }
            catch (Exception)
            {
                resultado.Add("Error", "Identificador de requerimiento invalido");
            }

            InitJs(resultado);
        }

    }
}