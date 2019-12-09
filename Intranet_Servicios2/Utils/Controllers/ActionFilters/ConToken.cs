using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Intranet_Servicios2.Utils.Controllers.ActionFilters
{
    public class ConToken : _Autorizacion
    {

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (!actionContext.Request.Headers.Contains("Token"))
            {
                actionContext.Response = Error(HttpStatusCode.OK, "Debe mandar su token de acceso de Vecino Virtual");
                return;
            }
        }
    }
}