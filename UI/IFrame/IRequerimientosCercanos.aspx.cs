using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using Model.Consultas;
using UI.IFrame;

namespace UI
{
    public partial class IRequerimientosCercanos : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);


            try
            {

                int? idMotivo = null;
                if (Request.Params["IdMotivo"] != null)
                {
                    idMotivo = int.Parse(Request.Params["IdMotivo"] + "");
                }


                if (idMotivo.HasValue)
                {
                    var resultadoMotivo = new MotivoRules(userLogeado).GetById(idMotivo.Value);
                    if (!resultadoMotivo.Ok)
                    {
                        resultado.Add("Error", "Error inicializando la pantalla");
                        InitJs(resultado);
                        return;
                    }

                    if (resultadoMotivo.Return == null || resultadoMotivo.Return.FechaBaja != null)
                    {
                        resultado.Add("Error", "Error inicializando la pantalla");
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("Motivo", new Resultado_Motivo(resultadoMotivo.Return));
                }


                System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                };

                var resultadoCercanos = new RequerimientoRules(userLogeado).GetResultadoTablaCercanos(new Consulta_RequerimientoCercano()
                {
                    Default = true,
                    IdMotivo = idMotivo,
                    Latitud = double.Parse(Request.Params["Latitud"] + "", nf),
                    Longitud = double.Parse(Request.Params["Longitud"] + "", nf)
                });


                var resultadoMarcadores = new RequerimientoRules(userLogeado).GetMarcadoresGoogleMaps(resultadoCercanos.Return.Data.Select(x => x.Id).ToList());
                if (!resultadoMarcadores.Ok)
                {
                    resultado.Add("Error", "Error inicializando la pantalla");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Marcadores", resultadoMarcadores.Return);
                resultado.Add("Latitud", (Request.Params["Latitud"] + "").Replace(".", ","));
                resultado.Add("Longitud", (Request.Params["Longitud"] + "").Replace(".", ","));
                resultado.Add("Requerimientos", resultadoCercanos.Return);
            }
            catch (Exception)
            {
                resultado.Add("Error", "Error inicializando la pantalla");
                InitJs(resultado);
                return;
            }
            InitJs(resultado);
        }

    }
}