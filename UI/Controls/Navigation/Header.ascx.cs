using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Model.Entities;
using Rules;
using UI.Resources;
using Rules.Rules;
using System.Web;
using System.Configuration;
using System.Web.UI;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.WSVecinoVirtual.Resultados;

namespace UI.Controls.Navigation
{
    public partial class Header : System.Web.UI.UserControl
    {
        TextInfo textInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            textInfo = new CultureInfo("en-US", false).TextInfo;

            var user = SessionKey.getUsuarioLogueado(Session);
            var data = new Dictionary<string, object>();

            CrearMenu();

            //Foto
            //data.Add("UsuarioFoto", null);

            //var resultadoFoto = new Rules.WS_VecinoVirtual.ServiceCerrojo().GetFotoUsuario(user.Token);
            //if (!resultadoFoto.Ok)
            //{
            //}
            //else
            //{
            //    data.Add("UsuarioFoto", resultadoFoto.Return);
            //}

            var url = ConfigurationManager.AppSettings["URL_MIPERFIL"];
            data.Add("url_miperfil", url);

            //Version
            var resultadoVersion = new VersionSistemaRules(user).GetVersion();
            if (!resultadoVersion.Ok || resultadoVersion.Return == null)
            {
                data.Add("Version", "Sin datos");
            }
            else
            {
                data.Add("Version", resultadoVersion.Return);
            }

            InitJavascript(JsonUtils.toJson(data));
        }

        private void InitJavascript(string json)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script1", "$(function () { var data = parse( '" + json + "' ); initHeader(data); });", true);
        }


        //private void Inicializar(Usuario usuario, string foto)
        //{
        //    textoDrawer_Usuario.InnerText = textInfo.ToTitleCase(usuario.Nombre.ToLower() + " " + usuario.Apellido.ToLower());

        //    var resultConfig = new ConfiguracionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Get();
        //    if (!resultConfig.Ok)
        //    {
        //        Response.Redirect("~/Error.aspx");
        //        return;
        //    }

        //    textoVersion.InnerText = "v" + resultConfig.Return.Version;

        //    if (string.IsNullOrEmpty(foto))
        //    {
        //        imagenUsuario.Attributes["src"] = ResolveUrl("~/Resources/Imagenes/account-circle.png");
        //    }
        //    else
        //    {
        //        imagenUsuario.Attributes["src"] = ResolveUrl(foto);
        //    }
        //}

        private void CrearMenu()
        {
            var menu = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Menu;
            var rol = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol;

            string html = "";
            foreach (var menuItem in menu)
            {

                if (menuItem.Hijos == null || menuItem.Hijos.Count == 0)
                {
                    var itemHTML = CrearItem(menuItem, rol, 0);
                    if (itemHTML != "")
                    {
                        html += itemHTML;
                    }
                }
                else
                {

                    var itemHtml = CrearSubMenu(menuItem, rol, 0);
                    if (itemHtml != "")
                    {
                        html += itemHtml;
                    }
                }
            }
            contenedorMenu.InnerHtml = html;
        }

        private string CrearSubMenu(VVResultado_Menu menu, VVResultado_Permisos rol, int nivel)
        {

            bool tienePermiso = SessionKey.ValidarAcceso(rol, menu.Valor) && TienePermisoSegunAmbito(menu.Valor);
            string clase = tienePermiso ? "" : "sinPermiso";
            if (!tienePermiso) return "";

            string html = "";
            html += "<li class='subMenu " + clase + "'>";
            html += "   <div class='collapsible-header menuItem waves-effect nivel" + nivel + "'>";
            html += "       <a class='texto'>" + textInfo.ToTitleCase(menu.Nombre.ToLower()) + "</a>";
            html += "       <i class='indicador material-icons rotate90'>chevron_right</i>";
            html += "   </div>";
            html += "   <div class='collapsible-body'>";
            html += "       <ul>";


            foreach (var submenu in menu.Hijos)
            {
                if (submenu.Hijos == null || submenu.Hijos.Count == 0)
                {

                    var itemHTML = CrearItem(submenu, rol, (nivel + 1));
                    if (itemHTML != "")
                    {
                        html += itemHTML;
                    }
                }
                else
                {
                    var htmlSubMenu2 = CrearSubMenu(submenu, rol, (nivel + 1));
                    if (htmlSubMenu2 != "")
                    {
                        html += "<li>";
                        html += "   <ul class='collapsible' data-collapsible='accordion'>";
                        html += htmlSubMenu2;
                        html += "   </ul>";
                        html += "</li>";
                    }
                }
            }
            html += "       </ul>";
            html += "   </div>";
            html += "</li>";
            return html;
        }

        private string CrearItem(VVResultado_Menu menu, VVResultado_Permisos rol, int nivel)
        {
            //bool tienePermiso = TienePermiso(menu.Valor);
            bool tienePermiso = SessionKey.ValidarAcceso(rol, menu.Valor) && TienePermisoSegunAmbito(menu.Valor) && TienePermisoSegunArea(menu.Valor);
            string clase = tienePermiso ? "" : "sinPermiso";
            if (!tienePermiso) return "";

            string html = "";
            html += "<li id='" + menu.Valor + "' titulo='" + menu.Titulo + "' class='menuItem waves-effect nivel" + nivel + " " + clase + "'>";
            html += "   <a href='#' class='texto url " + clase + "' url='" + menu.Valor + "'>" + textInfo.ToTitleCase(menu.Nombre.ToLower()) + "</a>";
            html += "</li>";
            return html;
        }

        private bool TienePermisoSegunAmbito(string valor)
        {
            var ambito = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Ambito;
            var esCPC = true;

            if (ambito == null || ambito.KeyValue == 0 || ambito.KeyValue == -1)
            {
                esCPC = false;
            }

            if (valor.Equals("OrdenesDeTrabajoTop20"))
            {
                if (esCPC)
                    return false;
                return true;
            }

            return true;
        }

        private bool TienePermisoSegunArea(string valor)
        {
            var areas = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Areas;

            if (valor.Equals("RequerimientoEmergenciaNueva"))
            {
                if (areas.Where(x => x.CrearOrdenEspecial).Count() > 0)
                    return true;
                return false;
            }

            return true;
        }

        public string ToolbarTitulo
        {
            get
            {
                return textoToolbar_Titulo.InnerText;
            }
            set
            {
                textoToolbar_Titulo.InnerText = value;
            }
        }
    }
}