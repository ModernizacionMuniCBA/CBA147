using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IVisorArchivo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var paramId = "" + Request.Params["id"];
            if (paramId == null) return;

            var id = 0;
            if (!int.TryParse(paramId, out id))
            {
                return;
            }

            var resultConsulta = new ArchivoPorRequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
            if (!resultConsulta.Ok || resultConsulta.Return == null) return;

            //var archivo = resultConsulta.Return;
            //bool esPDF = archivo.Extension.ToLower().Equals("pdf");

            //var archivoJSON = JsonUtils.toJson(new Resultado_ArchivoPorRequerimiento(archivo));
            //switch (archivo.Tipo)
            //{
            //    case Enums.TipoArchivo.DOCUMENTO:
            //        if (esPDF)
            //        {
            //            LLamarJavasCript("setPDF(' " + archivoJSON + " ')");
            //        }
            //        else
            //        {
            //            LLamarJavasCript("setDocumento(' " + archivoJSON + " ')");
            //        }
            //        break;
            //    case Enums.TipoArchivo.IMAGEN:
            //        LLamarJavasCript("setImagen(' " + archivoJSON + " ')");
            //        break;
            //}
        }

        private void LLamarJavasCript(string funcion)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { " + funcion + "  });", true);
        }
    }
}