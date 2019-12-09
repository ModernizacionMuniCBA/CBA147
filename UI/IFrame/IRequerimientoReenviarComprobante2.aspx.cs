using Intranet_UI.Utils;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Resources;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IRequerimientoReenviarComprobante2 : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            int? idRQ = null;

            //id de la ot 
            if (Request.Params["id"] != null)
            {
                idRQ = int.Parse("" + Request.Params["id"]);
                var resultRQ = new RequerimientoRules(user).GetUsuariosReferentesById((int)idRQ);
                if (!resultRQ.Ok)
                {
                    resultado.Add("Error", resultRQ.Return);
                    InitJs(resultado);
                    return;
                }

                resultado.Add("UsuariosReferentes", resultRQ.Return);
                resultado.Add("IdRequerimiento", idRQ);
            }

            //Devuelvo la data
            InitJs(resultado);

        }
    }
}