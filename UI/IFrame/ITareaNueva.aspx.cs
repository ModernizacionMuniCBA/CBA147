using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class ITareaNueva : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var usuario=SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Cargo la tarea
            if (Request.Params["Id"] != null)
            {
                var id = int.Parse("" + Request.Params["Id"]);
                var resultSeccion = new SeccionRules(usuario).GetById(id);
                if (!resultSeccion.Ok || resultSeccion.Return == null)
                {
                    resultado.Add("Error", resultSeccion.ToStringPublico());
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Tarea", new Resultado_Seccion(resultSeccion.Return));
            }

            if (Request.Params["IdArea"] != null)
            {
                var idArea = int.Parse("" + Request.Params["IdArea"]);
                resultado.Add("IdArea", idArea);
            }

            InitJs(resultado);
        }
    }
}