using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;

namespace Rules.Rules
{
    public class CampoPorMotivoRules : BaseRules<CampoPorMotivo>
    {

        private readonly CampoPorMotivoDAO dao;

        public CampoPorMotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CampoPorMotivoDAO.Instance;
        }

        public  Result<bool> Equals(CampoPorMotivo comando)
        {
           return dao.Equals(comando);
        }

        public Result<List<Resultado_CampoPorMotivo>> GetByIdMotivo(int idMotivo){
            var resultado = new Result<List<Resultado_CampoPorMotivo>>();

            var resultadoConsulta = dao.GetByIdMotivo(idMotivo);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = Resultado_CampoPorMotivo.ToList(resultadoConsulta.Return.ToList());
            }

            return resultado;
        }
    }
}
