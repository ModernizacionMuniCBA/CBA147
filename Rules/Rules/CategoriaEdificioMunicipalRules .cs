using System;
using System.Linq;
using DAO.DAO;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using System.Collections.Generic;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class CategoriaEdificioMunicipalRules  : BaseRules<CategoriaEdificioMunicipal>
    {
  
        private readonly CategoriaEdificioMunicipalDAO dao;

        public CategoriaEdificioMunicipalRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CategoriaEdificioMunicipalDAO .Instance;
        }

        public Result<List<Resultado_CategoriaEdificioMunicipal>> GetAllConEdificios()
        {
            var resultado = new Result<List<Resultado_CategoriaEdificioMunicipal>>();
            var result = GetAll(false);
            if (!result.Ok)
            {
                return resultado;
            }

            resultado.Return = Resultado_CategoriaEdificioMunicipal.ToList(result.Return.Where(x => x.EdificiosMunicipales.Count() > 0).ToList());
            return resultado;
        }
    }
}
