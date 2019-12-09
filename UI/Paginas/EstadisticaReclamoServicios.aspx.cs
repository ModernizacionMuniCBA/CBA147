using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;


namespace UI
{
    public partial class EstadisticaReclamoServicios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            //Estados
            var resultEstados = new EstadoRequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetAll(false);
            if (!resultEstados.Ok || resultEstados.Return == null)
            {
                return;
            }
            resultEstados.Return = resultEstados.Return.OrderBy(o => o.Orden).ToList();
            resultado.Add("Estados", resultEstados.Return);

            //Permisos
            //resultado.Add("Permiso", SessionKey.getUsuarioLogueado(HttpContext.Current.Session).GetPermiso(Request.RawUrl));


            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
    }
}