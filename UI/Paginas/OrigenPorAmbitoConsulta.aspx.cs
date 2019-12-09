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
using Model.Resultados;

namespace UI
{
    public partial class OrigenPorAmbitoConsulta : System.Web.UI.Page
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

            //Busco todos los ambitos
            var resultadoConsultaAmbitos = new _CerrojoAmbitoRules(usuarioLogeado).GetAll(false);
            if (!resultadoConsultaAmbitos.Ok)
            {
                dictionary.Add("Error", "Error inicializando la pantalla");
                return;
            }
            dictionary.Add("Ambitos", Resultado_Ambito.ToList(resultadoConsultaAmbitos.Return));

            //Busco todos los origenes por usuario
            var resultadoConsultaOrigenes = new OrigenPorAmbitoRules(usuarioLogeado).GetAll(false);
            if (!resultadoConsultaOrigenes.Ok)
            {
                dictionary.Add("Error", "Error inicializando la pantalla");
                return;
            }
            dictionary.Add("Origenes", Resultado_OrigenPorAmbito.ToList(resultadoConsultaOrigenes.Return));

            //Devuelvo
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}