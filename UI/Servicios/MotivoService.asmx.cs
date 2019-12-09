using Model;
using Model.Comandos;
using Model.Consultas;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UI.Resources;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MotivoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_ServicioAreaMotivo>> GetInfo()
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetInfo();
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_DataInicialControlMotivos> GetDataInicialControlMotivos(Enums.TipoMotivo tipo, bool modoBusqueda)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetDataInicialControlMotivos(tipo, modoBusqueda);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Servicio> GetServicioByIdArea(int idArea)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetServicioByIdArea(idArea);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CategoriaMotivoArea>> GetCategoriasByIdArea(int idArea)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetCategoriasByIdArea(idArea);
        }


        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Motivo>> GetByFilters(Consulta_Motivo consulta)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Motivo> Insertar(Comando_Motivo comando)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Motivo> Editar(Comando_Motivo comando)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_CampoPorMotivo> InsertCampo(Comando_Motivo_Campo comando)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).InsertarCampo(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_CampoPorMotivo> EditarCampo(Comando_Motivo_Campo comando)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarCampo(comando);
        }
        
        [WebMethod(EnableSession = true)]
        public Result<Resultado_CampoPorMotivo> EliminarCampo(Comando_Motivo_Campo comando)
        {
            ValidarSesion(Session);
            return new MotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EliminarCampo(comando);
        }

                [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CampoPorMotivo>> GetCamposByIdMotivo(int idMotivo)
        {
            ValidarSesion(Session);
            return new CampoPorMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByIdMotivo(idMotivo);
        }
    }
}
