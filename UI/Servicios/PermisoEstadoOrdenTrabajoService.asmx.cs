﻿using Model;
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
    /// Descripción breve de PermisoEstadoOrdenTrabajoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class PermisoEstadoOrdenTrabajoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<bool> SetPermisos(List<Resultado_PermisoOrdenTrabajoAcceso> items)
        {
            ValidarSesion(Session);
            return new PermisoEstadoOrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).SetPermisos(items);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_PermisoOrdenTrabajoAcceso>> GetPermisos()
        {
            ValidarSesion(Session);
            return new PermisoEstadoOrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).GetPermisos();
        }
    }
}
