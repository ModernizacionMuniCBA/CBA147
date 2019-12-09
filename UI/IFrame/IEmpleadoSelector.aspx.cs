using Intranet_UI.Utils;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Resources;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IEmpleadoSelector : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            int? idOT = null;

            //id de la ot 
            if (Request.Params["IdOT"] != null)
            {
                idOT = int.Parse("" + Request.Params["IdOT"]);
            }

            //id del area, y traigo los empleados
            if (Request.Params["IdArea"] != null)
            {
                int idArea = int.Parse("" + Request.Params["IdArea"]);
                var resultadoEmpleados= new EmpleadoPorAreaRules(user).GetParaAgregarAOT(idArea, idOT);
                if (!resultadoEmpleados.Ok || resultadoEmpleados.Return == null)
                {
                    resultado.Add("Error", resultadoEmpleados.ToStringPublico());
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Empleados", resultadoEmpleados.Return.Data);
            }

            //Funciones
            var resultFunciones = new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByMisAreas(false);
            if (!resultFunciones.Ok || resultFunciones.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo las Funciones'");
                return;

            }
            resultado.Add("Funciones", resultFunciones.Return);

            //Devuelvo la data
            InitJs(resultado);

        }
    }
}