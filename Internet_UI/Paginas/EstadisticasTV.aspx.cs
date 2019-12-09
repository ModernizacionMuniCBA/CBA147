using Internet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;

namespace Internet_UI.Paginas
{
    public partial class EstadisticasTV : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //var data = new Dictionary<string, object>();

            //var servicio = new Servicios.ServicioUsuario();

            //var resultadoPermisos = servicio.ValidarUsuarioEstadisticasTV();
            //if (!resultadoPermisos.Ok || !resultadoPermisos.Return)
            //{
            //    servicio.CerrarSesion();
            //    data.Add("Login", true);
            //}
            //else
            //{
            //    var resultado = new Servicios.ServicioUsuario().GetDatosUsuario();
            //    data.Add("Usuario", resultado.Return);
            //}

            //Inicializate(data);
        }

        //[WebMethod(EnableSession = true)]
        //public static WS_Usuario.CerrojoResultOfCerrojoUsuario IniciarSesion(string user, string pass)
        //{
        //    var resultado = new WS_Usuario.CerrojoResultOfCerrojoUsuario();

        //    var servicio = new Servicios.ServicioUsuario();

        //    var resultLogin = servicio.IniciarSesion(user, pass);
        //    if (!resultLogin.Ok)
        //    {
        //        resultado.Error = resultLogin.Error;
        //        return resultado;
        //    }

        //    var resultadoPermisos = servicio.ValidarUsuarioEstadisticasTV();
        //    if (!resultadoPermisos.Ok)
        //    {
        //        servicio.CerrarSesion();

        //        resultado.Error = resultadoPermisos.Error;
        //        return resultado;
        //    }

        //    if (!resultadoPermisos.Return)
        //    {
        //        servicio.CerrarSesion();

        //        resultado.Error = "No tiene permisos para ver esta pagina";
        //        return resultado;
        //    }

        //    return new Servicios.ServicioUsuario().GetDatosUsuario();
        //}

        //[WebMethod(EnableSession = true)]
        //public static WS_Usuario.CerrojoResultOfBoolean CerrarSesion()
        //{
        //    new Servicios.ServicioUsuario().CerrarSesion();
        //    var result = new WS_Usuario.CerrojoResultOfBoolean();
        //    result.Return = true;
        //    result.Ok = true;
        //    return result;
        //}

        //public void Inicializate(Dictionary<string, object> data)
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + JsonUtils.toJson(data) + "' ); });", true);
        //}

    }
}
