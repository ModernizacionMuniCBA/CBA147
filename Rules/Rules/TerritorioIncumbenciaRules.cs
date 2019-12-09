using System;
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
using Model.Utiles;
using System.Globalization;

namespace Rules.Rules
{
    public class TerritorioIncumbenciaRules : BaseRules<TerritorioIncumbencia>
    {

        private readonly TerritorioIncumbenciaDAO dao;

        public TerritorioIncumbenciaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TerritorioIncumbenciaDAO.Instance;
        }

    }
}
