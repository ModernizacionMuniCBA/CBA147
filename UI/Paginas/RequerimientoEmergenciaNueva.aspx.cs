using Intranet_UI.Utils;
using Model;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI.Paginas
{
    public partial class RequerimientoEmergenciaNueva : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Defensa civil
            var idArea = 243;
            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //var tipo=Request.Params["Tipo"] ;
            //if (tipo == null)
            //{
            //    resultado.Add("Error", "Error al inicializar la página");
            //    InitJs(resultado);
            //}

            //if (tipo == "107")
            //{
            var resultMotivos = new MotivoRules(userLogeado).GetByArea(idArea, Enums.TipoMotivo.PRIVADO, false);
            if (!resultMotivos.Ok)
            {
                resultado.Add("Error", resultMotivos.Error);
                InitJs(resultado);
                return;
            }

            resultado.Add("Motivos", Resultado_Motivo.ToList(resultMotivos.Return));

            var resultCategoriasMotivos = new MotivoRules(userLogeado).GetCategoriasByIdArea(idArea);
            if (!resultCategoriasMotivos.Ok)
            {
                resultado.Add("Error", resultCategoriasMotivos.Error);
                InitJs(resultado);
                return;
            }

            resultado.Add("CategoriasMotivos", resultCategoriasMotivos.Return);

            //}

            //Devuelvo la info
            InitJs(resultado);
        }

        private void InitJs(object resultado)
        {
            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}