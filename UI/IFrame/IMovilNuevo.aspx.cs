using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IMovilNuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            //Cargo el servicio
            if (Request.Params["Id"] != null)
            {
                var id = int.Parse("" + Request.Params["Id"]);
                var resultSeccion = new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
                if (!resultSeccion.Ok || resultSeccion.Return == null)
                {
                    Response.Clear();
                    return;
                }
                resultado.Add("Seccion", new Resultado_Seccion(resultSeccion.Return));
            }



            //Usuario
           // InitUsuario(Request.RawUrl);
           // resultado.Add("Usuario", SessionKey.getUsuarioLogueado(HttpContext.Current.Session).GetPermiso(Request.RawUrl));

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}