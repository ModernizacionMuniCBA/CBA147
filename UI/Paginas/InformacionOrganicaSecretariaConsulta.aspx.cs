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
    public partial class InformacionOrganicaSecretariaConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            //Busco todos los origenes
            var resultadoConsulta = new InformacionOrganicaSecretariaRules(usuarioLogeado).GetAll(null);
            if (!resultadoConsulta.Ok)
            {
                dictionary.Add("Error", "Error inicializando la pantalla");
                init(dictionary);
                return;
            }

            dictionary.Add("Secretarias", Resultado_InformacionOrganicaSecretaria.ToList(resultadoConsulta.Return));

            //Devuelvo
            init(dictionary);
        }

        private void init(object dictionary)
        {
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}