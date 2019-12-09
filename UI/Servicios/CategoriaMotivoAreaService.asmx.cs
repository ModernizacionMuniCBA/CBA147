using Model;
using Model.Comandos;
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
    /// <summary>
    /// Descripción breve de CategoriaMotivoAreaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class CategoriaMotivoAreaService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_CategoriaMotivoArea> Insertar(Comando_CategoriaMotivoArea comando)
        {
            ValidarSesion(Session);
            return new CategoriaMotivoAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_CategoriaMotivoArea> Editar(Comando_CategoriaMotivoArea comando)
        {
            ValidarSesion(Session);
            return new CategoriaMotivoAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_CategoriaMotivoArea> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new CategoriaMotivoAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Delete(id);
        }
    }
}
