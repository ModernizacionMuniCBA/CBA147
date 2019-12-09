using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using UI.Servicios;
using Model.Resultados;
using System.Web;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Resultados.Estadisticas;
//using Model.Resultados.Estadisticas;

namespace UI
{
    public partial class MapaCritico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //var resultado = new Dictionary<string, object>();

                //var fechaHasta = DateTime.Now;
                //var fechaDesde = DateTime.Now.AddDays(-30);

                //var result = new EstadisticaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetMapaCritico(Enums.TipoRequerimiento.RECLAMO, fechaDesde, fechaHasta);

                //if (!result.Ok)
                //{
                //    resultado.Add("Error", result.ToDictionary());
                //    //Devuelvo la info
                //    var data1 = JsonUtils.toJson(resultado);
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data1 + "' ); });", true);
                //    return;
                //}

                //var url = CrearMapaCpc(result.Return);
                //resultado.Add("UrlMapa", url);

                ////Devuelvo la info
                //var data = JsonUtils.toJson(resultado);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
            }
            catch (Exception ex)
            {
                msjError.InnerText = ex.Message;
            }
        }

        private Result<string> CrearMapaCpc(List<Resultado_DatosEstadisticaPanel_Cpc> CPCs)
        {
            var result = new Result<string>();
            result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");
            return result;
            /*
            try
            {
                var catastro = new CatastroMapas.CatastroWSDL();

            //    var datosCpc = new List<CatastroMapas.datosCPC>();

            //    foreach (var cpc in CPCs)
            //    {
            //        var datoCpc = new CatastroMapas.datosCPC();
            //        //cambio una linea
            //        int cant = cpc.CantidadRequerimientos;
            //        datoCpc.CantReclamos = cant;
            //        datoCpc.NumeroCPC = cpc.Cpc.Numero;
            //        datoCpc.Color = cpc.Color;
            //        datoCpc.Criticidad = cpc.Criticidad;
            //        datosCpc.Add(datoCpc);
            //    }

            //    CatastroMapas.datosCPC[] datos = datosCpc.ToArray();

            //    var url = catastro.reclamosCPC(datos);

            //    var parsedQuery = HttpUtility.ParseQueryString(url);
            //    var id = parsedQuery["idmapa"];
          
            //    result.Return = url;
            //}
            //catch (Exception e)
            //{
            //    result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");
            //    result.AddErrorInterno(e.Message);
            //    if (e.InnerException != null)
            //    {
            //        result.AddErrorInterno(e.InnerException.Message);
            //    }
            //}
            return result;
        }
             **/
        }

    }
}