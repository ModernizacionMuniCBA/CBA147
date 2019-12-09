using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoOrdenEspecialRules : BaseRules<EstadoOrdenEspecial>
    {

        private readonly EstadoOrdenEspecialDAO dao;

        public EstadoOrdenEspecialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoOrdenEspecialDAO.Instance;
        }

        public Result<EstadoOrdenEspecial> GetByKeyValue(Enums.EstadoOrdenEspecial estado)
        {
            return dao.GetByKeyValue(estado);
        }

        private Result<List<EstadoOrdenEspecial>> ObtenerEstados(List<Enums.EstadoOrdenEspecial> keyValues)
        {
            var result = new Result<List<EstadoOrdenEspecial>>();

            var estados = new List<EstadoOrdenEspecial>();
            foreach (Enums.EstadoOrdenEspecial keyValue in keyValues)
            {
                var resultEstado = GetByKeyValue(keyValue);
                if (!resultEstado.Ok)
                {
                    result.Copy(resultEstado.Errores);
                    return result;
                }

                estados.Add(resultEstado.Return);
            }
            result.Return = estados;
            return result;
        }

        public List<Enums.EstadoOrdenEspecial> GetEstadosValidosParaEdicion_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenEspecial>();
            estados.Add(Enums.EstadoOrdenEspecial.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenEspecial>> GetEstadosValidosParaEdicion()
        {
            return ObtenerEstados(GetEstadosValidosParaEdicion_KeyValue());
        }

        public List<Enums.EstadoOrdenEspecial> GetEstadosValidosParaCompletar_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenEspecial>();
            estados.Add(Enums.EstadoOrdenEspecial.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenEspecial>> GetEstadosValidosParaCompletar()
        {
            return ObtenerEstados(GetEstadosValidosParaCompletar_KeyValue());
        }

    }
}
