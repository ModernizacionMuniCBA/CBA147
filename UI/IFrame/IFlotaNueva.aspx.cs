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
using Model;

namespace UI.IFrame
{
    public partial class IFlotaNueva : _IFrame
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
                int? idFlota = null;
                if (Request.Params["Id"] != null)
                {
                    idFlota = int.Parse(Request.Params["Id"]);
                    var resultadoFlota= new FlotaRules(usuarioLogeado).GetById(idFlota.Value);
                    if (!resultadoFlota.Ok)
                    {
                        resultado.Add("Error", resultadoFlota.Errores);
                        InitJs(resultado);
                        return;
                    }

                    if (resultadoFlota.Return == null)
                    {
                        resultado.Add("Error", "La flota no existe");
                        InitJs(resultado);
                        return;
                    }

                    idArea = resultadoFlota.Return.Area.Id;
                    resultado.Add("Flota", new Resultado_Flota(resultadoFlota.Return));
                }

                //Ids empleados disponibles
                var resultadoEmpleados = new EmpleadoPorAreaRules(usuarioLogeado).GetResultadoTablaByFilters(new Consulta_Empleado()
                {
                    IdArea = idArea,
                    Flota = false,
                    Estados = new List<Enums.EstadoEmpleado>() { Enums.EstadoEmpleado.DISPONIBLE, Enums.EstadoEmpleado.OCUPADO},
                    DadosDeBaja = false
                });

                if (!resultadoEmpleados.Ok)
                {
                    resultado.Add("Error", "Error leyendo los empleados disponibles");
                    InitJs(resultado);
                    return;
                }
                
                //Moviles disponibles
                var resultadoMoviles = new MovilRules(usuarioLogeado).GetResultadoTablaByFilters(new Consulta_Movil()
                {
                    IdArea = idArea,
                    Estados = new List<Enums.EstadoMovil>() { Enums.EstadoMovil.DISPONIBLE },
                    DadosDeBaja = false,
                    Flota=false
                });

                if (!resultadoMoviles.Ok)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Moviles", resultadoMoviles.Return);
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