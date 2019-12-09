using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Comandos;
using Model.Resultados;
using System.IO;
using System.Drawing;

namespace UI.Servicios
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PermisoEstadoRequerimientoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<bool> SetPermisos(List<Resultado_PermisoRequerimientoAcceso> items)
        {
            ValidarSesion(Session);
            return new PermisoEstadoRequerimientoRules(SessionKey.getUsuarioLogueado(Session)).SetPermisos(items);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_PermisoRequerimientoAcceso>> GetPermisos()
        {
            ValidarSesion(Session);
            return new PermisoEstadoRequerimientoRules(SessionKey.getUsuarioLogueado(Session)).GetPermisos();
        }


    }
}
