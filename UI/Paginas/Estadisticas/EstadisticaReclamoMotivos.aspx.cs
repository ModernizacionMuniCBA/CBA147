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
    public partial class EstadisticaReclamoMotivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Estados
            var resultEstados = new EstadoRequerimientoRules(userLogeado).GetAll(false);
            if (!resultEstados.Ok || resultEstados.Return == null)
            {
                return;
            }
            resultado.Add("Estados", resultEstados.Return);

            //Permisos
            //resultado.Add("Permiso", SessionKey.getUsuarioLogueado(HttpContext.Current.Session).GetPermiso(Request.RawUrl));
         
            
            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
    }
}