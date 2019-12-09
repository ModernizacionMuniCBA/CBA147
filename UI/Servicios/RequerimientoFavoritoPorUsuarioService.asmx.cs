using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Comandos;
using Model.Resultados;
using Model;
using Model.Consultas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class RequerimientoFavoritoPorUsuarioService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_RequerimientoFavoritoPorUsuario> ToggleFavorito(int idRequerimiento)
        {
            var resultado = new Result<Resultado_RequerimientoFavoritoPorUsuario>();

            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultadoInsert = new RequerimientoFavoritoPorUsuarioRules(userLogueado).ToggleFavorito(idRequerimiento);

            if (!resultadoInsert.Ok)
            {
                resultado.Copy(resultadoInsert.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_RequerimientoFavoritoPorUsuario(resultadoInsert.Return);
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_RequerimientoFavoritoPorUsuario> MarcarFavorito(Comando_RequerimientoFavoritoPorUsuario comando)
        {
            var resultado = new Result<Resultado_RequerimientoFavoritoPorUsuario>();

            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultadoInsert = new RequerimientoFavoritoPorUsuarioRules(userLogueado).MarcarFavorito(comando);

            if (!resultadoInsert.Ok)
            {
                resultado.Copy(resultadoInsert.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_RequerimientoFavoritoPorUsuario(resultadoInsert.Return);
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoFavoritoPorUsuarioRules(userLogueado).GetResultadoTablaRequerimientosByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoFavoritoPorUsuarioRules(userLogueado).GetCantidadRequerimientosByFilters(consulta);
        }

    }
}
