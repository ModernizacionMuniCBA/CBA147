
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DAO.DAO;
using Model;
using Model.Entities;
using System.Net.Mail;
using Rules.Rules.Reportes;
using Telerik.Reporting.Processing;
using System.IO;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;


namespace Rules.Rules
{
    public class OrdenEspecialRules : BaseRules<OrdenAtencionCritica>
    {

        private readonly OrdenEspecialDAO dao;

        public OrdenEspecialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrdenEspecialDAO.Instance;
        }

        public Result<Resultado_OrdenAtencionCritica> Insert(Comando_OrdenAtencionCritica comandoOrden)
        {
            var result = new Result<Resultado_OrdenAtencionCritica>();
            int id = 0;
            var rqXOE = new RequerimientoPorOrdenEspecial();

            var resultadoOT = dao.Transaction(() =>
            {

                var resultadoRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetById(comandoOrden.IdRequerimiento);
                if (!resultadoRequerimiento.Ok || resultadoRequerimiento.Return == null)
                {
                    result.Copy(resultadoRequerimiento.Errores);
                    return false;
                }

                //valido que el rq no tenga una orden especial
                if (resultadoRequerimiento.Return.OrdenesEspeciales != null && resultadoRequerimiento.Return.OrdenesEspeciales.Count != 0)
                {
                    result.AddErrorPublico("Éste requerimiento ya posee una orden de atención crítica.");
                    return false;
                }

                var orden = new OrdenAtencionCritica();
                orden.Descripcion = comandoOrden.Descripcion;
                orden.UsuarioCreador = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
                var resultInsert = base.Insert(orden);

                //inserto la orden especial
                if (!resultInsert.Ok || resultInsert.Return == null)
                {
                    result.AddErrorPublico("Error al generar la orden especial.");
                    result.AddErrorInterno("Error en la inserción de la orden especial.");
                    return false;
                }

                //Le pongo estado Nuevo
                var resultEstado = CambiarEstado(orden, Enums.EstadoOrdenEspecial.ENPROCESO, "Creado", true);
                if (!resultEstado.Ok)
                {
                    result.SetErrorInterno(resultEstado.MessagesInternos);
                    return false;
                }

                //inserto la relaicon

                rqXOE.OrdenEspecial = resultInsert.Return;
                rqXOE.Requerimiento = resultadoRequerimiento.Return;
                var resultrXOE = new RequerimientoPorOrdenEspecialRules(getUsuarioLogueado()).Insert(rqXOE);

                if (!resultrXOE.Ok || resultrXOE.Return == null)
                {
                    result.AddErrorPublico("Error al generar la orden de atención crítica.");
                    result.AddErrorInterno("Error en la inserción de la fila de relacion entre rq y orden especial.");
                    return false;
                }

                id = resultInsert.Return.Id;
                rqXOE = resultrXOE.Return;
                return true;
            });


            if (!resultadoOT)
            {
                result.AddErrorPublico("Error procesando la solicitud");
                return result;
            }

            var resultConsulta = base.GetById(id);

            var list = new List<RequerimientoPorOrdenEspecial>();
            list.Add(rqXOE);
            resultConsulta.Return.RequerimientosPorOrdenEspecial = list;
            result.Return = new Resultado_OrdenAtencionCritica(resultConsulta.Return);

            return result;
        }

        public Result<Resultado_OrdenAtencionCritica> Update(Comando_OrdenAtencionCritica comandoOrden)
        {
            var result = new Result<Resultado_OrdenAtencionCritica>();
            int id = 0;

            var resultadoOT = dao.Transaction(() =>
            {
                var resultadoOrden = new OrdenEspecialRules(getUsuarioLogueado()).GetById((int)comandoOrden.Id);
                if (!resultadoOrden.Ok || resultadoOrden.Return == null)
                {
                    result.Copy(resultadoOrden.Errores);
                    return false;
                }

                var orden = resultadoOrden.Return;
                var resultValidate=ValidateUpdate(orden);
                if (!resultValidate.Ok)
                {
                    result.Copy(resultValidate.Errores);
                    return false;
                }

                orden.Descripcion = comandoOrden.Descripcion;

                var resultUpdate = base.Update(orden);

                //actualizo la orden especial
                if (!resultUpdate.Ok || resultUpdate.Return == null)
                {
                    result.AddErrorPublico("Error al generar la orden especial.");
                    result.AddErrorInterno("Error en la inserción de la orden especial.");
                    return false;
                }

                id = resultUpdate.Return.Id;

                return true;
            });


            var o = base.GetById(id);
            result.Return = new Resultado_OrdenAtencionCritica(o.Return);

            if (!resultadoOT)
            {
                result.AddErrorPublico("Error procesando la solicitud");
            }

            return result;
        }

