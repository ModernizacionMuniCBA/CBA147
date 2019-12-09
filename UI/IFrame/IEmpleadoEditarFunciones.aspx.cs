using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using System.Web.UI;
using System.Web.Script.Serialization;
using Intranet_UI.Utils;
using Model.Entities;

namespace UI.IFrame
{
    public partial class IEmpleadoEditarFunciones : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            if (Request.Params["IdEmpleado"] != null)
            {
                var idEmpleado = int.Parse(Request.Params["IdEmpleado"] + "");

                //Empleado
                var consultaEmpleado = new EmpleadoPorAreaRules(usuarioLogeado).GetResultadoByIdObligatorio(idEmpleado);
                if (!consultaEmpleado.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                //Funciones
                var consultaFunciones = new FuncionPorAreaRules(usuarioLogeado).GetByIdArea(consultaEmpleado.Return.IdArea);
                if (!consultaFunciones.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Funciones", consultaFunciones.Return);
                resultado.Add("Empleado", consultaEmpleado.Return                    );
            }

            InitJs(resultado);
        }

    }
}