using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Controls.Navigation;
using UI.Resources;
using Intranet_UI.Utils;

namespace UI
{
    public partial class PaginaError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();

            //Mensaje
            var mensaje = "Error procesando la solicitud";
            if (Request.Params["mensaje"] != null)
            {
                mensaje = Request.Params["mensaje"];
            }
            dictionary.Add("Mensaje", mensaje);

            //Mensaje Color
            var mensajeColor = "black";
            if (Request.Params["mensaje_color"] != null)
            {
                mensajeColor = Request.Params["mensaje_color"];
            }
            dictionary.Add("MensajeColor", mensajeColor);

            //Icono
            var icono = "error_outline";
            if (Request.Params["icono"] != null)
            {
                icono = Request.Params["icono"];
            }
            dictionary.Add("Icono", icono);

            //Icono color
            var iconoColor = "black";
            if (Request.Params["icono_color"] != null)
            {
                iconoColor = Request.Params["icono_color"];
            }
            dictionary.Add("IconoColor", iconoColor);

            initJs(dictionary);

        }

        private void initJs(object data)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data);  });", true);
        }
    }
}