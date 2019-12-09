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
    public class OrdenInspeccionRules : BaseRules<OrdenInspeccion>
    {

        private readonly OrdenInspeccionDAO dao;

        public OrdenInspeccionRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrdenInspeccionDAO.Instance;
        }


        private static Random random = new Random();


        public Result<Resultado_OrdenInspeccionInit> Init(List<int> idsRequerimientos)
        {
            var resultado = new Result<Resultado_OrdenInspeccionInit>();

            //Busco los rqs
            var rqs = new List<Resultado_Requerimiento>();

            var rules = new RequerimientoRules(getUsuarioLogueado());
            idsRequerimientos = idsRequerimientos.Distinct().ToList();
            foreach (int id in idsRequerimientos)
            {
                var resultadoRequerimiento = rules.GetById(id);
                if (!resultadoRequerimiento.Ok)
                {
                    resultado.Copy(resultadoRequerimiento.Errores);
                    return resultado;
                }

                var soyArea = getUsuarioLogueado().Ambito == null || getUsuarioLogueado().Ambito.KeyValue == 0;
                //valido que el rq este en control del area o cpc (segun quien este haciendo la orden)
                if ((resultadoRequerimiento.Return.Marcado && soyArea) || (!resultadoRequerimiento.Return.Marcado && !soyArea))
                {
                    resultado.AddErrorPublico("Uno de los requerimientos no se encuentra en su control.");
                    return resultado;
                }

                if (resultadoRequerimiento.Return == null)
                {
                    resultado.AddErrorPublico("Un requerimiento seleccionado no existe");
                    return resultado;
                }

                if (resultadoRequerimiento.Return.OrdenTrabajoActiva != null)
                {
                    resultado.AddErrorPublico("Uno de los requerimientos seleccionados está en una orden de trabajo, por lo que no puede formar parte de una orden de inspección");
                    return resultado;
                }

                rqs.Add(new Resultado_Requerimiento(resultadoRequerimiento.Return));
            }

            var resultadoOrdenInspeccion = new Resultado_OrdenInspeccionInit()
           {
               Requerimientos = rqs
           };

            resultado.Return = resultadoOrdenInspeccion;
            return resultado;
        }

        public override Result<OrdenInspeccion> Insert(OrdenInspeccion entity)
        {
            throw new Exception("Usar el otro metodo!");
        }

        public Result<OrdenInspeccion> Insert(Comando_OrdenInspeccion comando)
        {
            var result = new Result<OrdenInspeccion>();
            var entity = new OrdenInspeccion();

            dao.Transaction(() =>
            {
                //Seteo los datos del numero
                var numero = GenerarNumeroIdentificatorio();
                entity.Marcado = getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0;
                entity.UserAgent = comando.UserAgent;
                entity.TipoDispositivo = comando.TipoDispositivo;
                entity.Año = numero.Año;
                entity.Numero = numero.Numero;
                entity.Descripcion = comando.Descripcion;
                entity.FechaCreacion = DateTime.Now;
                entity.UsuarioCreador = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetByIdObligatorio(getUsuarioLogueado().Usuario.Id).Return;

                //Cambio a Estado en Proceso
                var resultCambiarEstado = CambiarEstado(entity, Enums.EstadoOrdenInspeccion.ENPROCESO, "Orden de Inspección creada", false);
                if (!resultCambiarEstado.Ok)
                {
                    result.Copy(resultCambiarEstado);
                    return false;
                }

                //-----------------------------------
                // Requerimientos
                //-----------------------------------

                entity.RequerimientosPorOrdenInspeccion = new List<RequerimientoPorOrdenInspeccion>();

                //Valido que tenga requerimientos
                if (comando.IdRequerimientos == null || comando.IdRequerimientos.Count == 0)
                {
                    result.AddErrorPublico("La orden de Inspección debe contener requerimientos");
                    return false;
                }

                var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
                foreach (var idRq in comando.IdRequerimientos)
                {
                    var rqResult = requerimientoRules.GetByIdObligatorio(idRq);
                    if (!rqResult.Ok)
                    {
                        result.AddErrorPublico(rqResult.Errores.ErroresPublicos);
                        return false;
                    }

                    var rq = rqResult.Return;

                    var soyArea = getUsuarioLogueado().Ambito == null || getUsuarioLogueado().Ambito.KeyValue == 0;
                    //valido que el rq este en control del area o cpc (segun quien este haciendo la orden)
                    if ((rq.Marcado && soyArea) || (!rq.Marcado && !soyArea))
                    {
                        result.AddErrorPublico("Uno de los requerimientos no se encuentra en su control.");
                        return false;
                    }

                    //Valido que no debe estar en otra OT
                    if (rq.OrdenInspeccionActiva != null)
                    {
                        result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " se encuentra en la Orden de Inspección N° " + rq.OrdenInspeccionActiva.GetNumero());
                        return false;
                    }

                    //Valido que este en un estado valido para formar parte de la OT
                    var keyValueEstado = rq.GetUltimoEstado().Estado.KeyValue;
                    var permiso = Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeInspeccion;
                    var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
                    if (!resultadoPermiso.Ok)
                    {
                        result.Copy(resultadoPermiso.Errores);
                        return false;
                    }
                    if (!resultadoPermiso.Return)
                    {
                        result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " no se encuentra en un estado valido para formar parte de la orden de inspección.");
                        return false;
                    }
                }

                //---------------------------------------
                // Notas
                //---------------------------------------
                //var notaOrdenInspeccionRules = new NotaPorOrdenInspeccionRules(getUsuarioLogueado());
                //if (entity.Notas != null && entity.Notas.Count != 0)
                //{
                //    foreach (NotaPorOrdenInspeccion nota in entity.Notas)
                //    {
                //        Result<NotaPorOrdenInspeccion> resultNota;
                //        if (nota.Id == 0)
                //        {
                //            resultNota = notaOrdenInspeccionRules.ValidateInsert(nota);
                //        }
                //        else
                //        {
                //            resultNota = notaOrdenInspeccionRules.ValidateUpdate(nota);
                //        }

                //        if (!resultNota.Ok)
                //        {
                //            result.Copy(resultNota.Errores);
                //            return false;
                //        }
                //    }
                //}


                //Inserto
                result = base.Insert(entity);
                if (!result.Ok)
                {
                    return false;
                }


                //Una vez insertado, le seteo al Requerimiento la orden de inspeccion insertada (como su OI Actual).
                //Debe ser si o si LUEGO de insertarse el objeto.
                //Ademas le cambio el estado al Reclamo, indicando que fue agregado a la OT. 
                //Tambien se llama luego de insertar, porque en la observacion del cambio de estado
                //estoy agregando el numero de la OT.
                var requerimientoXOrdenInspeccionRules = new RequerimientoPorOrdenInspeccionRules(getUsuarioLogueado());
                foreach (var idRq in comando.IdRequerimientos)
                {
                    var rq = requerimientoRules.GetByIdObligatorio(idRq).Return;

                    //Genero el objeto RequerimientoXOrdenInspeccion
                    var rqxot = new RequerimientoPorOrdenInspeccion();
                    rqxot.Requerimiento = rq;
                    rqxot.OrdenInspeccion = result.Return;

                    //Inserto el RequerimientoXOrdenInspeccion
                    var resultInsertRQ = requerimientoXOrdenInspeccionRules.Insert(rqxot);
                    if (!resultInsertRQ.Ok)
                    {
                        result.Copy(resultInsertRQ.Errores);
                        return false;
                    }

                    //Seteo la OI activa
                    rq.OrdenInspeccionActiva = entity;

                    //Cambio el estado
                    var resultCambiarEstadoRQ = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.INSPECCION, "En Orden de Inspección N° " + result.Return.GetNumero(), false);
                    if (!resultCambiarEstadoRQ.Ok)
                    {
                        result.Copy(resultCambiarEstadoRQ.Errores);
                        return false;
                    }

                    //Actualizo el RQ
                    var resultUpdateRQ = requerimientoRules.ValidateUpdate(rq);
                    if (!resultUpdateRQ.Ok)
                    {
                        result.Copy(resultUpdateRQ.Errores);
                        return false;
                    }

                    resultUpdateRQ = RequerimientoDAO.Instance.Update(rq);
                    if (!resultUpdateRQ.Ok)
                    {
                        result.Copy(resultUpdateRQ.Errores);
                        return false;
                    }
                }

                return result.Ok;
            });

            return result;
        }

        //public override Result<OrdenInspeccion> Update(OrdenInspeccion entity)
        //{
        //    throw new Exception("No se usa este método");
        //}

        ////public Result<OrdenInspeccion> Update(Comando_OrdenInspeccion comando)
        ////{
        ////    var result = new Result<OrdenInspeccion>();


        ////    if (!comando.Id.HasValue)
        ////    {
        ////        result.AddErrorInterno("No viene un id para ediar");
        ////        result.AddErrorPublico("Error al actualizar la orden de trabajo.");
        ////        return result;
        ////    }

        ////    var resulOrdenInspeccion = GetById((int)comando.Id);
        ////    if (!resulOrdenInspeccion.Ok)
        ////    {
        ////        result.Copy(resulOrdenInspeccion.Errores);
        ////        return result;
        ////    }

        ////    var ordenBD = resulOrdenInspeccion.Return;
        ////    ordenBD.Descripcion = comando.Descripcion;

        ////    //Notas
        ////    var notas = new List<NotaPorOrdenInspeccion>();
        ////    foreach (Comando_NotaOrdenInspeccion comando_nota in comando.Notas)
        ////    {
        ////        NotaPorOrdenInspeccion nota;
        ////        if (comando_nota.Id.HasValue && comando_nota.Id != 0)
        ////        {
        ////            var resultConsultaNota = new NotaPorOrdenInspeccionRules(getUsuarioLogueado()).GetById((int)comando_nota.Id);
        ////            if (!resultConsultaNota.Ok)
        ////            {
        ////                result.Copy(resultConsultaNota.Errores);
        ////                return result;
        ////            }

        ////            nota = resultConsultaNota.Return;
        ////        }
        ////        else
        ////        {
        ////            nota = new NotaPorOrdenInspeccion();
        ////            nota.Observaciones = comando_nota.Observaciones;
        ////            nota.FechaAlta = DateTime.Now;
        ////        }
        ////        nota.OrdenInspeccion = ordenBD;
        ////        notas.Add(nota);
        ////    }
        ////    ordenBD.Notas = notas;


        ////    //Consulto los rqs anteriores
        ////    List<Requerimiento> rqsAnteriores = null;
        ////    var resultOtraSesion = dao.EjecutarEnOtraSession(() =>
        ////    {
        ////        var resultOt = GetById((int)comando.Id);
        ////        if (!resultOt.Ok)
        ////        {
        ////            result.Copy(resultOt);
        ////            return false;
        ////        }
        ////        var ot = resultOt.Return;

        ////        rqsAnteriores = ot.Requerimientos();
        ////        return true;
        ////    });

        ////    if (!resultOtraSesion)
        ////    {
        ////        result.AddErrorInterno("Error editando");
        ////        return result;
        ////    }


        ////    //Consulta las notas anteriores
        ////    List<NotaPorOrdenInspeccion> notasAnteriores = null;
        ////    resultOtraSesion = dao.EjecutarEnOtraSession(() =>
        ////    {
        ////        var resultNotas = new NotaPorOrdenInspeccionRules(getUsuarioLogueado()).GetByFilters((int)comando.Id, false);
        ////        if (!resultNotas.Ok)
        ////        {
        ////            result.Copy(resultNotas.Errores);
        ////            return false;
        ////        }
        ////        notasAnteriores = resultNotas.Return;
        ////        return true;
        ////    });

        ////    if (!resultOtraSesion)
        ////    {
        ////        result.AddErrorInterno("Error editando");
        ////        return result;
        ////    }

        ////    //Consulta los moviles anteriores
        ////    List<MovilPorOrdenInspeccion> movilesAnteriores = null;
        ////    resultOtraSesion = dao.EjecutarEnOtraSession(() =>
        ////    {
        ////        var resultMoviles = new MovilPorOrdenInspeccionRules(getUsuarioLogueado()).GetByIdOrdenInspeccion((int)comando.Id, false);
        ////        if (!resultMoviles.Ok)
        ////        {
        ////            result.Copy(resultMoviles.Errores);
        ////            return false;
        ////        }
        ////        movilesAnteriores = resultMoviles.Return;
        ////        return true;
        ////    });

        ////    if (!resultOtraSesion)
        ////    {
        ////        result.AddErrorInterno("Error editando");
        ////        return result;
        ////    }

        ////    dao.Transaction(() =>
        ////    {
        ////        //Valido que la OT este en un estado valido
        ////        var estadoOT = ordenBD.GetUltimoEstado();

        ////        //Busco el estado proximo que van a tener los requerimientos de la OT (el estado se busca segun el estado de la OT)
        ////        Enums.EstadoRequerimiento estado = Enums.EstadoRequerimiento.ENPROCESO;
        ////        switch (estadoOT.Estado.KeyValue)
        ////        {
        ////            case Enums.EstadoOrdenInspeccion.ENPROCESO:
        ////                estado = Enums.EstadoRequerimiento.ENPROCESO;
        ////                break;
        ////            case Enums.EstadoOrdenInspeccion.SUSPENDIDO:
        ////                estado = Enums.EstadoRequerimiento.SUSPENDIDO;
        ////                break;
        ////            case Enums.EstadoOrdenInspeccion.COMPLETADO:
        ////                {
        ////                    result.AddErrorPublico("La Orden de trabajo esta en un estado no valido para la edicion");
        ////                    return false;
        ////                }
        ////            case Enums.EstadoOrdenInspeccion.CANCELADO:
        ////                {
        ////                    result.AddErrorPublico("La Orden de trabajo esta en un estado no valido para la edicion");
        ////                    return false;
        ////                }
        ////        }

        ////        //-----------------------------------
        ////        // Requerimientos
        ////        //-----------------------------------

        ////        //Valido que tenga requerimientos
        ////        if (comando.Requerimientos == null || comando.Requerimientos.Count == 0)
        ////        {
        ////            result.AddErrorPublico("La orden de Trabajo debe contener requerimientos");
        ////            return false;
        ////        }

        ////        var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
        ////        var requerimientoXOrdenInspeccionRules = new RequerimientoPorOrdenInspeccionRules(getUsuarioLogueado());
        ////        var notaPorRequerimientoRules = new NotaPorRequerimientoRules(getUsuarioLogueado());
        ////        var notaOrdenInspeccionRules = new NotaPorOrdenInspeccionRules(getUsuarioLogueado());
        ////        var movilOrdenInspeccionRules = new MovilPorOrdenInspeccionRules(getUsuarioLogueado());
        ////        var recursoPorOrdenInspeccionRules = new RecursoPorOrdenInspeccionRules(getUsuarioLogueado());
        ////        var seccionRules = new SeccionRules(getUsuarioLogueado());

        ////        //Seccion
        ////        if (comando.IdSeccion.HasValue)
        ////        {
        ////            var resultSeccion = seccionRules.GetById((int)comando.IdSeccion);
        ////            if (!resultSeccion.Ok)
        ////            {
        ////                result.AddErrorPublico("Error al actualizar la orden de trabajo");
        ////                result.AddErrorInterno("Error al consutlar la seccion");
        ////                return false;
        ////            }
        ////            ordenBD.Seccion = resultSeccion.Return;
        ////        }
        ////        else
        ////        {
        ////            ordenBD.Seccion = null;
        ////        }

        ////        //Itero los requerimientos que formaran parte de la OT
        ////        var idsNuevos = new List<int>();
        ////        foreach (var rqXot in comando.Requerimientos)
        ////        {
        ////            var idRq = rqXot.Id;
        ////            idsNuevos.Add(idRq);

        ////            var consultaRQ = requerimientoRules.GetById(idRq);
        ////            if (!consultaRQ.Ok)
        ////            {
        ////                result.Copy(consultaRQ.Errores);
        ////                return false;
        ////            }

        ////            var rq = consultaRQ.Return;
        ////            if (rq == null)
        ////            {
        ////                result.AddErrorPublico("El requerimiento no existe");
        ////                return false;
        ////            }

        ////            //Valido que el area del Reclamo sea la misma que la de la OT
        ////            if (rq.AreaResponsable.Id != ordenBD.Area.Id)
        ////            {
        ////                result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " no es del mismo area que la orden de trabajo");
        ////                return false;
        ////            }

        ////            ////Valido que este en un estado valido para formar parte de la OT
        ////            //var keyValueEstado = rq.GetUltimoEstado().Estado.KeyValue;
        ////            //var permiso = Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo;
        ////            //var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
        ////            //if (!resultadoPermiso.Ok)
        ////            //{
        ////            //    result.Copy(resultadoPermiso.Errores);
        ////            //    return false;
        ////            //}
        ////            //if (!resultadoPermiso.Return)
        ////            //{
        ////            //    result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " no se encuentra en un estado valido para formar parte de la orden de trabajo.");
        ////            //    return false;
        ////            //}

        ////            //Cambio el estado, si es q no esta en ese estado ya
        ////            if (rq.GetUltimoEstado().Estado.KeyValue != estado)
        ////            {
        ////                var resultEstadoRQ = requerimientoRules.ProcesarCambioEstado(rq, estado, "Agregado a Orden Trabajo N° " + ordenBD.GetNumero(), false);
        ////                if (!resultEstadoRQ.Ok)
        ////                {
        ////                    result.Copy(resultEstadoRQ.Errores);
        ////                    return false;
        ////                }

        ////                //Actualizo el RQ
        ////                var resultUpdateRq = requerimientoRules.ValidateUpdate(rq);
        ////                if (!resultUpdateRq.Ok)
        ////                {
        ////                    result.Copy(resultUpdateRq.Errores);
        ////                    return false;
        ////                }

        ////                //Mando a actualizar directamente al DAO porque el update del Requerimiento tiene unas validaciones
        ////                //sobre el estado del RQ, que no aplican para este caso
        ////                resultUpdateRq = RequerimientoDAO.Instance.Update(rq);
        ////                if (!resultUpdateRq.Ok)
        ////                {
        ////                    result.Copy(resultUpdateRq.Errores);
        ////                    return false;
        ////                }
        ////            }

        ////            //Solo  si la OT no contiene al RQ, lo agrego.
        ////            //Si ya lo contiene, no debo hacer nada
        ////            if (!ordenBD.ContieneRequerimiento(idRq))
        ////            {

        ////                //Puede ser que alguna vez el RQ formo parte de la OT, es decir, que este dado de baja
        ////                //Si es el caso, debo darlo de alta nuevamente
        ////                var yaEsInactivo = -1;
        ////                var rqsInactivos = ordenBD.RequerimientosPorOrdenInspeccion.Where(x => x.FechaBaja != null).ToList();
        ////                foreach (var rqOT in rqsInactivos)
        ////                {
        ////                    if (rqOT.Requerimiento.Id == rq.Id)
        ////                    {
        ////                        yaEsInactivo = rqOT.Id;
        ////                    }
        ////                }

        ////                RequerimientoPorOrdenInspeccion rqxot = new RequerimientoPorOrdenInspeccion();
        ////                if (yaEsInactivo != -1)
        ////                {
        ////                    rqxot = requerimientoXOrdenInspeccionRules.GetById(yaEsInactivo).Return;
        ////                    rqxot.FechaBaja = null;
        ////                }
        ////                rqxot.Requerimiento = rq;
        ////                rqxot.OrdenInspeccion = ordenBD;

        ////                Result<RequerimientoPorOrdenInspeccion> resultRqot;
        ////                if (yaEsInactivo != -1)
        ////                {
        ////                    //LLamo directamente al DAO asi no em agregue fecha de modificacion
        ////                    resultRqot = RequerimientoPorOrdenInspeccionDAO.Instance.Update(rqxot);
        ////                }
        ////                else
        ////                {
        ////                    resultRqot = requerimientoXOrdenInspeccionRules.Insert(rqxot);
        ////                }
        ////                if (!resultRqot.Ok)
        ////                {
        ////                    result.Copy(resultRqot.Errores);
        ////                    return false;
        ////                }
        ////            }

        ////            // Notas del RQ
        ////            var comandoNotas = rqXot.Notas;
        ////            var idsNotasNuevas = new List<int>();
        ////            foreach (var n in comandoNotas)
        ////            {

        ////                var idNota = n.Id;
        ////                //idsNotasNuevas.Add(idNota);
        ////                if (idNota == null || idNota == 0)
        ////                {
        ////                    var nota = new NotaPorRequerimiento();
        ////                    nota.Requerimiento = rq;
        ////                    nota.OrdenInspeccion = ordenBD;
        ////                    nota.Observaciones = n.Contenido;

        ////                    var resultInsert = notaPorRequerimientoRules.Insert(nota);
        ////                    if (!resultInsert.Ok)
        ////                    {
        ////                        result.Copy(resultInsert.Errores);
        ////                        return false;
        ////                    }
        ////                }
        ////            }
        ////        }


        ////        //Borrar rq que no estan mas
        ////        var idRequerimientosBorrados = new List<int>();
        ////        idRequerimientosBorrados = rqsAnteriores.Where(x => !idsNuevos.Contains(x.Id)).Select(x => x.Id).ToList();

        ////        //Itero los Requerimientos que fueron borrados de la OT
        ////        foreach (int idRqBorrado in idRequerimientosBorrados)
        ////        {
        ////            foreach (var rqxot in ordenBD.RequerimientosPorOrdenInspeccion)
        ////            {
        ////                var rq = rqxot.Requerimiento;

        ////                //si el requerimiento que estoy iterando, fue borrado...
        ////                if (idRqBorrado == rq.Id)
        ////                {
        ////                    //Le quito la orden de trabajo activa
        ////                    rq.OrdenInspeccionActiva = null;

        ////                    //Paso su estado a Incompleto
        ////                    var resultEstadoRq = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.INCOMPLETO, "Salio de la Orden de Trabajo N° " + ordenBD.GetNumero(), true);
        ////                    if (!resultEstadoRq.Ok)
        ////                    {
        ////                        result.Copy(resultEstadoRq.Errores);
        ////                        return false;
        ////                    }

        ////                    //Borro la fila de la tabla RequerimientoXOrdenInspeccion
        ////                    var resultDelete = requerimientoXOrdenInspeccionRules.DeleteById(rqxot.Id);
        ////                    if (!resultDelete.Ok)
        ////                    {
        ////                        result.Copy(resultDelete.Errores);
        ////                        return false;
        ////                    }
        ////                }
        ////            }
        ////        }

        ////        //---------------------------------------
        ////        // Notas
        ////        //---------------------------------------

        ////        if (ordenBD.Notas != null && ordenBD.Notas.Count != 0)
        ////        {
        ////            foreach (NotaPorOrdenInspeccion nota in ordenBD.Notas)
        ////            {
        ////                Result<NotaPorOrdenInspeccion> resultNota;
        ////                if (nota.Id == 0)
        ////                {
        ////                    resultNota = notaOrdenInspeccionRules.ValidateInsert(nota);
        ////                }
        ////                else
        ////                {
        ////                    resultNota = notaOrdenInspeccionRules.ValidateUpdate(nota);
        ////                }

        ////                //Inserto la Nota
        ////                if (!resultNota.Ok)
        ////                {
        ////                    result.Copy(resultNota.Errores);
        ////                    return false;
        ////                }

        ////            }
        ////        }

        ////        //Borro las notas que no van más
        ////        var idsNotaBorrar = notasAnteriores.Where(x => !ordenBD.Notas.Select(y => y.Id).Contains(x.Id)).Select(x => x.Id).ToList();
        ////        foreach (var idNotaBorrar in idsNotaBorrar)
        ////        {
        ////            var resultBorrar = notaOrdenInspeccionRules.DeleteById(idNotaBorrar);
        ////            if (!resultBorrar.Ok)
        ////            {
        ////                result.Copy(resultBorrar.Errores);
        ////                return false;
        ////            }
        ////        }

        ////        //---------------------------------------
        ////        //  Moviles
        ////        //---------------------------------------

        ////        var movilesComando = new List<Movil>();
        ////        //veo cuales estan en la orden
        ////        foreach (int idMovil in comando.Moviles)
        ////        {
        ////            var m = movilesAnteriores.Where(z => z.Movil.Id == idMovil).FirstOrDefault();
        ////            //si no está, doy de alta la relacion
        ////            if (m != null)
        ////            {
        ////                continue;
        ////            }

        ////            var resultConsultaMovil = new MovilRules(getUsuarioLogueado()).GetById(idMovil);
        ////            if (!resultConsultaMovil.Ok)
        ////            {
        ////                result.Copy(resultConsultaMovil.Errores);
        ////                return false;
        ////            }

        ////            var mxot = new MovilPorOrdenInspeccion();
        ////            mxot.Movil = resultConsultaMovil.Return;
        ////            mxot.OrdenInspeccion = ordenBD;

        ////            var resultInsertMovil = new MovilPorOrdenInspeccionRules(getUsuarioLogueado()).Insert(mxot);
        ////            if (!resultInsertMovil.Ok)
        ////            {
        ////                result.Copy(resultInsertMovil.Errores);
        ////                return false;
        ////            }
        ////        }

        ////        foreach (MovilPorOrdenInspeccion movxot in movilesAnteriores)
        ////        {
        ////            var esta = comando.Moviles.Where(z => z == movxot.Movil.Id).FirstOrDefault();
        ////            if (esta == 0)
        ////            {
        ////                var resultDeleteMovil = new MovilPorOrdenInspeccionRules(getUsuarioLogueado()).Delete(movxot);
        ////                if (!resultDeleteMovil.Ok)
        ////                {
        ////                    result.Copy(resultDeleteMovil.Errores);
        ////                    return false;
        ////                }
        ////            }
        ////        }

        ////        //Borro las notas que no van más
        ////        var idsMovilesBorrar = movilesAnteriores.Where(x => !ordenBD.MovilesPorOrdenInspeccion.Select(y => y.Id).Contains(x.Id)).Select(x => x.Id).ToList();
        ////        foreach (var idMovilBorrar in idsMovilesBorrar)
        ////        {
        ////            var resultBorrar = movilOrdenInspeccionRules.DeleteById(idMovilBorrar);
        ////            if (!resultBorrar.Ok)
        ////            {
        ////                result.Copy(resultBorrar.Errores);
        ////                return false;
        ////            }
        ////        }


        ////        //--------------------------------------
        ////        // Recurso
        ////        //--------------------------------------
        ////        var recurso = new RecursoPorOrdenInspeccion();
        ////        if (comando.Recursos == null)
        ////        {
        ////            var resultQuery = recursoPorOrdenInspeccionRules.GetByIdOrdenInspeccion(ordenBD.Id);
        ////            if (!resultQuery.Ok)
        ////            {
        ////                result.Copy(resultQuery.Errores);
        ////                return false;
        ////            }

        ////            if (resultQuery.Return != null && resultQuery.Return.Count != 0)
        ////            {
        ////                recurso = resultQuery.Return[0];

        ////                //Solo si no esta dada de baja
        ////                if (recurso.FechaBaja == null)
        ////                {
        ////                    var resultDarDeBaja = recursoPorOrdenInspeccionRules.DeleteById(recurso.Id);
        ////                    if (!resultDarDeBaja.Ok)
        ////                    {
        ////                        result.Copy(resultDarDeBaja.Errores);
        ////                        return false;
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            recurso.OrdenInspeccion = ordenBD;

        ////            var resultQuery = recursoPorOrdenInspeccionRules.GetByIdOrdenInspeccion(ordenBD.Id);
        ////            if (!resultQuery.Ok)
        ////            {
        ////                result.Copy(resultQuery.Errores);
        ////                return false;
        ////            }

        ////            //Debo Insertar
        ////            if (resultQuery.Return == null || resultQuery.Return.Count == 0)
        ////            {
        ////                var resultInsert = recursoPorOrdenInspeccionRules.Insert(recurso);
        ////                if (!resultInsert.Ok)
        ////                {
        ////                    result.Copy(resultInsert.Errores);
        ////                    return false;
        ////                }
        ////            }

        ////            //Debo actualizar
        ////            else
        ////            {
        ////                var recursoAnterior = resultQuery.Return[0];
        ////                recursoAnterior.FechaBaja = null;
        ////                recursoAnterior.Flota = comando.Recursos.Flota;
        ////                recursoAnterior.Material = comando.Recursos.Material;
        ////                recursoAnterior.Personal = comando.Recursos.Personal;
        ////                recursoAnterior.Observaciones = comando.Recursos.Observaciones;

        ////                var resultUpdate = recursoPorOrdenInspeccionRules.Update(recursoAnterior);
        ////                if (!resultUpdate.Ok)
        ////                {
        ////                    result.Copy(resultUpdate.Errores);
        ////                    return false;
        ////                }
        ////            }
        ////        }

        ////        ordenBD.Descripcion = comando.Descripcion;

        ////        //-------------------------------
        ////        // Actualizo la OT
        ////        //-------------------------------

        ////        result = base.Update(ordenBD);
        ////        if (!result.Ok)
        ////        {
        ////            return false;
        ////        }

        ////        return result.Ok;
        ////    });

        ////    return result;
        ////}

        /*Acciones*/
        public Result<bool> EditarDescripcion(Comando_OrdenInspeccion_Descripcion comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenInspeccion, Enums.PermisoEstadoOrdenInspeccion.EditarDescripcion);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(comando.IdOrdenInspeccion);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            ot.Descripcion = comando.Descripcion;

            var resultadoUpdate = base.Update(ot);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> AgregarRequerimientos(Comando_OrdenInspeccion_Requerimientos comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenInspeccion, Enums.PermisoEstadoOrdenInspeccion.AgregarRequerimiento);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de inspeccion
            var consulta = GetByIdObligatorio(comando.IdOrdenInspeccion);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var oi = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la oi para que se le ponga fecha de modificación 
                var resultUpdateoi = base.Update(oi);
                if (!resultUpdateoi.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de inspección");
                    return false;
                }

                var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
                var permisoRequerimientoRules = new PermisoEstadoRequerimientoRules(getUsuarioLogueado());

                foreach (var idRequerimiento in comando.IdsRequerimientos)
                {

                    var contiene = oi.ContieneRequerimiento(idRequerimiento);
                    if (contiene)
                    {
                        resultado.AddErrorPublico("El requerimiento ya se encuentra en la orden de inspección.");
                        return false;
                    }


                    //Busco el requerimiento
                    var consultaRQ = requerimientoRules.GetByIdObligatorio(idRequerimiento);
                    if (!consultaRQ.Ok)
                    {
                        resultado.Copy(consultaRQ.Errores);
                        return false;
                    }

                    //Estados Requerimiento Para oi
                    var consultaEstadosRequerimientoParaoi = permisoRequerimientoRules.GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeInspeccion);
                    if (!consultaEstadosRequerimientoParaoi.Ok)
                    {
                        resultado.Copy(consultaEstadosRequerimientoParaoi.Errores);
                        return false;
                    }

                    var rq = consultaRQ.Return;
                    var estadoValido = false;
                    foreach (var estado in consultaEstadosRequerimientoParaoi.Return)
                    {
                        if (rq.GetUltimoEstado().Estado.KeyValue == estado)
                        {
                            estadoValido = true;
                        }
                    }

                    if (!estadoValido)
                    {
                        resultado.AddErrorPublico("El requerimiento no puede agregarse a la orden de inspección ya que se encuentra en estado " + rq.GetUltimoEstado().Estado.Nombre);
                        return false;
                    }

                    //Una vez insertado, le seteo al Requerimiento la orden de inspección insertada (como su oi Actual).
                    //Debe ser si o si LUEGO de insertarse el objeto.
                    //Ademas le cambio el estado al rq, indicando que fue agregado a la oi. 
                    //Tambien se llama luego de insertar, porque en la observacion del cambio de estado
                    //estoy agregando el numero de la oi.
                    var requerimientoXOrdenInspeccionRules = new RequerimientoPorOrdenInspeccionRules(getUsuarioLogueado());
                    //Genero el objeto RequerimientoPorOrdenInspeccion
                    var rqxoi = new RequerimientoPorOrdenInspeccion();
                    rqxoi.Requerimiento = rq;
                    rqxoi.OrdenInspeccion = oi;

                    //Inserto el RequerimientoPorOrdenInspeccion
                    var resultInsertRQ = requerimientoXOrdenInspeccionRules.Insert(rqxoi);
                    if (!resultInsertRQ.Ok)
                    {
                        resultado.Copy(resultInsertRQ.Errores);
                        return false;
                    }

                    //Seteo la oi activa
                    rq.OrdenInspeccionActiva = oi;
                    oi.ContieneRequerimiento(idRequerimiento);

                    //Cambio el estado
                    var resultCambiarEstadoRQ = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.INSPECCION, "En Orden de Inspección N° " + oi.GetNumero(), false);
                    if (!resultCambiarEstadoRQ.Ok)
                    {
                        resultado.Copy(resultCambiarEstadoRQ.Errores);
                        return false;
                    }

                    //Actualizo el RQ
                    var resultUpdateRQ = requerimientoRules.ValidateUpdate(rq);
                    if (!resultUpdateRQ.Ok)
                    {
                        resultado.Copy(resultUpdateRQ.Errores);
                        return false;
                    }

                    resultUpdateRQ = RequerimientoDAO.Instance.Update(rq);
                    if (!resultUpdateRQ.Ok)
                    {
                        resultado.Copy(resultUpdateRQ.Errores);
                        return false;
                    }



                }

                resultado.Return = true;
                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
                return resultado;
            }

            return resultado;
        }

        public Result<bool> QuitarRequerimiento(Comando_OrdenTrabajo_QuitarRequerimiento comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso((int)comando.IdOrdenInspeccion, Enums.PermisoEstadoOrdenInspeccion.QuitarRequerimiento);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de inspeccion
            var consulta = GetByIdObligatorio((int)comando.IdOrdenInspeccion);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            if (ot.Requerimientos().Count() == 1)
            {
                resultado.AddErrorPublico("No se puede eliminar el único requerimiento de la orden de inspección.");
                return resultado;
            }

            var contiene = ot.ContieneRequerimiento(comando.IdRequerimiento);
            if (!contiene)
            {
                resultado.AddErrorPublico("El requerimiento no se encuentra en la orden de inspección.");
                return resultado;
            }

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdateOt = base.Update(ot);
                if (!resultUpdateOt.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de inspección");
                    return false;
                }

                var rqRules = new RequerimientoRules(getUsuarioLogueado());

                //Busco el requerimiento
                var consultaRQ = rqRules.GetByIdObligatorio(comando.IdRequerimiento);
                if (!consultaRQ.Ok)
                {
                    resultado.Copy(consultaRQ.Errores);
                    return false;
                }

                var rq = consultaRQ.Return;

                //Cambio el estado
                var resultCambiarEstadoRQ = rqRules.ProcesarCambioEstado(rq, comando.EstadoKeyValue, comando.Observaciones, false);
                if (!resultCambiarEstadoRQ.Ok)
                {
                    resultado.Copy(resultCambiarEstadoRQ.Errores);
                    return false;
                }

                if (comando.Desmarcar.HasValue)
                    rq.Marcado = !(bool)comando.Desmarcar;

                //Actualizo el RQ
                var resultUpdateRQ = rqRules.ValidateUpdate(rq);
                if (!resultUpdateRQ.Ok)
                {
                    resultado.Copy(resultUpdateRQ.Errores);
                    return false;
                }

                resultUpdateRQ = RequerimientoDAO.Instance.Update(rq);
                if (!resultUpdateRQ.Ok)
                {
                    resultado.Copy(resultUpdateRQ.Errores);
                    return false;
                }

                var rqXOT = ot.GetRequerimientoPorOrdenInspeccion(rq.Id);
                if (rqXOT == null)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de inspección.");
                    return false;
                }

                var resultDelete = new RequerimientoPorOrdenInspeccionRules(getUsuarioLogueado()).Delete(rqXOT);
                if (rqXOT == null)
                {
                    resultado.AddErrorPublico("Error al eliminar el requerimiento de la orden de inspección.");
                    return false;
                }

                return true;
            }
             );

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            return resultado;
        }

        public Result<bool> AgregarNota(Comando_OrdenInspeccion_Nota comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenInspeccion, Enums.PermisoEstadoOrdenInspeccion.AgregarNota);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenInspeccion);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var oi = consulta.Return;

            //Consulto el usuario
            var resultUser = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
            if (!resultUser.Ok)
            {
                resultado.Copy(resultUser.Errores);
                return resultado;
            }

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la oi para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(oi);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de inspección");
                    return false;
                }

                //si no tiene requerimiento, la nota es para la orden
                if (!comando.IdRequerimiento.HasValue)
                {
                    var nota = new NotaPorOrdenInspeccion();
                    nota.Observaciones = comando.Observaciones;
                    nota.Usuario = resultUser.Return;
                    nota.OrdenInspeccion = oi;

                    var resultInsert = new NotaPorOrdenInspeccionRules(getUsuarioLogueado()).Insert(nota);
                    if (!resultInsert.Ok)
                    {
                        resultado.Copy(resultInsert.Errores);
                        return false;
                    }

                    return true;
                }

                //Busco el rq
                var consultaRq = new RequerimientoRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdRequerimiento);
                if (!consultaRq.Ok)
                {
                    resultado.Copy(consultaRq.Errores);
                    return false;
                }

                var rq = consultaRq.Return;

                if (oi.Requerimientos().Where(x => x.Id == rq.Id).ToList().Count() == 0)
                {
                    resultado.AddErrorPublico("El requerimiento no se encuentra en la orden de inspección");
                    return false;
                }

                //si tiene requerimiento, la nota es para el requerimiento desde la orden de trabajo
                var notaRq = new NotaPorRequerimiento();
                notaRq.Requerimiento = rq;
                notaRq.Observaciones = comando.Observaciones;
                notaRq.Usuario = resultUser.Return;
                notaRq.OrdenInspeccion = oi;

                var resultInsertNotaRq = new NotaPorRequerimientoRules(getUsuarioLogueado()).Insert(notaRq);
                if (!resultInsertNotaRq.Ok)
                {
                    resultado.Copy(resultInsertNotaRq.Errores);
                    return false;
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            return resultado;
        }

        public Result<bool> Completar(Comando_OrdenInspeccion_Cerrar comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.Id, Enums.PermisoEstadoOrdenInspeccion.Cerrar);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de inspeccion
            var consulta = GetByIdObligatorio(comando.Id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var oi = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                var estadoRequerimientoRules = new EstadoRequerimientoRules(getUsuarioLogueado());
                var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());

                //Valido si hay algun rq que no está asociado a la oi
                var rqsNoAsociados = comando.Requerimientos.Where(x => oi.RequerimientosPorOrdenInspeccionActivos().All(y => y.Requerimiento.Id != x.IdRequerimiento)).ToList();
                if (rqsNoAsociados.Count != 0)
                {
                    resultado.AddErrorPublico("Hay algunos requerimientos que no están asociados a la Orden de Inspección.");
                    return false;
                }

                //Rq que quedan 
                var rqs = oi.RequerimientosPorOrdenInspeccionActivos().Where(x => comando.Requerimientos.Any(y => y.IdRequerimiento == x.Requerimiento.Id)).ToList();

                //Paso cada RQ al estado que el usuario selecciono
                foreach (RequerimientoPorOrdenInspeccion rxot in rqs)
                {
                    //Busco el estado del rq que puso el usuario
                    var rqComando = comando.Requerimientos.Where(x => x.IdRequerimiento == rxot.Requerimiento.Id).FirstOrDefault();
                    var enumEstado = (Enums.EstadoRequerimiento)rqComando.KeyValueEstado;

                    //Busco el estado al cual quiero pasar el RQ
                    var consultaEstadoRQ = estadoRequerimientoRules.GetByKeyValue(enumEstado);
                    if (!consultaEstadoRQ.Ok || consultaEstadoRQ.Return == null)
                    {
                        resultado.Copy(consultaEstadoRQ.Errores);
                        return false;
                    }
                    var estado = consultaEstadoRQ.Return;

                    ////Valido que sea un estado de Cierre
                    //var permiso = Enums.PermisoEstadoRequerimiento.SalirDeOrdenDeTrabajo;
                    //var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(enumEstado, permiso);
                    //if (!resultadoPermiso.Ok)
                    //{
                    //    resultado.Copy(resultadoPermiso.Errores);
                    //    return false;
                    //}
                    //if (!resultadoPermiso.Ok)
                    //{
                    //    resultado.AddErrorPublico("El estado " + Utils.toTitleCase(estado.Nombre.ToUpper()) + " no es válido para un Requerimiento");
                    //    return false;
                    //}

                    var rq = rxot.Requerimiento;
                    //Solo si el RQ no esta ya en ese estado, lo cambio
                    if (rq.GetUltimoEstado().Estado.KeyValue != enumEstado)
                    {
                        var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, enumEstado, "Por haberse completado la Orden de Inspección N° " + oi.Numero, false);
                        if (!resultCambioEstado.Ok)
                        {
                            resultado.Copy(resultCambioEstado.Errores);
                            return false;
                        }
                    }

                    //Quito la OI activa
                    rq.OrdenInspeccionActiva = null;

                    //Quito el marcado del RQ
                    rq.Marcado = false;

                    //Actualizo el RQ
                    var resultUpdateRq = requerimientoRules.ValidateUpdate(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }

                    //Se llama directamente al metodo del DAO, porque el Update del Rules tiene logica que no
                    //debo saltear desde acá
                    resultUpdateRq = RequerimientoDAO.Instance.Update(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }
                }

                //Rq que se quitan
                var rqsQuitar = oi.RequerimientosPorOrdenInspeccionActivos().Where(x => comando.Requerimientos.All(y => y.IdRequerimiento != x.Requerimiento.Id)).ToList();

                //Quito los RQ
                foreach (RequerimientoPorOrdenInspeccion rxot in rqsQuitar)
                {

                    //Busco el estado al cual quiero pasar el RQ (Incompleto)
                    var enumEstado = Enums.EstadoRequerimiento.PENDIENTE;
                    var consultaEstadoRQ = estadoRequerimientoRules.GetByKeyValue(enumEstado);
                    if (!consultaEstadoRQ.Ok || consultaEstadoRQ.Return == null)
                    {
                        resultado.Copy(consultaEstadoRQ.Errores);
                        return false;
                    }

                    var estado = consultaEstadoRQ.Return;
                    var rq = rxot.Requerimiento;

                    //Solo si el RQ no esta ya en ese estado, lo cambio
                    if (rq.GetUltimoEstado().Estado.KeyValue != enumEstado)
                    {
                        var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, enumEstado, "Por haberse completado la Orden de Inspección", false);
                        if (!resultCambioEstado.Ok)
                        {
                            resultado.Copy(resultCambioEstado.Errores);
                            return false;
                        }
                    }

                    var requerimientoXOrdenInspeccionRules = new RequerimientoPorOrdenInspeccionRules(getUsuarioLogueado());
                    //Quito la fila de RequerimientoPorOrdenInspeccion
                    var rqsActivosPorOt = oi.RequerimientosPorOrdenInspeccion.Where(x => x.FechaBaja == null).ToList();
                    foreach (var rqActivo in rqsActivosPorOt)
                    {
                        if (rqActivo.Requerimiento.Id == rq.Id)
                        {
                            var resultDelete = requerimientoXOrdenInspeccionRules.Delete(rqActivo);
                            if (!resultDelete.Ok)
                            {
                                resultado.Copy(resultDelete.Errores);
                                return false;
                            }
                        }
                    }

                    //Quito la OT activa
                    rq.OrdenInspeccionActiva = null;

                    //Actualizo el RQ
                    var resultUpdateRq = requerimientoRules.ValidateUpdate(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }

                    //Se llama directamente al metodo del DAO, porque el Update del Rules tiene logica que no
                    //debo saltear desde acá
                    resultUpdateRq = RequerimientoDAO.Instance.Update(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }
                }

                //Cambio el estado de la OI
                var resultCambiarEstadoOT = CambiarEstado(oi, Enums.EstadoOrdenInspeccion.COMPLETADO, comando.Observaciones, false);
                if (!resultCambiarEstadoOT.Ok)
                {
                    resultado.Copy(resultCambiarEstadoOT.Errores);
                    return false;
                }

                //Hago el PreUpdate (pone fecha de modificacion y valida integridad de datos)
                var resultUpdate = ValidateUpdate(oi);
                if (!resultUpdate.Ok)
                {
                    resultado.Copy(resultUpdate.Errores);
                    return false;
                }

                //Actualizo directo de la base de datos (el metodo update del OrdenTrabajoRules tiene una logica propia que no aplica para el cierre de la OI)
                resultUpdate = dao.Update(oi);
                if (!resultado.Ok)
                {
                    resultado.Copy(resultUpdate.Errores);
                    return false;
                }

                resultado.Return = true;
                return true;
            });
            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            return resultado;
        }

        public Result<bool> Cancelar(Comando_OrdenTrabajo_Cancelar comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.Id, Enums.PermisoEstadoOrdenInspeccion.Cancelar);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de inspeccion
            var consulta = GetByIdObligatorio(comando.Id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                var estadoRequerimientoRules = new EstadoRequerimientoRules(getUsuarioLogueado());
                var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());

                //Paso cada RQ al estado que el usuario selecciono
                foreach (RequerimientoPorOrdenInspeccion rxot in ot.RequerimientosPorOrdenInspeccion)
                {

                    var rq = rxot.Requerimiento;
                    //Solo si el RQ no esta ya en ese estado, lo cambio
                    if (rq.GetUltimoEstado().Estado.KeyValue != Enums.EstadoRequerimiento.PENDIENTE)
                    {
                        var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.PENDIENTE, "Por cancelación de Orden de Inspección N° " + ot.Numero, false);
                        if (!resultCambioEstado.Ok)
                        {
                            resultado.Copy(resultCambioEstado.Errores);
                            return false;
                        }
                    }

                    //Quito la OI activa
                    rq.OrdenInspeccionActiva = null;

                    //Actualizo el RQ
                    var resultUpdateRq = requerimientoRules.ValidateUpdate(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }

                    //Se llama directamente al metodo del DAO, porque el Update del Rules tiene logica que no
                    //debo saltear desde acá
                    resultUpdateRq = RequerimientoDAO.Instance.Update(rq);
                    if (!resultUpdateRq.Ok)
                    {
                        resultado.Copy(resultUpdateRq.Errores);
                        return false;
                    }
                }

                //Cambio el estado de la OT
                var resultCambiarEstadoOT = CambiarEstado(ot, Enums.EstadoOrdenInspeccion.CANCELADO, comando.Motivo, false);
                if (!resultCambiarEstadoOT.Ok)
                {
                    resultado.Copy(resultCambiarEstadoOT.Errores);
                    return false;
                }

                //Hago el PreUpdate (pone fecha de modificacion y valida integridad de datos)
                var resultUpdate = ValidateUpdate(ot);
                if (!resultUpdate.Ok)
                {
                    resultado.Copy(resultUpdate.Errores);
                    return false;
                }

                //Actualizo directo de la base de datos (el metodo update del OrdenTrabajoRules tiene una logica propia que no aplica para el cierre de la OT)
                resultUpdate = dao.Update(ot);
                if (!resultado.Ok)
                {
                    resultado.Copy(resultUpdate.Errores);
                    return false;
                }

                resultado.Return = true;
                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            return resultado;
        }

        /*Numeración*/
        public Result<bool> ExisteNumero(string numero, int año)
        {
            return dao.ExisteNumero(numero, año);
        }

        public Resultado_NumeroIdentificatorio GenerarNumeroIdentificatorio()
        {
            int año = DateTime.Now.Year;
            string numero = null;
            bool yaexiste = true;


            do
            {
                numero = RandomString(6).ToUpper();

                //Compruebo si ya existe el numero y sigo buscando de ser asi.
                var resultYaExiste = ExisteNumero(numero, año);
                if (!resultYaExiste.Ok)
                {
                    yaexiste = true;
                }
                else
                {
                    yaexiste = resultYaExiste.Return;
                }

            } while (yaexiste);


            return new Resultado_NumeroIdentificatorio(numero, año);
        }

        public static string RandomString(int length)
        {
            const string chars = "ACDEFGHJKLMNPQRSTUWXZ123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        ///* Estados */
        public Result<EstadoOrdenInspeccionHistorial> CrearEstado(OrdenInspeccion ot, Enums.EstadoOrdenInspeccion e, string observaciones)
        {
            var result = new Result<EstadoOrdenInspeccionHistorial>();

            var resultEstado = new EstadoOrdenInspeccionRules(getUsuarioLogueado()).GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                return result;
            }

            var estadoOT = new EstadoOrdenInspeccionHistorial();
            estadoOT.Fecha = DateTime.Now;
            estadoOT.FechaAlta = DateTime.Now;
            estadoOT.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoOT.Estado = resultEstado.Return;
            estadoOT.Observaciones = observaciones;
            estadoOT.OrdenInspeccion = ot;

            result.Return = estadoOT;
            return result;
        }

        public Result<OrdenInspeccion> CambiarEstado(OrdenInspeccion ot, Enums.EstadoOrdenInspeccion estado, string observaciones, bool guardarCambios)
        {
            var estadoOT = CrearEstado(ot, estado, observaciones);
            return CambiarEstado(ot, estadoOT.Return, guardarCambios);
        }

        public Result<OrdenInspeccion> CambiarEstado(OrdenInspeccion oi, EstadoOrdenInspeccionHistorial estado, bool guardarCambios)
        {

            var result = new Result<OrdenInspeccion>();

            if (oi.Estados == null)
            {
                oi.Estados = new List<EstadoOrdenInspeccionHistorial>();
            }

            if (oi.Estados != null && oi.Estados.Count != 0)
            {
                foreach (EstadoOrdenInspeccionHistorial e in oi.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estado.Ultimo = true;
            oi.Estados.Add(estado);

            if (guardarCambios)
            {
                var resultUpdate = Update(oi);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }

            result.Return = oi;
            return result;
        }

        //Detalle
        public Result<Resultado_OrdenInspeccionDetalle> GetDetalleById(int id)
        {
            var resultado = new Result<Resultado_OrdenInspeccionDetalle>();

            int? idUsuario = null;
            if (getUsuarioLogueado() != null && getUsuarioLogueado().Usuario != null)
            {
                idUsuario = getUsuarioLogueado().Usuario.Id;
            }

            //Detalle
            var resultadoDetalle = dao.GetDetalleById(id, idUsuario);
            if (!resultadoDetalle.Ok)
            {
                resultado.Copy(resultadoDetalle.Errores);
                return resultado;
            }
            resultado.Return = resultadoDetalle.Return;

            //Historial Estados
            var resultadoEstados = GetDetalleHistorialEstadosById(id);
            if (!resultadoEstados.Ok)
            {
                resultado.Copy(resultadoEstados.Errores);
                return resultado;
            }
            resultado.Return.Estados = resultadoEstados.Return;

            //Requerimientos 
            var resultadoRequerimientos = GetDetalleRequerimientosById(id);
            if (!resultadoRequerimientos.Ok)
            {
                resultado.Copy(resultadoRequerimientos.Errores);
                return resultado;
            }
            resultado.Return.Requerimientos = resultadoRequerimientos.Return;

            //Notas 
            var resultadoNotas = GetDetalleNotasById(id);
            if (!resultadoNotas.Ok)
            {
                resultado.Copy(resultadoNotas.Errores);
                return resultado;
            }
            resultado.Return.Notas = resultadoNotas.Return;

            return resultado;
        }

        public Result<List<Resultado_OrdenInspeccionDetalle_EstadoHistorial>> GetDetalleHistorialEstadosById(int id)
        {
            return dao.GetDetalleHistorialEstadosById(id);
        }

        public Result<List<Resultado_OrdenInspeccionDetalle_Nota>> GetDetalleNotasById(int id)
        {
            return dao.GetDetalleNotasById(id);
        }

        public Result<List<Resultado_OrdenInspeccionDetalle_Requerimiento>> GetDetalleRequerimientosById(int id)
        {
            return dao.GetDetalleRequerimientosById(id);
        }

        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetUltimas(int cantidad)
        {
            var consulta = new Consulta_OrdenInspeccion(getUsuarioLogueado().Ambito!=null && getUsuarioLogueado().Ambito.KeyValue!=0);
            var estados = new List<Enums.EstadoOrdenInspeccion>();
            estados.Add(Enums.EstadoOrdenInspeccion.ENPROCESO);
            consulta.EstadosKeyValue = estados;
            return dao.GetUltimas(cantidad, consulta);
        }

        /* Permiso */
        public Result<bool> ValidarPemiso(int id, Enums.PermisoEstadoOrdenInspeccion permiso)
        {
            var resultado = new Result<bool>();

            //Lo busco
            var consulta = GetById(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            if (ot == null)
            {
                resultado.AddErrorPublico("La orden de inspección no existe");
                return resultado;
            }
            if (ot.FechaBaja != null)
            {
                resultado.AddErrorPublico("La orden de inspección se encuentra dado de baja");
                return resultado;
            }

            //Valido el permiso
            var keyValueEstado = ot.GetUltimoEstado().Estado.KeyValue;
            var resultadoPermiso = new PermisoEstadoOrdenInspeccionRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }
            if (!resultadoPermiso.Return)
            {
                resultado.AddErrorPublico("La orden de inspección no se encuentra en un estado válido para realizar esta accion");
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetDatosTabla(Consulta_OrdenInspeccion consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>>();
            consulta.Marcado = getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0;
            var resultadoIds = dao.GetIds(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetDatosTablaByIds(resultadoIds.Return);
        }

        int LIMITE_CANTIDAD_TABLA = 5000;
        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetDatosTablaByIds(List<int> ids)
        {
            return dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, ids);
        }

    }
}
