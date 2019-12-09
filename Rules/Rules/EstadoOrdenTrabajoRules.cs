using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class EstadoOrdenTrabajoRules : BaseRules<EstadoOrdenTrabajo>
    {
    
        private readonly EstadoOrdenTrabajoDAO dao;

        public EstadoOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoOrdenTrabajoDAO.Instance;
        }

        public Result<EstadoOrdenTrabajo> GetByKeyValue(Enums.EstadoOrdenTrabajo estado)
        {
            return dao.GetByKeyValue(estado);
        }

        public List<Enums.EstadoOrdenTrabajo> GetEstadosValidosParaEdicion_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenTrabajo>> GetEstadosValidosParaEdicion()
        {
            return ObtenerEstados(GetEstadosValidosParaEdicion_KeyValue());
        }

        public List<Enums.EstadoOrdenTrabajo> GetEstadosValidosParaCerrar_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenTrabajo>> GetEstadosValidosParaCerrar()
        {
            return ObtenerEstados(GetEstadosValidosParaCerrar_KeyValue());
        }

        public List<Enums.EstadoOrdenTrabajo> GetEstadosValidosParaCancelar_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenTrabajo>> GetEstadosValidosParaCancelar()
        {
            return ObtenerEstados(GetEstadosValidosParaCancelar_KeyValue());
        }

        public List<Enums.EstadoOrdenTrabajo> GetEstadosPorDefectoMisTrabajos_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenTrabajo>> GetEstadosPorDefectoMisTrabajos()
        {
            return ObtenerEstados(GetEstadosPorDefectoMisTrabajos_KeyValue());
        }

        public List<Enums.EstadoOrdenTrabajo> GetEstadosValidosParaCrear_KeyValue()
        {
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.NUEVO);
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            return estados;
        }

        public Result<List<EstadoOrdenTrabajo>> GetEstadosValidosParaCrear()
        {
            return ObtenerEstados(GetEstadosValidosParaCrear_KeyValue());
        }

        private Result<List<EstadoOrdenTrabajo>> ObtenerEstados(List<Enums.EstadoOrdenTrabajo> keyValues)
        {
            var result = new Result<List<EstadoOrdenTrabajo>>();

            var estados = new List<EstadoOrdenTrabajo>();
            foreach (Enums.EstadoOrdenTrabajo keyValue in keyValues)
            {
                var resultEstado = GetByKeyValue(keyValue);
                if (!resultEstado.Ok)
                {
                    result.Copy(resultEstado.Errores);
                    return result;
                }

                estados.Add(resultEstado.Return);
            }
            result.Return = estados.ToList();
            return result;
        }



    }
}
