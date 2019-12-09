using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;
using Model;

namespace UI.Controls
{
    public partial class SelectorMotivo : System.Web.UI.UserControl
    {
        public bool Interno {get;set;}

        //public int Tipo
        //{
        //    get { return Tipo; }
        //    set { Tipo = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            //var resultado = new Dictionary<string, object>();
            //var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //var tipos=new List<Enums.TipoMotivo>();
            //tipos.Add(Interno?Enums.TipoMotivo.INTERNO:Enums.TipoMotivo.GENERAL);

            //var resultServicios = new ServicioRules(userLogeado).GetByFilters(tipos, false);
            //if (!resultServicios.Ok || resultServicios.Return == null)
            //{
            //    return;
            //}
            //resultado.Add("Servicios", Resultado_Servicio.ToList(resultServicios.Return));
            //resultado.Add("Interno", Interno);

            //if (Interno)
            //{
            //    var resultAreas = new _CerrojoAreaRules(userLogeado).GetConMotivos(tipos);
            //    if (!resultAreas.Ok || resultAreas.Return == null)
            //    {
            //        return;
            //    }

            //    resultado.Add("Areas", Resultado_Area.ToList(resultAreas.Return));
            //}

            ////Devuelvo la info
            //var data = JsonUtils.toJson(resultado);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { initSelectorMotivo( '" + data + "' ); });", true);
        }
    }
}