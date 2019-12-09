using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Intranet_Servicios2.Utils.Controllers.ActionFilters
{
    public class ConClavePanel : _Autorizacion
    {

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            var ok = false;
            if (actionContext.Request.Headers.Contains("Clave"))
            {
                var a = actionContext.Request.Headers.GetValues("Clave").First();
                var clavePanel = ConfigurationManager.AppSettings["PANEL_APP_IDENTIFIER"];
                 if (String.Equals( a, clavePanel))
                {
                    ok = true;
                }
            }

            if (!ok)
            {
                actionContext.Response = Error(HttpStatusCode.OK, "Debe mandar una clave correcta");
                return;
            }
        }
    }
}