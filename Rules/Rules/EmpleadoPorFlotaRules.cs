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
    public class EmpleadoPorFlotaRules : BaseRules<EmpleadoPorFlota>
    {
  
        private readonly EmpleadoPorFlotaDAO dao;

        public EmpleadoPorFlotaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EmpleadoPorFlotaDAO.Instance;
        }


        public Result<EmpleadoPorFlota> GetByIdEmpleado(int id)
        {
            return dao.GetByIdEmpleado(id);
        }
    }
}
