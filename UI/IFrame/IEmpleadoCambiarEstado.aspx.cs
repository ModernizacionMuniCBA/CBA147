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
    public partial class IEmpleadoCambiarEstado : _IFrame
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

            if (Request.QueryString["IdEstadoAnterior"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int idEstadoAnterior = 0;
            if (!int.TryParse("" + Request.QueryString["IdEstadoAnterior"], out idEstadoAnterior) || idEstadoAnterior <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var consultaEstados = new EstadoEmpleadoRules(usuarioLogeado).GetAllParaCambiarEstado(false);
            if (!consultaEstados.Ok)
            {
                resultado.Add("Error", consultaEstados.Error);
                InitJs(resultado);
                return;
            }

            var estadoAnterior=consultaEstados.Return.Where(x=>x.Id==idEstadoAnterior).FirstOrDefault();
            consultaEstados.Return.Remove(estadoAnterior);

            resultado.Add("Estados", consultaEstados.Return);
            resultado.Add("Id", id);
            InitJs(resultado);  
        }
    }
}