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
using System.Collections.Generic;

namespace Rules.Rules
{
    public class FuncionPorEmpleadoRules  : BaseRules<FuncionPorEmpleado>
    {
  
        private readonly FuncionPorEmpleadoDAO dao;

        public FuncionPorEmpleadoRules (UsuarioLogueado data)
            : base(data)
        {
            dao = FuncionPorEmpleadoDAO.Instance;
        }

        public Result<List<Resultado_FuncionPorEmpleado>> GetByIdsEmpleados(List<int> idsEmpleados)
        {
            return dao.GetByIdsEmpleados(idsEmpleados);
        }

    }
}
