using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;

namespace Rules.Rules
{
    public class EstadoEmpleadoRules : BaseRules<EstadoEmpleado>
    {

        private readonly EstadoEmpleadoDAO dao;

        public EstadoEmpleadoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoEmpleadoDAO.Instance;
        }

        public Result<EstadoEmpleado> GetByKeyValue(Enums.EstadoEmpleado estado)
        {
            return dao.GetByKeyValue(estado);
        }

        public Result<List<Resultado_EstadoEmpleado>> GetAllParaCambiarEstado(bool dadosDeBaja)
        {
            var result = new Result<List<Resultado_EstadoEmpleado>>();
            var resultConsulta = dao.GetAll(dadosDeBaja);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            var list = resultConsulta.Return.Where(z => z.KeyValue != Enums.EstadoEmpleado.OCUPADO && z.KeyValue != Enums.EstadoEmpleado.ENFLOTA).ToList();
            result.Return = Resultado_EstadoEmpleado.ToList(list);
            return result;
        }

        public Result<List<Resultado_EstadoEmpleado>> GetAllOcupados()
        {
            var result = new Result<List<Resultado_EstadoEmpleado>>();
            var resultConsulta = dao.GetAll(false);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            var list = resultConsulta.Return.Where(z => z.KeyValue == Enums.EstadoEmpleado.OCUPADO || z.KeyValue == Enums.EstadoEmpleado.ENFLOTA).ToList();
            result.Return = Resultado_EstadoEmpleado.ToList(list);
            return result;
        }

        public Result<List<Enums.EstadoEmpleado>> GetEstadosParaOT()
        {
            var result = new Result<List<Enums.EstadoEmpleado>>();
            var list = new List<Enums.EstadoEmpleado>();
            list.Add(Enums.EstadoEmpleado.DISPONIBLE);
            result.Return = list;
            return result;
        }

    }
}
