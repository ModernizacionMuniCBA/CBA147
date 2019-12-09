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
using Model.Resultados;

namespace UI.Paginas.Estadisticas
{
    public partial class EstadisticaReclamoArea : System.Web.UI.Page
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
            //resultEstados.Return = resultEstados.Return.OrderBy(o => o.Orden).ToList();
            resultado.Add("Estados", resultEstados.Return);

            var resultServicios = new ServicioRules(userLogeado).GetAll(false);
            if (!resultServicios.Ok || resultServicios.Return == null)
            {
                return;
            }
            resultado.Add("Servicios", Resultado_Servicio.ToList(resultServicios.Return));     


            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
    }
}