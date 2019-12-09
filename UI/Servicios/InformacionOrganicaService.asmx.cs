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
    public class InformacionOrganicaService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganica> Insertar(Comando_InformacionOrganica comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganica> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_InformacionOrganica>> GetResultadoByFilters(Consulta_InformacionOrganica consulta)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaRules(userLogueado).GetResultadoByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganica> GetByIdArea(int idArea)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaRules(userLogueado).GetByIdArea(idArea);
        }

    }
}
