using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ServicioService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Servicio> Insertar(Comando_Servicio comando)
        {
            ValidarSesion(Session); 
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ServicioRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Servicio> Editar(Comando_Servicio comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ServicioRules(userLogueado).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<int>> GetAreasById(Consulta_Area consulta)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ServicioRules(userLogueado).GetIdsAreasById(consulta);
        }


    }
}
