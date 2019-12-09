using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using System.Collections.Generic;
using Model;
using Intranet_UI.Utils;
using UI.Resources;
using System.Web;
using Model.Resultados;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IFlotaDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = Int32.Parse(Request.QueryString["Id"]);
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                //Flota
                var resultFlota= new FlotaRules(userLogeado).GetResultadoById(id);
                if (!resultFlota.Ok)
                {
                    resultado.Add("Error", "La flota no existe");
                    InitJs(resultado);
                    return;
                }

                //Ordenes de Tranajo
                var resultOT = new OrdenTrabajoRules(userLogeado).GetByIdFlota(id);
                if (!resultOT.Ok)
                {
                    resultado.Add("Error", "Error al consultar uno de los datos");
                    InitJs(resultado);
                    return;
                }

                var resultEstadoOcupado = new EstadoFlotaRules(userLogeado).GetByKeyValue(Enums.EstadoFlota.OCUPADO);
                if (!resultEstadoOcupado.Ok)
                {
                    resultado.Add("Error", "Error al consultar uno de los datos");
                    InitJs(resultado);
                    return;
                }

                var resultEstadoFinalizado= new EstadoFlotaRules(userLogeado).GetByKeyValue(Enums.EstadoFlota.TURNOTERMINADO);
                if (!resultEstadoOcupado.Ok)
                {
                    resultado.Add("Error", "Error al consultar uno de los datos");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Flota", resultFlota.Return);
                resultado.Add("OrdenesTrabajo", resultOT.Return.Data);
                resultado.Add("EstadoOcupado", new Resultado_EstadoFlota( resultEstadoOcupado.Return));
                resultado.Add("EstadoTurnoTerminado", new Resultado_EstadoFlota(resultEstadoFinalizado.Return));
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}