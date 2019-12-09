using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;

namespace Rules.Rules
{
    public class EstadoRequerimientoRules : BaseRules<EstadoRequerimiento>
    {

        private readonly EstadoRequerimientoDAO dao;

        public EstadoRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoRequerimientoDAO.Instance;
        }

        public Result<EstadoRequerimiento> GetByKeyValue(Enums.EstadoRequerimiento estado)
        {
            return dao.GetByKeyValue(estado);
        }

        #region Listado de Estados anteriores. Se reemplazo esta funcion con los PERMISOS_ESTADO_REQUERIMIENTO

        /* ----- ESTADOS RQ -----*/

        ////Estados Finales
        //public Result<List<EstadoRequerimiento>> GetEstadosFinales()
        //{
        //    return ObtenerEstados(GetEstadosFinales_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosFinales_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CERRADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    return estadosKeyValue;
        //}


        ////Eatados Con Proceso
        //public Result<List<EstadoRequerimiento>> GetEstadosConProceso()
        //{
        //    return ObtenerEstados(GetEstadosConProceso_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosConProceso_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.ENPROCESO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CERRADO);
        //    //estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Cambiar Estado RQ
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaCambiarEstado()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaCambiarEstado_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaCambiarEstado_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);

        //    return estadosKeyValue;
        //}


        ////Estados Para Cancelar RQ
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaCancelar()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaCancelar_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaCancelar_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Editar RQ
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaEditar()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaEditar_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaEditar_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    //estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Editar Prioridad
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaEditarPrioridad()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaEditarPrioridad_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaEditarPrioridad_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.ENPROCESO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Marcar favorito
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaMarcarFavorito()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaMarcarFavorito_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaMarcarFavorito_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.ENPROCESO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CERRADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Marcar
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaMarcar()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaMarcar_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaMarcar_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    return estadosKeyValue;
        //}


        ////Estados Bandeja Urgentes
        //public Result<List<EstadoRequerimiento>> GetEstadosBandejaUrgentes()
        //{
        //    return ObtenerEstados(GetEstadosBandejaUrgentes_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosBandejaUrgentes_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    return estadosKeyValue;
        //}


        ////Estados Para Unirse a OT
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaUnirseOT()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaUnirseOT_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaUnirseOT_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    return estadosKeyValue;
        //}


        ////Estados Para Cerrar OT
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaCerrarOT()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaCerrarOT_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaCerrarOT_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.COMPLETADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.CANCELADO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    return estadosKeyValue;
        //}


        ////Estados Para armar OAC
        //public Result<List<EstadoRequerimiento>> GetEstadosValidosParaArmarOAC()
        //{
        //    return ObtenerEstados(GetEstadosValidosParaArmarOAC_KeyValue());
        //}
        //public List<Enums.EstadoRequerimiento> GetEstadosValidosParaArmarOAC_KeyValue()
        //{
        //    var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INCOMPLETO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.SUSPENDIDO);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estadosKeyValue.Add(Enums.EstadoRequerimiento.ENPROCESO);
        //    return estadosKeyValue;
        //}
        #endregion

        public Result<List<EstadoRequerimiento>> ObtenerEstados(List<Enums.EstadoRequerimiento> keyValues)
        {
            var result = new Result<List<EstadoRequerimiento>>();

            var estados = new List<EstadoRequerimiento>();
            foreach (Enums.EstadoRequerimiento keyValue in keyValues)
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

        public Result<List<Resultado_EstadoRequerimiento>> GetEstadosAlEntrarAOT()
        {
            var resultado = new Result<List<Resultado_EstadoRequerimiento>>();

            var resultConsulta = ObtenerEstados(new List<Enums.EstadoRequerimiento>() { Enums.EstadoRequerimiento.ENPROCESO, Enums.EstadoRequerimiento.INSPECCION });
            if (!resultConsulta.Ok)
            {
                resultado.Copy(resultConsulta.Errores);
                return resultado;
            }

            resultado.Return = Resultado_EstadoRequerimiento.ToList(resultConsulta.Return);
            return resultado;
        }
    }
}
