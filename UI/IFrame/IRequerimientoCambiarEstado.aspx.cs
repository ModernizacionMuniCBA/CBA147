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
    public partial class IRequerimientoCambiarEstado : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();
            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int id = 0;
            if (!int.TryParse("" + Request.QueryString["Id"], out id) || id <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultBuscarRQ = new RequerimientoRules(usuarioLogeado).GetById(id);
            if (!resultBuscarRQ.Ok)
            {
                resultado.Add("Error", resultBuscarRQ.Error);
                InitJs(resultado);
                return;
            }
            resultado.Add("Requerimiento", new Resultado_Requerimiento(resultBuscarRQ.Return));

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