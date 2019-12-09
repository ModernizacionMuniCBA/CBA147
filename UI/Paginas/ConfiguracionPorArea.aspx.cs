using Intranet_UI.Utils;
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
    public partial class ConfiguracionPorArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Configuracion Data Inicial
            var resultDataInicial= new ConfiguracionPorAreaRules(userLogueado).GetDataInicialConfiguraciones();
            if (!resultDataInicial.Ok || resultDataInicial.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar la información inicial.");
                var dataError = JsonUtils.toJson(resultado);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + dataError + "' ); });", true);
                return;
            }
            resultado.Add("DataInicial", resultDataInicial.Return);

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
        private void InitUsuario(string rawUrl)
        {
            var rol = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol;
        }
    }
}