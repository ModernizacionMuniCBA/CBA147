using Model;
using Model.Comandos;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UI.Servicios
{
    /// <summary>
    /// Descripción breve de EdificioMunicipalService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class EdificioMunicipalService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_EdificioMunicipal> Insert(Comando_EdificioMunicipal comando)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_EdificioMunicipal> Editar(Comando_EdificioMunicipal comando)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_EdificioMunicipal>> GetResultadoTabla(int idCategoria)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).GetResultadoTabla(idCategoria);
        }


        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_EdificioMunicipal> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).GetResultadoTablaById(id);
        }


        [WebMethod(EnableSession = true)]
        public Result<Resultado_Domicilio> GetDomicilioById(int id)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).GetDomicilioById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_EdificioMunicipal> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new EdificioMunicipalRules(GetUsuarioLogeado()).DarDeBaja(id);
        }

          [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CategoriaEdificioMunicipal>> GetCategoriasConEdificios()
        {
            ValidarSesion(Session);
            return new CategoriaEdificioMunicipalRules(GetUsuarioLogeado()).GetAllConEdificios();
        }
    }
}
