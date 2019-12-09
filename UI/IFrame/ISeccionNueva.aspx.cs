using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using System.Web;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;

namespace UI.IFrame
{
    public partial class ISeccionNueva : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            try
            {
                var idArea = -1;
                if (Request.Params["IdArea"] != null)
                {
                    idArea = int.Parse(Request.Params["IdArea"]);
                }

                //Si estoy editando
                int? idSeccion = null;
                if (Request.Params["Id"] != null)
                {
                    idSeccion = int.Parse(Request.Params["Id"]);
                    var resultadoSeccion = new SeccionRules(usuarioLogeado).GetById(idSeccion.Value);
                    if (!resultadoSeccion.Ok)
                    {
                        resultado.Add("Error", resultadoSeccion.Errores);
                        InitJs(resultado);
                        return;
                    }

                    if (resultadoSeccion.Return == null)
                    {
                        resultado.Add("Error", "La sección no existe");
                        InitJs(resultado);
                        return;
                    }

                    idArea = resultadoSeccion.Return.Area.Id;
                    resultado.Add("Seccion", new Resultado_Seccion(resultadoSeccion.Return));
                }

                //Ids empleados disponibles
                var resultadoEmpleados = new EmpleadoPorAreaRules(usuarioLogeado).GetResultadoTablaByFilters(new Consulta_Empleado()
                {
                    IdArea = idArea,
                    Seccion = false,
                    DadosDeBaja=false
                });

                if (!resultadoEmpleados.Ok)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Empleados", resultadoEmpleados.Return);
                resultado.Add("IdArea", idArea);

            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error");
            }


            //Devuelvo la info
            InitJs(resultado);
        }

    }
}