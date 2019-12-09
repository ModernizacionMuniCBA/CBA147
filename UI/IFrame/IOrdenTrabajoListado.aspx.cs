using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules.Rules;
using Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using UI.IFrame;

namespace UI
{
    public partial class IOrdenTrabajoListado : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var estadoOrdenTrabajoRules=new EstadoOrdenTrabajoRules(userLogeado);

            //Estados Para Editar
            var resultEstadosParaEditar = estadoOrdenTrabajoRules.GetEstadosValidosParaEdicion();
            resultado.Add("EstadosParaEditar", Resultado_EstadoOrdenTrabajo.ToList(resultEstadosParaEditar.Return));

            //Estados Para Cerrar
            var resultEstadosParaCerrar = estadoOrdenTrabajoRules.GetEstadosValidosParaCerrar();
            resultado.Add("EstadosParaCerrar", Resultado_EstadoOrdenTrabajo.ToList(resultEstadosParaCerrar.Return));

            //Estados Para Cancelar
            var resultEstadosParaCancelar = estadoOrdenTrabajoRules.GetEstadosValidosParaCancelar();
            resultado.Add("EstadosParaCancelar", Resultado_EstadoOrdenTrabajo.ToList(resultEstadosParaCancelar.Return));

            //Convierto la data en json
            InitJs(resultado);
        }
    }
}