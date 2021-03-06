﻿using System;
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
    public class DireccionService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_Direccion> Insertar(Comando_Direccion comando)
        {
            ValidarSesion(Session);

            return new DireccionRules(SessionKey.getUsuarioLogueado(Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Direccion> Actualizar(Comando_Direccion comando)
        {
            ValidarSesion(Session);

            return new DireccionRules(SessionKey.getUsuarioLogueado(Session)).Actualizar(comando);
        }

    }
}
