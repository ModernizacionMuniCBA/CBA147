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
    public partial class IMovilSelector : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            int? idOT = null;
            //id de la ot 
            if (Request.Params["IdOT"] != null)
            {
                idOT = int.Parse("" + Request.Params["IdOT"]);
            }

            //id del area, y traigo los moviles
            if (Request.Params["IdArea"] != null)
            {
                int idArea = int.Parse("" + Request.Params["IdArea"]);
                var resultadoMovil = new MovilRules(user).GetParaAgregarAOT(idArea, idOT);
                if (!resultadoMovil.Ok || resultadoMovil.Return == null)
                {
                    resultado.Add("Error", resultadoMovil.ToStringPublico());
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Moviles", new MovilService().GetResultadoTablaByIds(resultadoMovil.Return.Select(x => x.Id).ToList()).Return.Data);
            }

            //Devuelvo la data
            InitJs(resultado);

        }
    }
}