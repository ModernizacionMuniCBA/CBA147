using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using System.Web;
using UI.Resources;
using Model;
using Model.Resultados;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class DomicilioService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_Domicilio> Buscar(double lat, double lng)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new DomicilioRules(userLogeado).BuscarResultado(lat, lng);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Domicilio>> Sugerir(string consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new DomicilioRules(userLogeado).SugerirResultado(consulta);
        }
    }
}