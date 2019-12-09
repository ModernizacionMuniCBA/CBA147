using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using System.Collections.Generic;
using Model;
using Intranet_UI.Utils;
using UI.Resources;
using System.Web;
using Model.Resultados;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class ITareaPorAreaDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = Int32.Parse(Request.QueryString["Id"]);
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                //Usuario
                var resultTarea= new TareaPorAreaRules(userLogeado).GetDetalleById(id);
                if (!resultTarea.Ok)
                {
                    resultado.Add("Error", "La tarea no existe");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Tarea",  resultTarea.Return);
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}