using Internet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class Pagina : System.Web.UI.MasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var servicioUsuario = new Servicios.ServicioUsuario();

            var resultadoLogin = servicioUsuario.ValidarToken();
            if (!resultadoLogin.Ok || resultadoLogin.Return == false)
            {
                Response.Redirect("~/" + Consts.PAGE_LOGIN, false);
                return;
            }


            // Verifico Renaper
            if (Request.AppRelativeCurrentExecutionFilePath != "~/" + Consts.PAGE_VALIDAR_DATOS)
            {
                var resultadoRenaper = servicioUsuario.EsValidadoRenaper();
                if (!resultadoRenaper.Ok || !resultadoRenaper.Return.Value)
                {
                    Response.Redirect("~/" + Consts.PAGE_VALIDAR_DATOS, false);
                    return;
                }
            }

            // Busco al usuario logeado
            var resultado = new Dictionary<string, object>();

            var resultadoConsulta = servicioUsuario.GetDatosUsuario();
            if (!resultadoConsulta.Ok)
            {
                resultado.Add("Error", resultadoConsulta.Error);
                JsUtils.InitMasterPage(this, resultado);
                return;
            }

            resultado.Add("Usuario", resultadoConsulta.Return);
            resultado.Add("Token",  Session[Consts.TOKEN]);
            JsUtils.InitMasterPage(this, resultado);
        }
    }
}