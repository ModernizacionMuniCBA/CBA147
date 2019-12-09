using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using Rules;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using Intranet_UI.Utils.Strings;
using Intranet_UI.Utils;
using System.Configuration;
using UI.Servicios;

namespace UI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionKey.IsLogin(Session))
            {
                Response.Redirect(ResolveUrl("~/Login"), true);
                return;
            }

            var user = SessionKey.getUsuarioLogueado(Session);
            if (!user.IdOrigenElegido.HasValue)
            {
                Response.Redirect(ResolveUrl("~/Origen"), false);
                return;
            }

            var data = new Dictionary<string, object>();

            data.Add("UsuarioLogeado", SessionKey.getUsuarioLogueado(Session));
            data.Add("IdOrigenElegido", SessionKey.getOrigen(Session));
            data.Add("InitData", new UsuarioService().GetInitData().Return);
            data.Add("UrlUsuarioNuevo", ConfigurationManager.AppSettings["URL_VECINO_VIRTUAL_USUARIO_NUEVO"]);

            data.Add("AlertarServerLocalDbProduccion", AlertarServerLocalDbProduccion());
            data.Add("AlertarServerTestDbProduccion", AlertarServerTestDbProduccion());
            data.Add("AlertarServerProduccionDbTest", AlertarServerProduccionDbTest());
            InitJavascript(JsonUtils.toJson(data));
        }

        private void InitJavascript(string json)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script1", "$(function () { var data = parse( '" + json + "' ); init(data); });", true);
        }

        public static bool AlertarServerLocalDbProduccion()
        {


            string host = HttpContext.Current.Request.Url.Host.ToLower();
            var isLocalHost = (host == "localhost");
            var isDeploy = bool.Parse(ConfigurationManager.AppSettings["DEPLOY"] + "");
            return isLocalHost && isDeploy;
        }

        public static bool AlertarServerTestDbProduccion()
        {
            var urlTest = ConfigurationManager.AppSettings["URL_SERVIDOR_TEST"];
            if (urlTest == null) return false;
            urlTest = urlTest.ToLower();

            string host = HttpContext.Current.Request.Url.Host.ToLower();
            var isTest = (host.Contains(urlTest));
            var isDeploy = bool.Parse(ConfigurationManager.AppSettings["DEPLOY"] + "");
            return isTest && isDeploy;
        }

        public static bool AlertarServerProduccionDbTest()
        {
            var urlTest = ConfigurationManager.AppSettings["URL_SERVIDOR_TEST"];
            if (urlTest == null) return false;
            urlTest = urlTest.ToLower();

            string host = HttpContext.Current.Request.Url.Host.ToLower();
            var isProduccion = host != "localhost" && !host.Contains(urlTest);
            var isDeploy = bool.Parse(ConfigurationManager.AppSettings["DEPLOY"] + "");
            return isProduccion && !isDeploy;
        }

    }
}