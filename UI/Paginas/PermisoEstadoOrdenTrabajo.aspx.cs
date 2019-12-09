using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using System.Web.UI;

using Intranet_UI.Utils;
using Model.Resultados;

namespace UI
{
    public partial class PermisoEstadoOrdenTrabajo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();

            var resultado = new PermisoEstadoOrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).GetPermisos();
            if (!resultado.Ok)
            {
                dictionary.Add("Error", resultado.Error);
                initJs(dictionary);
                return;
            }

            var consultaEstados = new EstadoOrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).GetAll(false);
            if (!consultaEstados.Ok)
            {
                dictionary.Add("Error", consultaEstados.Error);
                initJs(dictionary);
                return;
            }

            var consultaPermisos = new PermisoEstadoOrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).GetAll(false);
            if (!consultaPermisos.Ok)
            {
                dictionary.Add("Error", consultaPermisos.Error);
                initJs(dictionary);
                return;
            }

            dictionary.Add("Estados", consultaEstados.Return.OrderBy(x=>x.KeyValue).Select(x=> new Resultado_EstadoOrdenTrabajo(x)).ToList());
            dictionary.Add("Permisos", consultaPermisos.Return.Select(x => new Resultado_PermisoOrdenTrabajoItem(x)).ToList());
            dictionary.Add("InfoPermisos", resultado.Return);
            initJs(dictionary);
        }

        private void initJs(object dictionary)
        {
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}