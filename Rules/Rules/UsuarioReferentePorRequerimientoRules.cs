﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;

namespace Rules.Rules
{
    public class UsuarioReferentePorRequerimientoRules : BaseRules<UsuarioReferentePorRequerimiento>
    {

        private readonly UsuarioReferentePorRequerimientoDAO dao;

        public UsuarioReferentePorRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = UsuarioReferentePorRequerimientoDAO.Instance;
        }
   
    }
}
