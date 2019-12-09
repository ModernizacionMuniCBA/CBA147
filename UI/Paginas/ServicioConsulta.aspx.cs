using Intranet_UI.Utils;
using Model.Entities;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;
using Model.Resultados;

namespace UI
{
    public partial class ServicioConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Servicios
            var resultServicios = new ServicioRules(userLogeado).GetAll();
            if (!resultServicios.Ok || resultServicios.Return == null)
            {
                return;
            }
            resultado.Add("Servicios", Resultado_Servicio.ToList (resultServicios.Return));

            //Usuario
            InitUsuario(Request.RawUrl);
           // resultado.Add("Usuario", SessionKey.getUsuarioLogueado(HttpContext.Current.Session).GetPermiso(Request.RawUrl));

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }

        private void InitUsuario(string rawUrl)
        {
            var rol = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol;
            
            ////Permiso ejecutar
            //if (!rol.TienePermiso(rawUrl, Model.Enums.NivelAccesoPermiso.EJECUCION))
            //{
            //    Response.Redirect("~/Inicio.aspx");
            //    return;
            //}

            ////KeyWord VERTODO
            //var puedeVerTodo = rol.TieneKeyWord(rawUrl, "VERTODO");
            //contenedorEstado.Visible = puedeVerTodo;
        }
    }
}