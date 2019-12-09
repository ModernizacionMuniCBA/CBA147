using System;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;

namespace Rules.Rules
{
    public class EmpleadoPorOrdenTrabajoRules : BaseRules<EmpleadoPorOrdenTrabajo>
    {
  
        private readonly EmpleadoPorOrdenTrabajoDAO dao;

        public EmpleadoPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EmpleadoPorOrdenTrabajoDAO.Instance;
        }

    }
}
