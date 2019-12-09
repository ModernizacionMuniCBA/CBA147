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
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class ICatalogo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Servicios
            var resultadoServicios = new ServicioRules(user).GetAll(false);
            if (!resultadoServicios.Ok || resultadoServicios.Return == null)
            {
                resultado.Add("Error", resultadoServicios.ToStringPublico());
                InitJs(resultado);
                return;
            }

            resultado.Add("Servicios", Resultado_Servicio.ToList(resultadoServicios.Return));

            //Devuelvo la data
            InitJs(resultado);
        }
    }
}