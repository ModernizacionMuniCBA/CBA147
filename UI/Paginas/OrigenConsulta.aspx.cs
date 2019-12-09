using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Controls.Navigation;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Consultas;

namespace UI
{
    public partial class OrigenConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();

            //Valido usuario logeado
            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);
            if (usuarioLogeado == null)
            {
                Response.Redirect("~/Login", false);
                return;
            }

            //Busco todos los origenes
            Consulta_Origen consulta = new Consulta_Origen();
            var resultadoConsulta = new OrigenRules(usuarioLogeado).GetResultadoByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                dictionary.Add("Error", "Error inicializando la pantalla");
                return;
            }
            dictionary.Add("Origenes", resultadoConsulta.Return);

            //Devuelvo
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}