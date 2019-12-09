using Intranet_UI.Utils;
using Model.Entities;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Resources;
using UI.Servicios;


namespace UI
{
    public partial class RequerimientosFavoritosPorUsuario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();

            var rqFavoritos = new RequerimientoFavoritoPorUsuarioService().GetResultadoTablaRequerimientosByFilters(new Model.Consultas.Consulta_RequerimientoFavoritoPorUsuario()
            {
                IdUser = SessionKey.getUsuarioLogueado(Session).Usuario.Id,
                DadosDeBaja = false
            });

            if (!rqFavoritos.Ok)
            {
                resultado.Add("Error", rqFavoritos.Errores.ToStringPublico());
                initJs(resultado);
                return;
            }

            resultado.Add("Requerimientos", rqFavoritos.Return);

            //Devuelvo la data
            initJs(resultado);
        }

        private void initJs(object resultado)
        {
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}