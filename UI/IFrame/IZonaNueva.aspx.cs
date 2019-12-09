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

namespace UI.IFrame
{
    public partial class IZonaNueva: _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            try
            {
                //Area
                var resultadoArea = new _CerrojoAreaRules(usuarioLogeado).GetByIdObligatorio(int.Parse(Request.Params["IdArea"])).Return;
                if (resultadoArea == null)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Area", new Resultado_Area(resultadoArea));

                //Si estoy editando
                int? idZona = null;
                if (Request.Params["Id"] != null)
                {
                    idZona = int.Parse(Request.Params["Id"]);
                    var resultadoZona = new ZonaRules(usuarioLogeado).GetById(idZona.Value);
                    if (!resultadoZona.Ok)
                    {
                        resultado.Add("Error", resultadoZona.Errores);
                        InitJs(resultado);
                        return;
                    }

                    if (resultadoZona.Return == null)
                    {
                        resultado.Add("Error", "La zona no existe");
                        InitJs(resultado);
                        return;
                    }

                    var resultadoBarriosZona = new BarrioPorZonaRules(usuarioLogeado).GetIdsBarrioByZona(resultadoZona.Return.Id);
                    if (!resultadoBarriosZona.Ok)
                    {
                        resultado.Add("Error", resultadoBarriosZona.Errores);
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("Zona", new Resultado_Zona(resultadoZona.Return));
                    resultado.Add("IdsBarrio", resultadoBarriosZona.Return);
                }


                //Ids barrios ya seleccionados por otros
                var resultadoIdsBarrios = new BarrioPorZonaRules(usuarioLogeado).GetIdsBarriosYaSeleccionados(idZona, resultadoArea.Id);
                if (!resultadoIdsBarrios.Ok)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("IdsBarrioNoDisponibles", resultadoIdsBarrios.Return);
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