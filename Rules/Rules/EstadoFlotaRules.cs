using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;

namespace Rules.Rules
{
    public class EstadoFlotaRules : BaseRules<EstadoFlota>
    {

        private readonly EstadoFlotaDAO dao;

        public EstadoFlotaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoFlotaDAO.Instance;
        }

        public Result<EstadoFlota> GetByKeyValue(Enums.EstadoFlota estado)
        {
            return dao.GetByKeyValue(estado);
        }

        //public Result<List<Resultado_EstadoFlota>> GetAllParaCambiarEstado(bool dadosDeBaja)
        //{
        //    var result = new Result<List<Resultado_EstadoFlota>>();
        //    var resultConsulta = dao.GetAll(dadosDeBaja);
        //    if (!resultConsulta.Ok)
        //    {
        //        result.Copy(resultConsulta.Errores);
        //        return result;
        //    }

        //    var list = resultConsulta.Return.Where(z => z.KeyValue != Enums.EstadoFlota.ENPROCESO).ToList();
        //    result.Return = Resultado_EstadoFlota.ToList(list);
        //    return result;
        //}

        public Result<List<Enums.EstadoFlota>> GetEstadosParaOT()
        {
            var result = new Result<List<Enums.EstadoFlota>>();
            var list = new List<Enums.EstadoFlota>();
            list.Add(Enums.EstadoFlota.DISPONIBLE);
            result.Return = list;
            return result;
        }

        public Result<int> GetKeyValueEstadoOcupado()
        {
            var result = new Result<int>();
            result.Return = (int)Enums.EstadoFlota.OCUPADO;
            return result;
        }
    }
}
