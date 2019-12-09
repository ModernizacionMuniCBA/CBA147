using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class _CerrojoAmbitoRules : BaseRules<CerrojoAmbito>
    {

        private readonly CerrojoAmbitoDAO dao;

        public _CerrojoAmbitoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CerrojoAmbitoDAO.Instance;
        }

        public Result<CerrojoAmbito> GetByKeyValue(int keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }

        public Result<List<int>> GetIdsAreas(int keyValue)
        {
            return dao.GetIdsAreas(keyValue);
        }

        public Result<Resultado_Ambito> GetResultadoByKeyValue(int keyValue)
        {
            var resultado = new Result<Resultado_Ambito>();

            var resultadoAmbito = dao.GetByKeyValue(keyValue);
            if (!resultadoAmbito.Ok)
            {
                resultado.Copy(resultadoAmbito.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_Ambito(resultadoAmbito.Return);
            return resultado;
        }
    }
}
