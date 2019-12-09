using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IMovilNuevo2 : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Cargo el móvil
            if (Request.Params["Id"] != null)
            {
                int id = int.Parse("" + Request.Params["Id"]);
                var resultMovil = new MovilRules(user).GetResultadoById(id);
                if (!resultMovil.Ok || resultMovil.Return == null)
                {
                    resultado.Add("Error", resultMovil.ToStringPublico());
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Movil", resultMovil.Return);
            }

            //Condiciones
            var resultadoCondicion = new EnumsRules(user).GetAll(typeof(Enums.CondicionMovil));
            if (!resultadoCondicion.Ok)
            {
                resultado.Add("Error", resultadoCondicion.ToStringPublico());
                InitJs(resultado);
                return;
            }
            resultado.Add("Condiciones", resultadoCondicion.Return);

            //Tipos de combustible
            var resultadoTiposDeCombustible= new EnumsRules(user).GetAll(typeof(Enums.TipoCombustible));
            if (!resultadoTiposDeCombustible.Ok)
            {
                resultado.Add("Error", resultadoTiposDeCombustible.ToStringPublico());
                InitJs(resultado);
                return;
            }
            resultado.Add("TiposCombustible", resultadoTiposDeCombustible.Return);

            //Tipos movil
            var resultadoTipoMovil = new TipoMovilRules(user).GetAll(false);
            if (!resultadoTipoMovil.Ok)
            {
                resultado.Add("Error", resultadoTipoMovil.ToStringPublico());
                InitJs(resultado);
                return;
            }
            resultado.Add("Tipos", resultadoTipoMovil.Return);

            //Estados movil
            var resultadoEstados = new EstadoMovilRules(user).GetAllParaCambiarEstado();
            if (!resultadoEstados.Ok)
            {
                resultado.Add("Error", resultadoEstados.ToStringPublico());
                InitJs(resultado);
                return;
            }
            resultado.Add("Estados", resultadoEstados.Return);

            //Devuelvo la data
            InitJs(resultado);
        }
    }
}