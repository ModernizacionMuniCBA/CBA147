using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;

namespace Rules.Rules
{
    public class EstadoMovilRules : BaseRules<EstadoMovil>
    {

        private readonly EstadoMovilDAO dao;

        public EstadoMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoMovilDAO.Instance;
        }

        public Result<EstadoMovil> GetByKeyValue(Enums.EstadoMovil estado)
        {
            return dao.GetByKeyValue(estado);
        }

        public Result<List<Resultado_EstadoMovil>> GetAll(bool dadosDeBaja)
        {
            var result = new Result<List<Resultado_EstadoMovil>>();
            var resultConsulta = dao.GetAll(dadosDeBaja);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_EstadoMovil.ToList(resultConsulta.Return);
            return result;
        }

        public Result<List<Enums.EstadoMovil>> GetEstadosParaOT()
        {
            var result = new Result<List<Enums.EstadoMovil>>();
            var list = new List<Enums.EstadoMovil>();
            list.Add(Enums.EstadoMovil.DISPONIBLE);
            result.Return = list;
            return result;
        }

        public Result<List<Resultado_EstadoMovil>> GetAllParaCambiarEstado()
        {
            var result = new Result<List<Resultado_EstadoMovil>>();
            var resultConsulta = GetAll(false);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = resultConsulta.Return.Where(x => x.KeyValue != (int)Enums.EstadoMovil.ENFLOTA && x.KeyValue!= (int)Enums.EstadoMovil.OCUPADO).ToList();
            return result;
        }


        public Result<List<Resultado_EstadoMovil>> GetAllOcupados()
        {
            var result = new Result<List<Resultado_EstadoMovil>>();
            var resultConsulta = GetAll(false);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = resultConsulta.Return.Where(x => x.KeyValue == (int)Enums.EstadoMovil.ENFLOTA || x.KeyValue == (int)Enums.EstadoMovil.OCUPADO).ToList();
            return result;
        }
    }
}
