using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using Intranet_UI.Utils;
using System.Web.UI;

namespace UI
{
    public partial class InformacionOrganicaConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            //Busco todos la informacion organica
            var resultadoConsulta = new InformacionOrganicaRules(usuarioLogeado).GetAll(false);
            if (!resultadoConsulta.Ok)
            {
                dictionary.Add("Error", "Error inicializando la pantalla");
                init(dictionary);
                return;
            }

            dictionary.Add("InformacionOrganica", Resultado_InformacionOrganica.ToList(resultadoConsulta.Return));


            //Areas
            var resultadoAreas = new _CerrojoAreaRules(usuarioLogeado).GetAll(false);
            dictionary.Add("Areas", Resultado_Area.ToList(resultadoAreas.Return));

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