using Intranet_UI.Utils;
using Model.Consultas;
using Model.Entities;
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
    public partial class ZonaConsulta : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(Session);

            var zonaRules = new ZonaRules(userLogueado);

            //Zonas
            var resultZonas = zonaRules.GetResultadoTablaByFilters(new Consulta_Zona()
            {
                IdsArea = userLogueado.IdsAreas,
                DadosDeBaja = null
            });
            resultado.Add("Zonas", resultZonas.Return);

            //Areas
            var areas = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Areas;
            resultado.Add("Areas", areas);

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}