using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;

namespace UI.Servicios
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class BarrioService : _BaseService
    {

        //[WebMethod(EnableSession = true)]
        //public Result<List<Resultado_Barrio>> BuscarBarrioEnCatastro(string nombre)
        //{
        //    ValidarSesion(Session);
        //    var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

        //    return new BarrioRules(userLogueado).BuscarBarriosEnCatastro(nombre);
        //}

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Barrio>> GetBarriosXZona(Consulta_BarrioPorZona consulta)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultado = new Result<List<Resultado_Barrio>>();

            var resultadoBarriosPorZona = new BarrioPorZonaRules(userLogueado).GetByFilters(consulta);
            if (!resultadoBarriosPorZona.Ok)
            {
                resultado.Copy(resultadoBarriosPorZona.Errores);
                return resultado;
            }

            var barrios = new List<Resultado_Barrio>();
            foreach (var b in resultadoBarriosPorZona.Return)
            {
                var resultadoBarrio = new BarrioRules(userLogueado).GetById(b.IdBarrio);
                if (!resultadoBarrio.Ok)
                {
                    resultado.Copy(resultadoBarrio.Errores);
                    return resultado;
                }

                barrios.Add(new Resultado_Barrio(resultadoBarrio.Return));
            }
            resultado.Return = barrios;
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> InsertarDesdeCordobaGeoApi()
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new BarrioRules(userLogueado).InsertarDesdeCordobaGeoApi();
        }

    }
}
