using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IOrdenTrabajoQuitarRequerimiento : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultado = new Dictionary<string, object>();


            //Ot
            int idOt = 0;
            int idOi = 0;
            if (int.TryParse("" + Request.QueryString["IdOt"], out idOt) )
            {
                var resultBuscarOt = new OrdenTrabajoRules(usuarioLogeado).GetById(idOt);
                if (!resultBuscarOt.Ok)
                {
                    resultado.Add("Error", resultBuscarOt.Error);
                    InitJs(resultado);
                    return;
                }
                resultado.Add("OrdenTrabajo", new Resultado_OrdenTrabajo(resultBuscarOt.Return));
                //Oi
            }
            else if (int.TryParse("" + Request.QueryString["IdOi"], out idOi))
            {
                var resultBuscarOi = new OrdenInspeccionRules(usuarioLogeado).GetById(idOi);
                if (!resultBuscarOi.Ok)
                {
                    resultado.Add("Error", resultBuscarOi.Error);
                    InitJs(resultado);
                    return;
                }
                resultado.Add("OrdenInspeccion", new Resultado_OrdenInspeccion(resultBuscarOi.Return));
            }
            else
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }
     

            //Rq
            int idRq = 0;
            if (!int.TryParse("" + Request.QueryString["IdRq"], out idRq) || idRq <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            var resultBuscarRQ = new RequerimientoRules(usuarioLogeado).GetById(idRq);
            if (!resultBuscarRQ.Ok)
            {
                resultado.Add("Error", resultBuscarRQ.Error);
                InitJs(resultado);
                return;
            }
            resultado.Add("Requerimiento", new Resultado_Requerimiento(resultBuscarRQ.Return));

            //Estados
            var consultaEstados = new PermisoEstadoRequerimientoRules(usuarioLogeado).GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento.VerEnCambioEstado);
            if (!consultaEstados.Ok)
            {
                resultado.Add("Error", consultaEstados.Error);
                InitJs(resultado);
                return;
            }
            resultado.Add("Estados", Resultado_EstadoRequerimiento.ToList(new EstadoRequerimientoRules(usuarioLogeado).ObtenerEstados(consultaEstados.Return).Return));

            InitJs(resultado);
        }
    }
}