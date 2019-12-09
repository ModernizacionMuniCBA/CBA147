using Intranet_UI.Utils;
using Model.Consultas;
using Model.Entities;
using Model.Resultados;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Resources;

namespace UI
{
    public partial class FlotaPanel : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var user=SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            ////Estados
            //var resultEstados = new EstadoEmpleadoRules(user).GetAll(false);
            //if (!resultEstados.Ok || resultEstados.Return == null)
            //{
            //    return;
            //}
            //resultado.Add("Estados", Resultado_EstadoEmpleado.ToList(resultEstados.Return));
            resultado.Add("EstadoOcupado", new EstadoFlotaRules(user).GetKeyValueEstadoOcupado().Return);
            
            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
    }
}