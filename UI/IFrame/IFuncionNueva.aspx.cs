using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IFuncionNueva : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            if (Request.Params["IdArea"] == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las funciones.");
                InitJs(resultado);
                return;
            }

                var idArea = Int32.Parse(Request.Params["IdArea"]);
           
            //Funciones
            var resultFunciones= new FuncionPorAreaRules(userLogueado).GetByIdArea(idArea);
            if (!resultFunciones.Ok || resultFunciones.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las funciones.");
                InitJs(resultado);
                return;
            }
            resultado.Add("Funciones", resultFunciones.Return);
            resultado.Add("IdArea", idArea);
            //Devuelvo la data
            InitJs(resultado);
        }

    }
}