        public override Result<OrdenAtencionCritica> ValidateUpdate(OrdenAtencionCritica entity)
        {
            var result = new Result<OrdenAtencionCritica>();
            if (entity.GetUltimoEstado().Estado.KeyValue != Enums.EstadoOrdenEspecial.ENPROCESO)
            {
                result.AddErrorPublico("La orden de atención crítica no puede ser editada por encontrarse en estado: " + entity.GetUltimoEstado().Estado.Nombre);
                return result;
            }

            return base.ValidateUpdate(entity);
        }

        /* Estados */
        public Result<EstadoOrdenEspecialHistorial> CrearEstado(OrdenAtencionCritica orden, Enums.EstadoOrdenEspecial e, string observaciones)
        {
            var result = new Result<EstadoOrdenEspecialHistorial>();
            var estadoOrdenEspecialRules = new EstadoOrdenEspecialRules(getUsuarioLogueado());
            var resultEstado = estadoOrdenEspecialRules.GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                return result;
            }

            var estado = resultEstado.Return;

            var estadoRQ = new EstadoOrdenEspecialHistorial();
            estadoRQ.Fecha = DateTime.Now;
            estadoRQ.FechaAlta = DateTime.Now;
            estadoRQ.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoRQ.Estado = estado;
            estadoRQ.Observaciones = observaciones;
            estadoRQ.OrdenEspecial = orden;

            result.Return = estadoRQ;
            return result;
        }

        public Result<OrdenAtencionCritica> CambiarEstado(OrdenAtencionCritica orden, Enums.EstadoOrdenEspecial estado, string observaciones, bool guardarCambios)
        {
            var estadoorden = CrearEstado(orden, estado, observaciones);
            //Estados
            List<Enums.EstadoOrdenEspecial> keyValuesEstados = new List<Enums.EstadoOrdenEspecial>();

            keyValuesEstados.Add(Enums.EstadoOrdenEspecial.ENPROCESO);
            keyValuesEstados.Add(Enums.EstadoOrdenEspecial.COMPLETADO);

            var result = new Result<OrdenAtencionCritica>();

            if (orden.Estados == null)
            {
                orden.Estados = new List<EstadoOrdenEspecialHistorial>();
            }

            if (orden.Estados != null && orden.Estados.Count != 0)
            {
                foreach (EstadoOrdenEspecialHistorial e in orden.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estadoorden.Return.Ultimo = true;
            orden.Estados.Add(estadoorden.Return);

            if (guardarCambios)
            {
                 var  resultUpdate = dao.Update(orden);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }
            result.Return = orden;

            return result;
        }

        public Result<Resultado_OrdenAtencionCritica> Completar(Comando_OrdenAtencionCritica comandoOrden)
        {
            var result = new Result<Resultado_OrdenAtencionCritica>();
            var resultOrden=GetById((int)comandoOrden.Id);

            if(!resultOrden.Ok){
                result.Copy(resultOrden.Errores);
                return result;
            }

            var resultEstados = new EstadoOrdenEspecialRules(getUsuarioLogueado()).GetEstadosValidosParaCompletar();
            if(!resultEstados.Ok){
                result.Copy(resultEstados.Errores);
                return result;
            }

            var orden=resultOrden.Return;
            if (resultEstados.Return.Where(z => z.Id == orden.GetUltimoEstado().Estado.Id) == null)
            {
                result.AddErrorPublico("La orden de atención crítica no se puede completar, ya que se encuentra en estado: " + orden.GetUltimoEstado().Estado.Nombre);
                return result;
            }

            var resultValidarUpdate = ValidateUpdate(orden);
            if (!resultValidarUpdate.Ok)
            {
                result.Copy(resultValidarUpdate.Errores);
                result.AddErrorPublico("Error al cambiar el estado");
                return result;
            }

           var resultCambiarEstado= CambiarEstado(orden, Enums.EstadoOrdenEspecial.COMPLETADO, comandoOrden.Descripcion, true);
           if (!resultCambiarEstado.Ok)
            {
                result.AddErrorPublico("Error al cambiar el estado");
                return result;
            }

           result.Return = new Resultado_OrdenAtencionCritica(resultCambiarEstado.Return);
            return result;
        }

        /*Datos Tabla*/
        public Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>> GetResultadoTabla(Consulta_OrdenAtencionCritica consulta)
        {
            return dao.GetResultadoTabla(2000, consulta);
        }

        public Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>> GetResultadoTabla(List<int> ids)
        {
            return dao.GetResultadoTabla(2000, ids);
        }

    }
}
