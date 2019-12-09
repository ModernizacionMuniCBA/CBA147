using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Model;
using Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using Model.Resultados;
using NHibernate.Transform;
using Model.Consultas;
using System.Text;
using Model.Utiles;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Loader.Criteria;

namespace DAO.DAO
{
    public class RequerimientoDAO : BaseDAO<Requerimiento>
    {
        private static RequerimientoDAO instance;

        public static RequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new RequerimientoDAO();
                return instance;
            }
        }


        #region Queries

        //private IQueryOver<Requerimiento, Requerimiento> GetQuery(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, string numero, int? anio, int? idServicio, int? idMotivo, int? idArea, bool? esPersonaFisica, int? idPersonaReferente, int? idUsuarioReferente, int? idCalle, int? idCpc, List<int> idsBarrio, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, List<Enums.PrioridadRequerimiento> prioridades, bool? dadosDeBaja, int? altura, bool? marcado)
        //{
        //    var query = GetSession().QueryOver<Requerimiento>();

        //    var joinMotivo = query.JoinQueryOver<Motivo>(x => x.Motivo);

        //    //Marcado
        //    if (marcado != null)
        //    {
        //        query.Where(x => x.Marcado == marcado);
        //    }

        //    //Tipo
        //    if (tipo.HasValue)
        //    {
        //        query.JoinQueryOver<TipoRequerimiento>(x => x.Tipo).Where(x => x.KeyValue == tipo);
        //    }

        //    //Estados
        //    if (estados != null && estados.Count != 0)
        //    {
        //        query.JoinQueryOver<EstadoRequerimientoHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoRequerimiento>(x => x.Estado).Where(x => x.KeyValue.IsIn(estados));
        //    }

        //    //Anio
        //    if (anio.HasValue && anio.Value != -1)
        //    {
        //        query.Where(x => x.Año == anio.Value);
        //    }

        //    //Numero
        //    if (numero != null)
        //    {
        //        query.Where(x => x.Numero == numero);
        //    }

            //Motivo
            //if (idMotivo.HasValue && idMotivo.Value != -1)
            //{
            //    joinMotivo.Where(x => x.Id == idMotivo.Value);
            //}

        //    //tipos motivo
        //    if (motivo != null && prioridades.Count != 0)
        //    {
        //        query.Where(x => x.Prioridad.IsIn(prioridades));
        //    }

        //    //Servicio
        //    if (idServicio.HasValue && idServicio.Value != -1)
        //    {
        //        joinMotivo.JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id == idServicio.Value);
        //    }

        //    //Areas
        //    if (idArea.HasValue && idArea != 0)
        //    {
        //        joinMotivo.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == idArea.Value);
        //        //if (idsBarrio[0] == -1)
        //        //{
        //        //    query.JoinQueryOver<Domicilio>(x => x.Domicilio).JoinQueryOver<Barrio>(x => x.Barrio).Where(x => x.Id.IsIn(idsBarrio));
        //        //}

        //    }

        //    //Persona
        //    if (esPersonaFisica != null && idPersonaReferente.HasValue && idPersonaReferente.Value != -1)
        //    {
        //        if (esPersonaFisica == true)
        //        {
        //            query.JoinQueryOver<PersonaFisica>(x => x.PersonaFisica).Where(x => x.Id == idPersonaReferente.Value);
        //        }

        //    }

        //    if (idUsuarioReferente.HasValue)
        //    {
        //        query.JoinQueryOver<UsuarioReferentePorRequerimiento>(x => x.UsuariosReferentes).Where(x => x.UsuarioReferente.Id == idUsuarioReferente.Value);
        //    }

        //    //Calle, CPC, BARRIO, ALTURA
        //    if ((idCalle.HasValue && idCalle != -1) || (idCpc.HasValue && idCpc.Value != -1) || (idsBarrio != null && idsBarrio.Count != 0) || (altura.HasValue && altura.Value != -1))
        //    {
        //        var joinDomicilio = query.JoinQueryOver<Domicilio>(x => x.Domicilio);

        //        //Calle
        //        if (idCalle.HasValue)
        //        {
        //            //joinDomicilio.JoinQueryOver<Calle>(x => x.Calle).Where(x => x.Id == idCalle.Value);
        //        }

        //        //CPC
        //        if (idCpc.HasValue)
        //        {
        //            joinDomicilio.JoinQueryOver<Cpc>(x => x.Cpc).Where(x => x.Id == idCpc.Value);
        //        }

        //        //Barrio
        //        if (idsBarrio != null && idsBarrio.Count != 0)
        //        {
        //            joinDomicilio.JoinQueryOver<Barrio>(x => x.Barrio).Where(x => x.Id.IsIn(idsBarrio));
        //        }

        //        // RANGO DE ALTURA
        //        if (altura.HasValue)
        //        {
        //            int rango = 100;
        //            int alturaMinima = altura.Value - rango;
        //            int alturaMaxima = altura.Value + rango;
        //            //joinDomicilio.Where(x => x.Altura >= alturaMinima && x.Altura <= alturaMaxima);
        //        }
        //    }

        //    //Fechas
        //    if (fechaDesde.HasValue && fechaHasta.HasValue)
        //    {
        //        query.Where(m => m.FechaAlta.Value.Date >= fechaDesde).Where(m => m.FechaAlta.Value.Date <= fechaHasta);
        //    }

        //    //Relevamiento interno
        //    if (relevamientoInterno.HasValue)
        //    {
        //        query.Where(x => x.RelevamientoInterno == relevamientoInterno.Value);
        //    }

        //    //Prioridades
        //    if (prioridades != null && prioridades.Count != 0)
        //    {
        //        query.Where(x => x.Prioridad.IsIn(prioridades));
        //    }

        //    //Dado de baja
        //    if (dadosDeBaja.HasValue)
        //    {
        //        if (dadosDeBaja.Value)
        //        {
        //            query.Where(x => x.FechaBaja != null);
        //        }
        //        else
        //        {
        //            query.Where(x => x.FechaBaja == null);
        //        }
        //    }

        //    ////Barrios x Zona
        //    //if (barrios != null)
        //    //{
        //    //    query.JoinQueryOver<Domicilio>(x => x.Domicilio).Where(x => x.Barrio.Id.IsIn(barrios));
        //    //}            
        //    return query;
            //}            

        private IQueryOver<Requerimiento, Requerimiento> GetQuery(Consulta_Requerimiento consulta, bool? marcado)
        {
            var query = GetSession().QueryOver<Requerimiento>();
            query.JoinQueryOver<TipoRequerimiento>(x => x.Tipo).Where(x => x.KeyValue == Enums.TipoRequerimiento.RECLAMO);
            var joinArea = query.JoinQueryOver<CerrojoArea>(x => x.AreaResponsable);
            var joinMotivo = query.JoinQueryOver<Motivo>(x => x.Motivo);
            var joinOrigen = query.JoinQueryOver<Origen>(x => x.Origen);
            var joinServicio = joinMotivo.JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio);

            //Marcado
            if (marcado != null)
            {
                query.Where(x => x.Marcado == marcado);
            }

            //Numero
            if (consulta.Numero != null)
            {
                query.Where(x => x.Numero == consulta.Numero);
            }

            //Anio
            if (consulta.Año.HasValue && consulta.Año.Value != -1)
            {
                query.Where(x => x.Año == consulta.Año.Value);
            }

            //Prioridad
            if (consulta.Prioridades != null && consulta.Prioridades.Count != 0)
            {
                query.Where(x => x.Prioridad.IsIn(consulta.Prioridades));
            }

            //Estados
            if (consulta.EstadosKeyValue != null && consulta.EstadosKeyValue.Count != 0)
            {
                var joinUltimoEstado = query.JoinQueryOver<EstadoRequerimientoHistorial>(x => x.Estados).Where(x => x.Ultimo == true);
                joinUltimoEstado.JoinQueryOver<EstadoRequerimiento>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.EstadosKeyValue));

                //var joinEstadoRH = query.JoinQueryOver<EstadoRequerimientoHistorial>(x => x.Estados);
                //joinEstadoRH.JoinQueryOver<EstadoRequerimiento>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.EstadosKeyValue));

            }

            //Area
            if (consulta.IdsArea != null && consulta.IdsArea.Count != 0)
            {
                joinArea.Where(x => x.Id.IsIn(consulta.IdsArea));
            }

            //Origen
            if (consulta.IdsOrigen != null && consulta.IdsOrigen.Count != 0)
            {
                joinOrigen.Where(x => x.Id.IsIn(consulta.IdsOrigen));
            }

            //Servicio
            if (consulta.IdsServicio != null && consulta.IdsServicio.Count != 0)
            {
                joinServicio.Where(x => x.Id.IsIn(consulta.IdsServicio));
            }

            //Motivo
            if (consulta.IdsMotivo != null && consulta.IdsMotivo.Count != 0)
            {
                joinMotivo.Where(x => x.Id.IsIn(consulta.IdsMotivo));
            }

            //Categoria
            if (consulta.IdsCategoria != null && consulta.IdsCategoria.Count != 0)
            {
                joinMotivo.Where(x => x.Categoria.Id.IsIn(consulta.IdsCategoria));
            }

            //tipo de motivos
            if (consulta.Tipos != null && consulta.Tipos.Count != 0)
            {
                joinMotivo.Where(x => x.Tipo.IsIn(consulta.Tipos));
            }

            //Urgentes
            if (consulta.Urgente.HasValue)
            {
                joinMotivo.Where(x => x.Urgente == consulta.Urgente.Value);
            }

            //Persona fisica
            if (consulta.IdsPersonaFisica != null && consulta.IdsPersonaFisica.Count != 0)
            {
                query.JoinQueryOver<PersonaFisica>(x => x.PersonaFisica).Where(x => x.Id.IsIn(consulta.IdsPersonaFisica));
            }

            //Usuario Refernte
            if (consulta.IdsUsuarioReferente != null && consulta.IdsUsuarioReferente.Count != 0)
            {
                var joinReferente = query.JoinQueryOver<UsuarioReferentePorRequerimiento>(x => x.UsuariosReferentes);
                joinReferente.JoinQueryOver<_VecinoVirtualUsuario>(x => x.UsuarioReferente).Where(x => x.Id.IsIn(consulta.IdsUsuarioReferente));
            }

            //Usuario creador
            if (consulta.IdsUsuarioCreador != null && consulta.IdsUsuarioCreador.Count != 0)
            {
                query.JoinQueryOver<_VecinoVirtualUsuario>(x => x.UsuarioCreador).Where(x => x.Id.IsIn(consulta.IdsUsuarioCreador));
            }


            var joinDomicilio = query.JoinQueryOver<Domicilio>(x => x.Domicilio);

            //Barrio (pero a traves de la zona y/o subzona seleccionada)
            List<int> idsBarrio = new List<int>();
            if (((consulta.IdsZona != null && consulta.IdsZona.Count != 0)))
            {
                idsBarrio = GetSession().QueryOver<BarrioPorZona>().Where(x => x.Zona.Id.IsIn(consulta.IdsZona) && x.FechaBaja == null).Select(x => x.Barrio.Id).List<int>().ToList();
            }
            //Barrio a traves de los ids mandados
            else
            {
                idsBarrio = consulta.IdsBarrio;
            }

            //Barrio 
            if ((idsBarrio != null && idsBarrio.Count != 0) || (consulta.IdsBarrioCatastro != null && consulta.IdsBarrioCatastro.Count != 0))
            {
                if (idsBarrio != null && idsBarrio.Count != 0)
                {
                    joinDomicilio.JoinQueryOver<Barrio>(x => x.Barrio).Where(x => x.Id.IsIn(idsBarrio));
                }
                else
                {
                    joinDomicilio.JoinQueryOver<Barrio>(x => x.Barrio).Where(x => x.IdCatastro.IsIn(consulta.IdsBarrioCatastro));
                }
            }

            //CPC
            if (consulta.KeyValuesCPC != null && consulta.KeyValuesCPC.Count != 0 && consulta.KeyValuesCPC.Count != -1)
            {
                joinDomicilio.JoinQueryOver<Cpc>(x => x.Cpc).Where(x => x.Numero.IsIn(consulta.KeyValuesCPC));
            }

            //Domicilio
            if (!string.IsNullOrEmpty(consulta.Domicilio))
            {
                joinDomicilio.Where(x => x.Direccion.IsLike(consulta.Domicilio, MatchMode.Anywhere) || x.Observaciones.IsLike(consulta.Domicilio, MatchMode.Anywhere));
            }

            //Relevamiento de oficio
            if (consulta.RelevamientoDeOficio.HasValue)
            {
                query.Where(x => x.RelevamientoInterno == consulta.RelevamientoDeOficio.Value);
            }

            //Orden de atencion critica
            if (consulta.OrdenAtencionCritica.HasValue)
            {
                if (consulta.OrdenAtencionCritica.Value)
                {
                    query.Left.JoinQueryOver<RequerimientoPorOrdenEspecial>(x => x.OrdenesEspeciales).Where(x => x.FechaBaja == null && x.Id != null);
                    //query.Where(x => x.OrdenesEspeciales != null && x.OrdenesEspeciales.Count() != 0);
                }
                else
                {
                    query.Left.JoinQueryOver<RequerimientoPorOrdenEspecial>(x => x.OrdenesEspeciales).Where(x => x.FechaBaja == null && x.Id == null);
                    //query.Where(x => x.OrdenesEspeciales == null || x.OrdenesEspeciales.Count() == 0);
                }
            }

            //Inspeccionado
            if (consulta.Inspeccionado.HasValue)
            {
                if (consulta.Inspeccionado.Value)
                {
                    query.Left.JoinQueryOver<RequerimientoPorOrdenInspeccion>(x => x.OrdenesInspeccion).Where(x => x.FechaBaja == null && x.Id != null);
                    //query.Where(x => x.OrdenesEspeciales != null && x.OrdenesEspeciales.Count() != 0);
                }
                else
                {
                    query.Left.JoinQueryOver<RequerimientoPorOrdenInspeccion>(x => x.OrdenesInspeccion).Where(x => x.FechaBaja == null && x.Id == null);
                    //query.Where(x => x.OrdenesEspeciales == null || x.OrdenesEspeciales.Count() == 0);
                }
            }

            //Fecha desde y hasta

            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                DateTime? fechaDesde = consulta.FechaDesde;
                DateTime? fechaHasta = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                query.Where(x => x.FechaAlta.Value.Date >= fechaDesde).Where(x => x.FechaAlta.Value.Date <= fechaHasta);
            }

            //Mes y Año
            if (consulta.Mes.HasValue && consulta.AñoDeMes.HasValue)
            {
                query.Where(x => x.FechaAlta.Value.Month == consulta.Mes).Where(x => x.FechaAlta.Value.Year == consulta.AñoDeMes);
            }

            //Dados de baja
            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }
            return query;
        }



        private IQueryOver<Requerimiento, Requerimiento> GetQueryConDomicilio(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, bool? conDomicilio, int? idCpc, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, bool? dadosDeBaja)
        {
            var query = GetSession().QueryOver<Requerimiento>();



            //Tipo
            if (tipo.HasValue)
            {
                query.JoinQueryOver<TipoRequerimiento>(x => x.Tipo).Where(x => x.KeyValue == tipo);
            }

            //Estados
            if (estados != null && estados.Count != 0)
            {
                query.JoinQueryOver<EstadoRequerimientoHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoRequerimiento>(x => x.Estado).Where(x => x.KeyValue.IsIn(estados));
            }

            ////CPC (REVISAR)
            //if ((idCpc.HasValue && idCpc.Value != -1))
            //{
            //    var joinDomicilio = query.JoinQueryOver<Domicilio>(x => x.Domicilio);              

            //    //CPC
            //    if (idCpc.HasValue)
            //    {
            //        joinDomicilio.JoinQueryOver<Cpc>(x => x.Cpc).Where(x => x.Id == idCpc.Value);
            //    }               
            //}

            //Fechas
            if (fechaDesde.HasValue && fechaHasta.HasValue)
            {
                query.Where(m => m.FechaAlta.Value.Date >= fechaDesde).Where(m => m.FechaAlta.Value.Date <= fechaHasta);
            }

            //Relevamiento interno
            if (relevamientoInterno.HasValue)
            {
                query.Where(x => x.RelevamientoInterno == relevamientoInterno.Value);
            }

            //Dado de baja
            if (dadosDeBaja.HasValue)
            {
                if (dadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }

            //Con Domicilio
            if (conDomicilio.HasValue)
            {

                if (conDomicilio.Value)
                {
                    query.Where(x => x.Domicilio != null);
                    query.JoinQueryOver<Domicilio>(x => x.Domicilio).Where(x => x.Cpc != null);
                    //var joinDomicilio = query.JoinQueryOver<Domicilio>(x => x.Domicilio);
                    //joinDomicilio.JoinQueryOver<Cpc>(x => x.Cpc).Where(x => x.Id != null);
                }
                else
                {
                    query.Where(x => x.Domicilio == null);
                }
            }

            return query;
        }

        #endregion


        //Para Estadisticas (Debe irse)

        //public Result<List<int>> GetIds(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, string numero, int? anio, int? idServicio, int? idMotivo, int? idArea, bool? esPersonaFisica, int? idPersona, int? idUsuarioCerrojo, int? idCalle, int? idCpc, List<int> idsBarrio, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, List<Enums.PrioridadRequerimiento> prioridades, bool? dadosDeBaja, int? altura, bool? marcado)
        //{
        //    var result = new Result<List<int>>();

        //    try
        //    {
        //        var query = GetQuery(tipo, estados, numero, anio, idServicio, idMotivo, idArea, esPersonaFisica, idPersona, idUsuarioCerrojo, idCalle, idCpc, idsBarrio, fechaDesde, fechaHasta, relevamientoInterno, prioridades, dadosDeBaja, altura, marcado);
        //        query.Select(x => x.Id);
        //        result.Return = query.List<int>().ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        result.AddErrorInterno(e);
        //    }
        //    return result;
        //}

        public Result<List<int>> GetIdsConDomicilio(Enums.TipoRequerimiento? tipo, List<Enums.EstadoRequerimiento> estados, bool? conDomicilio, int? idCpc, DateTime? fechaDesde, DateTime? fechaHasta, bool? relevamientoInterno, bool? dadosDeBaja)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQueryConDomicilio(tipo, estados, conDomicilio, idCpc, fechaDesde, fechaHasta, relevamientoInterno, dadosDeBaja);
                query.Select(x => x.Id);
                result.Return = query.List<int>().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


        //Consulta By Filters

        public Result<List<int>> GetIdsByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var query = GetQuery(consulta, marcado);
                if (consulta.Limite.HasValue)
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).Take(consulta.Limite.Value).List<int>().ToList();
                }
                else
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).List<int>().ToList();
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsAreaByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var query = GetQuery(consulta, marcado);
                if (consulta.Limite.HasValue)
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).Take(consulta.Limite.Value).List<int>().ToList();
                }
                else
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).List<int>().ToList();
                }

                if (resultado.Return.Count != 0)
                {
                    string sql = @"SELECT
	a.Id
FROM
	Requerimiento r
INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable
WHERE
	r.Id IN (" + string.Join(",", resultado.Return) + @")";

                    resultado.Return = GetSession().CreateSQLQuery(sql).List<int>().ToList();
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsTipoByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var query = GetQuery(consulta, marcado);
                if (consulta.Limite.HasValue)
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).Take(consulta.Limite.Value).List<int>().ToList();
                }
                else
                {
                    resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).List<int>().ToList();
                }

                if (resultado.Return.Count != 0)
                {
                    string sql = @"SELECT
	m.Tipo
FROM
	Requerimiento r
INNER JOIN Motivo m ON m.Id = r.IdMotivo 
WHERE
	r.Id IN (" + string.Join(",", resultado.Return) + @")";

                    resultado.Return = GetSession().CreateSQLQuery(sql).List<int>().ToList();
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
        public Result<List<Requerimiento>> GetByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<List<Requerimiento>>();
            try
            {
                var query = GetQuery(consulta, marcado);
                resultado.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<int> GetCantidadByFilters(Consulta_Requerimiento consulta, bool? marcado)
        {
            var resultado = new Result<int>();
            try
            {
                var query = GetQuery(consulta, marcado);
                resultado.Return = query.RowCount();
            }
            catch (Exception e)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
            }
            return resultado;
        }


        //Consulta Por Numero

        public Result<Requerimiento> GetByNumero(string numero, int año)
        {
            var resultado = new Result<Requerimiento>();
            var resultadoConsulta = GetByFilters(new Consulta_Requerimiento()
            {
                Numero = null,
                Año = año,
                DadosDeBaja = false
            }, null);

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == null || resultadoConsulta.Return.Count == 0)
            {
                resultado.Return = null;
            }
            else
            {
                resultado.Return = resultadoConsulta.Return[0];
            }

            return resultado;
        }

        public Result<int> GetIdByNumero(string numero, int año)
        {
            var resultado = new Result<int>();
            var resultadoConsulta = GetIdsByFilters(new Consulta_Requerimiento()
            {
                Numero = null,
                Año = año,
                DadosDeBaja = false
            }, null);

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null && resultadoConsulta.Return.Count == 0)
            {
                resultado.Return = resultadoConsulta.Return[0];
            }

            return resultado;
        }

        public Result<int> GetDiasDesdeCreacion(int idRequerimiento)
        {
            //var resultado = new Result<int>();
            //var resultadoConsulta = GetIdsByFilters(new Consulta_Requerimiento()
            //{
            //    Numero = null,
            //    Año = año,
            //    DadosDeBaja = false
            //}, null);

            //if (!resultadoConsulta.Ok)
            //{
            //    resultado.Copy(resultadoConsulta.Errores);
            //    return resultado;
            //}

            //if (resultadoConsulta.Return != null && resultadoConsulta.Return.Count == 0)
            //{
            //    resultado.Return = resultadoConsulta.Return[0];
            //}

            //return resultado;
            return null;
        }


        //Consulta Ultimos

        public Result<List<Requerimiento>> GetUltimos(int ultimos)
        {
            var resultado = new Result<List<Requerimiento>>();

            var query = GetQuery(new Consulta_Requerimiento()
            {
                DadosDeBaja = false
            }, null);

            resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Take(ultimos).List().ToList();
            return resultado;
        }

        public Result<List<int>> GetIdsUltimos(Consulta_Requerimiento consulta, int ultimos)
        {
            var resultado = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta, null);

                resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).Take(ultimos).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsPeligrososUltimos(Consulta_Requerimiento consulta, int cantidad)
        {
            var resultado = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta, null);
                resultado.Return = query.OrderBy(x => x.FechaAlta).Desc.Select(x => x.Id).Take(cantidad).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        //Consulta Cercanos

        public Result<List<Requerimiento>> GetCercanos(Consulta_RequerimientoCercano consulta)
        {
            var resultado = new Result<List<Requerimiento>>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"
                    select * from Requerimiento r");
                sb.Append(@"
                    inner join 
                    (
                        select -1 as Id2");

                var resultadoIds = GetIdsCercanos(consulta);

                foreach (var id in resultadoIds.Return)
                {
                    sb.Append(" union all select " + id + " ");
                }

                sb.Append(@"
                        ) 
                        as x on r.Id = x.Id2");

                resultado.Return = GetSession().CreateSQLQuery(sb.ToString()).List<Requerimiento>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsCercanos(Consulta_RequerimientoCercano consulta)
        {
            List<int> idsMotivo = null;
            if (consulta.IdMotivo.HasValue)
            {
                idsMotivo = new List<int>() { consulta.IdMotivo.Value };
            }

            var query = GetQuery(new Consulta_Requerimiento()
            {
                IdsMotivo = idsMotivo,
                EstadosKeyValue = consulta.EstadosKeyValue,
                DadosDeBaja = consulta.DadosDeBaja
            }, null);

            var ids = query.Select(x => x.Id).List<int>().ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                    select r.Id, d.Latitud, d.Longitud from Requerimiento r
                    inner join Domicilio d on d.Id = r.IdDomicilio");
            sb.Append(@"
                    inner join 
                    (
                        select -1 as Id2");


            foreach (var id in ids)
            {
                sb.Append(" union all select " + id + " ");
            }

            sb.Append(@"
                        ) 
                        as x on r.Id = x.Id2");

            sb.Append(@"
                WHERE d.Latitud is not null AND d.Longitud is not null
            ");

            var idsCercanos = new List<int>();
            var resultadoCoordenadas = GetSession().CreateSQLQuery(sb.ToString()).List<object[]>().ToList();

            double[] coordenadas1 = new double[2];
            coordenadas1[0] = consulta.Latitud;
            coordenadas1[1] = consulta.Longitud;

            System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
            {
                NumberDecimalSeparator = ",",
            };

            foreach (var item in resultadoCoordenadas)
            {
                var id = int.Parse(item[0] + "");
                double lat = double.Parse(item[1] + "", nf);
                double lng = double.Parse(item[2] + "", nf);
                double[] coordenadas2 = new double[] { lat, lng };
                var distancia = GoogleMapsUtils.Distancia(coordenadas1, coordenadas2) * 1000;
                if (distancia <= consulta.Distancia)
                {
                    idsCercanos.Add(id);
                }
            }



            var resultado = new Result<List<int>>();
            resultado.Return = idsCercanos;
            return resultado;

        }

        public Result<int> GetCantidadCercanos(Consulta_RequerimientoCercano consulta)
        {
            var resultado = new Result<int>();

            var resultadoIds = GetIdsCercanos(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            resultado.Return = resultadoIds.Return.Count();
            return resultado;
        }

        public Result<List<Resultado_RequerimientoTopBarrios>> GetTop(List<Enums.EstadoRequerimiento> estados, List<int> idsArea, bool marcado, int? keyValueCpc, int? idArea, int? idZona, int? idCategoria)
        {
            var resultado = new Result<List<Resultado_RequerimientoTopBarrios>>();

            List<int> e = new List<int>();
            estados.ForEach(x => e.Add((int)x));

            var sql = @"Select b.IdCatastro as BarrioId, b.Nombre as BarrioNombre, a.Id as AreaId, a.Nombre as AreaNombre, count(*) as Cantidad from Requerimiento r
                        inner join Motivo m on m.Id = r.IdMotivo
                        inner join CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable
                        inner join Domicilio d on d.Id = r.IdDomicilio
                        inner join Barrio b on b.Id = d.IdBarrio
                        inner join Cpc cpc on cpc.Id = d.IdCpc
                        inner join EstadoRequerimientoHistorial erh on erh.IdRequerimiento = r.Id
                        inner join EstadoRequerimiento er on er.Id = erh.IdEstado and erh.Ultimo = 1 and er.KeyValue in (" + string.Join(", ", e) + @")
                        where r.FechaBaja is null and a.Id in (" + string.Join(", ", idsArea) + @") and m.Tipo=1 and r.Marcado = " + (marcado ? 1 : 0);
            if (keyValueCpc.HasValue)
            {
                sql += @" and cpc.Numero = " + keyValueCpc.Value;
            }

            if (idCategoria.HasValue)
            {
                sql += @" and m.IdCategoriaMotivoArea = " + idCategoria.Value;                
            }

            if (idZona.HasValue && idArea.HasValue)
            {
                sql += @" and EXISTS (
		                    SELECT
	                            bxz.IdBarrio
                            FROM
	                            BarrioPorZona bxz
                            INNER JOIN Zona z ON z.Id = bxz.IdZona
                            WHERE
                                z.IdAreaCerrojo = " + idArea.Value + @" and bxz.IdBarrio = b.Id and  z.Id = " + idZona.Value + @" and bxz.FechaBaja is null
	                        ) and a.Id = " + idArea.Value;
            }
            else if (idArea.HasValue)
            {
                sql += @" and a.Id=" + idArea.Value;
            }
            sql += @"   GROUP BY b.IdCatastro, b.Nombre, a.Id, a.Nombre
                        order by count(*) desc";

            IQuery query = GetSession().CreateSQLQuery(sql);
            query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_RequerimientoTopBarrios)));
            var data = new List<Resultado_RequerimientoTopBarrios>();
            var info = query.List<Resultado_RequerimientoTopBarrios>().ToList();
            if (info.Count != 0 && info[0].AreaId != 0)
            {
                data.AddRange(info);
            }

            resultado.Return = data;
            return resultado;
        }

        public Result<List<Resultado_MarcadorGoogleMaps>> GetTopMarcadores(List<Enums.EstadoRequerimiento> estados, List<int> idsArea, bool marcado, int? keyValueCpc, int? idArea, int? idZona, int? idCategoria)
        {
            List<int> e = new List<int>();
            estados.ForEach(x => e.Add((int)x));

            var sql = @"Select r.Id from Requerimiento r
                        inner join Motivo m on m.Id=r.IdMotivo 
                        inner join CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable
                        inner join Domicilio d on d.Id = r.IdDomicilio
                        inner join Barrio b on b.Id = d.IdBarrio
                        inner join Cpc cpc on cpc.Id = d.IdCpc
                        inner join EstadoRequerimientoHistorial erh on erh.IdRequerimiento = r.Id
                        inner join EstadoRequerimiento er on er.Id = erh.IdEstado and erh.Ultimo = 1 and er.KeyValue in (" + string.Join(", ", e) + @")
                        where r.FechaBaja is null and a.Id in (" + string.Join(", ", idsArea) + @") and m.Tipo=1 and r.Marcado = " + (marcado ? 1 : 0);
            if (keyValueCpc.HasValue)
            {
                sql += @" and cpc.Numero = " + keyValueCpc.Value;
            }
            if (idCategoria.HasValue)
            {
                sql += @" and m.IdCategoriaMotivoArea = " + idCategoria.Value;
            }
            if (idZona.HasValue && idArea.HasValue)
            {
                sql += @" and EXISTS (
		                    SELECT
	                            bxz.IdBarrio
                            FROM
	                            BarrioPorZona bxz
                            INNER JOIN Zona z ON z.Id = bxz.IdZona
                            WHERE
	                            z.IdAreaCerrojo = " + idArea.Value + @" and bxz.IdBarrio = b.Id and  z.Id = " + idZona.Value + @" and bxz.FechaBaja is null
	                        ) and a.Id = " + idArea.Value;
            }
            IQuery query = GetSession().CreateSQLQuery(sql);
            var info = query.List<int>().ToList();
            return GetMarcadoresGoogleMaps(info);
        }


        //Busqueda Global
        public Result<List<int>> GetIdsBusquedaGlobal(string input)
        {
            var resultado = new Result<List<int>>();
            try
            {
               input = input.Trim().ToLower();


                List<string> filtros = null;
                bool and = false;
                if (input.Contains(" o "))
                {
                    if (input.Contains(" y "))
                    {
                        resultado.AddErrorPublico("Formato invalido");
                        return resultado;
                    }

                    and = false;
                    filtros = input.Split(new[] { " o " }, StringSplitOptions.None).ToList();
                }
                else if (input.Contains(" y "))
                {
                    if (input.Contains(" o "))
                    {
                        resultado.AddErrorPublico("Formato invalido");
                        return resultado;
                    }

                    and = true;
                    filtros = input.Split(new[] { " y " }, StringSplitOptions.None).ToList();
                }
                else
                {
                    filtros = new List<string>() { input };
                }

                var ids = new List<int>();

                var primero = true;
                foreach (var filtro in filtros)
                {
                    var f = filtro.Trim().ToLower();
                    var resultadoIds = ProcesarFiltroBusquedaGlobal(f);
                    if (!resultadoIds.Ok)
                    {
                        resultado.Copy(resultadoIds.Errores);
                        return resultado;
                    }

                    if (primero)
                    {
                        ids = resultadoIds.Return;
                        primero = false;
                    }
                    else
                    {
                        if (and)
                        {
                            ids = ids.Intersect(resultadoIds.Return).ToList();
                        }
                        else
                        {
                            ids.AddRange(resultadoIds.Return);
                        }
                    }
                }

                ids = ids.Distinct().ToList();
                resultado.Return = ids;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;

        }

        private Result<List<int>> ProcesarFiltroBusquedaGlobal(string input)
        {
            input="referente:"+input;
            var resultado = new Result<List<int>>();

            try
            {
                input = input.Trim().ToLower();

                if (input.StartsWith("barrio:") || input.StartsWith("barrios:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_barrio;" + parametros;
                }
                else if (input.StartsWith("cpc:") || input.StartsWith("cpcs:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_cpc;" + parametros;
                }
                else if (input.StartsWith("area:") || input.StartsWith("areas:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }
                    input = "_area;" + parametros;
                }
                else if (input.StartsWith("motivo:") || input.StartsWith("motivos:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_motivo;" + parametros;
                }
                else if (input.StartsWith("servicio:") || input.StartsWith("servicios:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_servicio;" + parametros;
                }
                else if (input.StartsWith("referente:") || input.StartsWith("referentes:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_user_referente;" + parametros;
                }
                else if (input.StartsWith("creador:") || input.StartsWith("creadores:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;

                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_user_creador;" + parametros;
                }
                else if (input.StartsWith("estado:") || input.StartsWith("estados:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }

                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }

                    input = "_estado;" + parametros;
                }
                else if (input.StartsWith("origen:") || input.StartsWith("origenes:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }
                        parametros += p.Trim();
                    }
                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }
                    input = "_origen;" + parametros;
                }
                else if (input.StartsWith("fecha:"))
                {
                    string parametros = "";
                    var listaPalabras = input.Split(':')[1].Trim().ToLower().Split(',').SelectMany(x => x.Trim().Split(' ')).ToList();
                    foreach (string p in listaPalabras)
                    {
                        if (p.Trim() == "") continue;
                        if (!parametros.Equals(""))
                        {
                            parametros += "|";
                        }

                        var fecha = DateTime.ParseExact(p.Trim(), "dd/MM/yyyy", null);
                        parametros += fecha.ToString("MM/dd/yyyy");
                    }
                    if (parametros == "")
                    {
                        resultado.Return = new List<int>();
                        return resultado;
                    }
                    input = "_dia;" + parametros;
                }


                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_BusquedaGlobal @input=:input");
                query.SetString("input", input);

                var ids = new List<int>();
                query.List<object>().ToList().ForEach(x => ids.Add(int.Parse(("" + x))));
                resultado.Return = ids;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<Resultado_RequerimientoDetalle2> GetDetalleById(int id, int? idUsuario)
        {
            var resultado = new Result<Resultado_RequerimientoDetalle2>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle @id=:id,@idUsuario=:idUsuario");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2>());
                query.SetInt32("id", id);
                query.SetInt32("idUsuario", idUsuario.HasValue ? idUsuario.Value : -1);
                var data = query.List<Resultado_RequerimientoDetalle2>().ToList();
                if (data.Count == 0 || data[0].Id == 0)
                {
                    resultado.Return = null;
                }
                else
                {
                    resultado.Return = data[0];
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_RequerimientoDetalle2_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_HistoricoEstados>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_HistoricoEstados @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_HistoricoEstados>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_HistoricoEstados>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_RequerimientoDetalle2_Comentario>> GetDetalleComentariosById(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_Comentario>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_Comentarios @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_Comentario>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_Comentario>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo>> GetDetalleHistoricoOrdenesTrabajoById(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_HistoricoOrdenesTrabajo @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
        //public Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion>> GetDetalleHistoricoOrdenesInspeccionById(int id)
        //{
        //    var resultado = new Result<List<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion>>();

        //    try
        //    {
        //        IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_HistoricoOrdenesInspeccion @id=:id");
        //        query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion>());
        //        query.SetInt32("id", id);
        //        resultado.Return = query.List<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion>().ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorInterno(e);
        //    }
        //    return resultado;
        //}

        public Result<List<Resultado_RequerimientoDetalle2_UsuarioReferente>> GetDetalleUsuariosReferentes(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_UsuarioReferente>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_UsuariosReferentes @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_UsuarioReferente>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_UsuarioReferente>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
        public Result<List<Resultado_RequerimientoDetalle2_Tarea>> GetDetalleTareasById(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_Tarea>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_Tareas @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_Tarea>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_Tarea>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_RequerimientoDetalle2_CampoDinamico>> GetDetalleCamposDinamicosById(int id)
        {
            var resultado = new Result<List<Resultado_RequerimientoDetalle2_CampoDinamico>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_Detalle_CamposDinamicos @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoDetalle2_CampoDinamico>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_RequerimientoDetalle2_CampoDinamico>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsByNumero(string numero, int? año)
        {
            var resultado = new Result<List<int>>();

            try
            {
                var query = GetSession().QueryOver<Requerimiento>();
                query.Where(x => x.Numero == numero);
                if (año.HasValue && año.Value > 0)
                {
                    query.Where(x => x.Año == año.Value);
                }
                resultado.Return = query.Select(x => x.Id).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_RequerimientoInfo>> GetInfoGlobal()
        {
            var resultado = new Result<List<Resultado_RequerimientoInfo>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Requerimiento_InfoCba");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_RequerimientoInfo>());
                var data = query.List<Resultado_RequerimientoInfo>().ToList();
                if (data.Count == 0 || data[0].Id == 0)
                {
                    resultado.AddErrorPublico("Error procesando la solicitud");
                }
                else
                {
                    resultado.Return = data;
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }



        //Varios

        public Result<List<object[]>> GetReporteListado(List<int> ids)
        {
            var result = new Result<List<object[]>>();

            try
            {

                var resultado = new Result<List<object[]>>();

                using (var session = SessionManager.Instance.SessionFactory.OpenSession())
                {
                    session.SetBatchSize(1000);


                    using (var trans = session.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            var sqlInicio = @"
                          SELECT
                        (r.Numero + '/' + CONVERT(varchar, r.Anio)) as numero,
                        r.FechaAlta as fecha,
                        e.Nombre as estado,
                        m.Nombre as motivo,
                        (c.Nombre + ' ' + convert(varchar,d.Altura) + ' B°' + b.Nombre) AS barrio,
                        
                        a.Id as idArea,
                        a.Nombre as area

                        FROM
                        dbo.Requerimiento r
                        INNER JOIN Motivo m ON r.IdMotivo = m.Id
                        LEFT JOIN Domicilio d ON r.IdDomicilio = d.Id
                        LEFT JOIN Barrio b ON d.IdBarrio = b.Id
                        INNER JOIN EstadoRequerimientoHistorial eh ON eh.IdRequerimiento = r.Id
                        INNER JOIN EstadoRequerimiento e ON eh.IdEstado = e.Id
                        INNER JOIN CerrojoArea a ON r.IdAreaCerrojoResponsable = a.Id
                        LEFT JOIN dbo.Calle AS c ON d.IdCalle = c.Id
                        inner join                       
                        (
                            select -1 as Id2";

                            sb.Append(sqlInicio);

                            foreach (var id in ids)
                            {
                                sb.Append(" union all select " + id + " ");
                            }

                            var sqlFin = @"
                        ) as x on r.Id = x.Id2
                        where eh.Ultimo = 1 AND
                        r.FechaBaja is null 
                        ORDER BY r.FechaAlta DESC";

                            sb.Append(sqlFin);

                            var sql = sb.ToString();
                            var resultadoData = session.CreateSQLQuery(sql).List<object[]>();

                            trans.Commit();

                            resultado.Return = resultadoData.ToList();
                            return resultado;
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();

                            resultado.Return = null;
                            resultado.AddErrorInterno(e);
                            return resultado;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        //        public Result<List<object[]>> GetReporteListado(List<int> ids)
        //        {
        //            var result = new Result<List<object[]>>();

        //            try
        //            {
        //                var idsString = "(" + string.Join(",", ids) + ")";

        //                var sql = @"
        //                      SELECT
        //                        (r.Numero + '/' + CONVERT(varchar, r.Anio)) as numero,
        //                        r.FechaAlta as fecha,
        //                        e.Nombre as estado,
        //                        m.Nombre as motivo,
        //                        (c.Nombre + ' ' + convert(varchar,d.Altura) + ' B°' + b.Nombre) AS barrio,
        //                        
        //                        a.Id as idArea,
        //                        a.Nombre as area
        //
        //                        FROM
        //                        dbo.Requerimiento r
        //                        INNER JOIN Motivo m ON r.IdMotivo = m.Id
        //                        LEFT JOIN Domicilio d ON r.IdDomicilio = d.Id
        //                        LEFT JOIN Barrio b ON d.IdBarrio = b.Id
        //                        INNER JOIN EstadoRequerimientoHistorial eh ON eh.IdRequerimiento = r.Id
        //                        INNER JOIN EstadoRequerimiento e ON eh.IdEstado = e.Id
        //                        INNER JOIN CerrojoArea a ON r.IdAreaCerrojoResponsable = a.Id
        //                        INNER JOIN dbo.Calle AS c ON d.IdCalle = c.Id
        //                        
        //                        WHERE
        //                        eh.Ultimo = 1 AND
        //                        r.Id in " + idsString + @" 
        //                        ORDER BY r.FechaAlta DESC";

        //                var query = GetSession().CreateSQLQuery(sql);
        //                result.Return = query.List<object[]>().ToList();
        //            }
        //            catch (Exception e)
        //            {
        //                result.AddErrorInterno(e);
        //            }

        //            return result;
        //        }
        public Result<bool> ExisteNumero(string numero, int año)
        {
            var result = new Result<bool>();

            try
            {
                var query = GetSession().QueryOver<Requerimiento>();
                query.Where(x => x.Numero == numero);
                query.Where(x => x.Año == año);
                result.Return = query.RowCount() != 0;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<string> GetMailReferente(int idReclamo)
        {
            var result = new Result<string>();

            try
            {
                var sql = @"
                SELECT
                p.Mail as Mail
                FROM
                Requerimiento r
                INNER JOIN PersonaFisica p ON r.IdPersonaFisica = p.Id
                WHERE
                r.Id =" + idReclamo;

                var query = GetSession().CreateSQLQuery(sql);

                result.Return = query.List<string>()[0];
            }
            //Probar esto de abajo, evita ewl sql injection

                //                r.Id = ?" ;
            //var query = GetSession().CreateSQLQuery(sql).SetParameter(1,idReclamo);

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresGoogleMaps(List<int> ids)
        {
            var resultado = new Result<List<Resultado_MarcadorGoogleMaps>>();

            using (var session = SessionManager.Instance.SessionFactory.OpenSession())
            {
                session.SetBatchSize(1000);

                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        var sqlInicio = @"
                        Select 
                            r.Id as Id, 
                            r.Numero as Numero, 
                            r.Anio as Año, 
                            d.Latitud as Latitud, 
                            d.Longitud as Longitud, 
                            b.IdCatastro as BarrioId,
                            b.Nombre as BarrioNombre,
                            er.Nombre as EstadoNombre, 
                            er.Color as EstadoColor, 
                            er.KeyValue as EstadoKeyValue, 
                            d.Id as DomicilioId,
                            a.Id as AreaId,
                            a.Nombre as AreaNombre
                        from Requerimiento r 
                        inner join Domicilio d on d.Id = r.IdDomicilio
                        inner join Barrio b on b.Id = d.IdBarrio
                        inner join EstadoRequerimientoHistorial erh on erh.IdRequerimiento = r.Id
                        inner join EstadoRequerimiento er on er.Id = erh.IdEstado
                        inner join CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable
                        inner join                       
                        (
                            select -1 as Id2";

                        sb.Append(sqlInicio);

                        foreach (var id in ids)
                        {
                            sb.Append(" union all select " + id + " ");
                        }

                        var sqlFin = @"
                        ) as x on r.Id = x.Id2
                        where r.FechaBaja is null and erh.Ultimo = 1 and d.Latitud is not null and d.Longitud is not null";
                        sb.Append(sqlFin);

                        IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                        query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_MarcadorGoogleMaps)));
                        var data = new List<Resultado_MarcadorGoogleMaps>();
                        var info = query.List<Resultado_MarcadorGoogleMaps>().ToList();
                        if (resultado != null && info.Count != 0 && info[0].Id != 0)
                        {
                            data.AddRange(info);
                        }


                        //var marcadores = new List<Resultado_MarcadorGoogleMaps>();

                        //System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
                        //{
                        //    NumberDecimalSeparator = ",",
                        //};
                        //var data = query.List<Resultado_MarcadorGoogleMaps>().ToList();


                        //foreach (var item in resultadoData)
                        //{
                        //    var marcador = new Resultado_MarcadorGoogleMaps();
                        //    marcador.Id = int.Parse("" + item[0]);
                        //    marcador.IdRequerimiento = int.Parse("" + item[0]);
                        //    marcador.IdDomicilio = int.Parse("" + item[8]);
                        //    marcador.Titulo = ("" + item[1] + "/") + (int.Parse("" + item[2]));
                        //    marcador.Descripcion = ("Estado: " + item[5]);
                        //    marcador.Latitud = double.Parse(("" + item[3]).Replace(".", ","), nf);
                        //    marcador.Longitud = double.Parse(("" + item[4]).Replace(".", ","), nf);
                        //    marcador.Descripcion = "" + item[5];
                        //    marcador.Color = "#" + item[6];
                        //    marcadores.Add(marcador);
                        //}

                        trans.Commit();

                        resultado.Return = data;
                        return resultado;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();

                        resultado.Return = null;
                        resultado.AddErrorInterno(e);
                        return resultado;
                    }
                }
            }
        }

        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresLatitudLongitud(List<int> ids)
        {
            var resultado = new Result<List<Resultado_MarcadorGoogleMaps>>();

            using (var session = SessionManager.Instance.SessionFactory.OpenSession())
            {
                session.SetBatchSize(1000);

                using (var trans = session.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        var sqlInicio = @"
                        Select 
                        d.Latitud as Latitud, 
                        d.Longitud as Longitud
                       
                        from Requerimiento r 
                        inner join Domicilio d on d.Id = r.IdDomicilio
                        inner join                       
                        (
                            select -1 as Id2";

                        sb.Append(sqlInicio);

                        foreach (var id in ids)
                        {
                            sb.Append(" union all select " + id + " ");
                        }

                        var sqlFin = @"
                        ) as x on r.Id = x.Id2
                        where r.FechaBaja is null and d.Latitud is not null and d.Longitud is not null";
                        sb.Append(sqlFin);

                        IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                        query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_MarcadorGoogleMaps)));
                        var data = new List<Resultado_MarcadorGoogleMaps>();
                        var info = query.List<Resultado_MarcadorGoogleMaps>().ToList();
                        if (resultado != null && info.Count != 0 && info[0].Id != 0)
                        {
                            data.AddRange(info);
                        }


                        //var marcadores = new List<Resultado_MarcadorGoogleMaps>();

                        //System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo()
                        //{
                        //    NumberDecimalSeparator = ",",
                        //};
                        //var data = query.List<Resultado_MarcadorGoogleMaps>().ToList();


                        //foreach (var item in resultadoData)
                        //{
                        //    var marcador = new Resultado_MarcadorGoogleMaps();
                        //    marcador.Id = int.Parse("" + item[0]);
                        //    marcador.IdRequerimiento = int.Parse("" + item[0]);
                        //    marcador.IdDomicilio = int.Parse("" + item[8]);
                        //    marcador.Titulo = ("" + item[1] + "/") + (int.Parse("" + item[2]));
                        //    marcador.Descripcion = ("Estado: " + item[5]);
                        //    marcador.Latitud = double.Parse(("" + item[3]).Replace(".", ","), nf);
                        //    marcador.Longitud = double.Parse(("" + item[4]).Replace(".", ","), nf);
                        //    marcador.Descripcion = "" + item[5];
                        //    marcador.Color = "#" + item[6];
                        //    marcadores.Add(marcador);
                        //}

                        trans.Commit();

                        resultado.Return = data;
                        return resultado;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();

                        resultado.Return = null;
                        resultado.AddErrorInterno(e);
                        return resultado;
                    }
                }
            }
        }


        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_Requerimiento>>();

            if (ids == null || ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Requerimiento>();
                resultadoTabla.Data = new List<ResultadoTabla_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }            
            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_Requerimiento>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                r.Id as Id,
	                r.FechaAlta as FechaAlta,
	                r.Prioridad as Prioridad,
	                r.Numero  as Numero,
                    r.Anio as Año,	                             
	                s.Id as ServicioId,                        
  	                s.Nombre as ServicioNombre,
  	                m.Id as MotivoId,
  	                m.Nombre as MotivoNombre,
                    m.Tipo as MotivoTipo,
                    cat.Id as CategoriaId,
  	                cat.Nombre as CategoriaNombre,
  	                m.Urgente as Urgente,
  	                a.Id as AreaId,
  	                a.Nombre as AreaNombre,
  	                e.Id as EstadoId,
  	                e.Nombre as EstadoNombre,
  	                e.Color as EstadoColor,
  	                e.KeyValue as EstadoKeyValue,
	                b.Nombre as BarrioNombre,
                    cpc.Nombre as CpcNombre,
                    cpc.Numero as CpcNumero,
 	                d.Observaciones as DomicilioObservaciones,
 	                d.Direccion as DomicilioDireccion,
                    d.Longitud as DomicilioLatitud,
                    d.Latitud as DomicilioLongitud,
 	                r.Marcado as Marcado,
	                oe.Id as OrdenEspecialId,
 	                eoe.Id as EstadoOrdenEspecialId,
 	                eoe.Nombre as EstadoOrdenEspecialNombre,
 	                eoe.Color as EstadoOrdenEspecialColor,
 	                eoe.KeyValue as EstadoOrdenEspecialKeyValue,
                            DATEDIFF(DAY, eh.FechaAlta , GETDATE()) as Dias,
                    cast(IIF((Select count(*) from RequerimientoPorOrdenInspeccion rxot2 inner join OrdenInspeccion oi2 on oi2.Id = rxot2.IdOrdenInspeccion inner join EstadoOrdenInspeccionHistorial eoth2 on eoth2.IdOrdenInspeccion = oi2.Id and eoth2.Ultimo = 1 inner join EstadoOrdenInspeccion eot2 on eot2.Id = eoth2.IdEstado where rxot2.IdRequerimiento = r.Id and eot2.KeyValue!=1) > 0,1,0) as bit) as Inspeccionado,

                    iif(m.Tipo=2, SUBSTRING(
                        (
                            SELECT '/ '+dxr.Descripcion + ' '  AS [text()]
                            FROM DescripcionPorRequerimiento dxr
                            WHERE dxr.IdRequerimiento= r.Id
                            FOR XML PATH ('')
                        ), 3, 1000), '') as Descripcion

	                FROM
 	                Requerimiento r
 	                INNER JOIN EstadoRequerimientoHistorial eh ON eh.IdRequerimiento = r.Id
 	                INNER JOIN EstadoRequerimiento e ON eh.IdEstado = e.Id
 	                left JOIN RequerimientoPorOrdenEspecial oe ON r.Id=oe.IdRequerimiento
 	                left JOIN OrdenEspecial orden ON orden.Id=oe.IdOrdenEspecial
	                left join EstadoOrdenEspecialHistorial eoeh on eoeh.IdOrdenEspecial=orden.Id
	                left join EstadoOrdenEspecial eoe on eoeh.IdEstado=eoe.Id
 	                INNER JOIN Motivo m ON r.IdMotivo = m.Id
                    left join CategoriaMotivoArea cat on cat.Id=m.IdCategoriaMotivoArea  
	                INNER JOIN Tema t ON t.Id = m.IdTema                        
	                INNER JOIN Servicio s on s.Id  = t.IdServicio
	                INNER JOIN CerrojoArea a ON r.IdAreaCerrojoResponsable = a.Id
	                LEFT JOIN Domicilio d ON d.Id = r.IdDomicilio
	                LEFT JOIN Barrio b ON b.Id = d.IdBarrio
                    LEFT JOIN Cpc cpc on cpc.Id = d.IdCpc
	                inner join (
 	                    select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on r.Id = x.Id2
	                WHERE eh.Ultimo = 1 AND (eoeh.Ultimo = 1 or eoeh.Ultimo is null)
	                ORDER BY r.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_Requerimiento)));
                var resultado = query.List<ResultadoTabla_Requerimiento>().ToList();
                if (resultado != null && resultado.Count != 0 && resultado[0].Id != 0)
                {
                    data.AddRange(resultado);
                }

                resultadoTabla.Data = data;
                resultadoTabla.Cantidad = ids.Count();
                result.Return = resultadoTabla;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<ResultadoTabla<ResultadoReporte_Requerimiento>> GetResultadoReporteByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoReporte_Requerimiento>>();

            if (ids == null || ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoReporte_Requerimiento>();
                resultadoTabla.Data = new List<ResultadoReporte_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }
            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoReporte_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoReporte_Requerimiento>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                r.Id as Id,
	                r.FechaAlta as FechaAlta,
	                r.Prioridad as Prioridad,
	                r.Numero  as Numero,
                    r.Anio as Año,	                             
	                s.Id as ServicioId,                        
  	                s.Nombre as ServicioNombre,
  	                m.Id as MotivoId,
  	                m.Nombre as MotivoNombre,
                    m.Tipo as MotivoTipo,
                    cat.Id as CategoriaId,
  	                cat.Nombre as CategoriaNombre,
  	                m.Urgente as Urgente,
  	                a.Id as AreaId,
  	                a.Nombre as AreaNombre,
  	                e.Id as EstadoId,
  	                e.Nombre as EstadoNombre,
  	                e.Color as EstadoColor,
  	                e.KeyValue as EstadoKeyValue,
	                b.Nombre as BarrioNombre,
                    cpc.Nombre as CpcNombre,
                    cpc.Numero as CpcNumero,
 	                d.Observaciones as DomicilioObservaciones,
 	                d.Direccion as DomicilioDireccion,
                    d.Longitud as DomicilioLatitud,
                    d.Latitud as DomicilioLongitud,
 	                r.Marcado as Marcado,
	                oe.Id as OrdenEspecialId,
 	                eoe.Id as EstadoOrdenEspecialId,
 	                eoe.Nombre as EstadoOrdenEspecialNombre,
 	                eoe.Color as EstadoOrdenEspecialColor,
 	                eoe.KeyValue as EstadoOrdenEspecialKeyValue,
                            DATEDIFF(DAY, eh.FechaAlta , GETDATE()) as Dias,
                    cast(IIF((Select count(*) from RequerimientoPorOrdenInspeccion rxot2 inner join OrdenInspeccion oi2 on oi2.Id = rxot2.IdOrdenInspeccion inner join EstadoOrdenInspeccionHistorial eoth2 on eoth2.IdOrdenInspeccion = oi2.Id and eoth2.Ultimo = 1 inner join EstadoOrdenInspeccion eot2 on eot2.Id = eoth2.IdEstado where rxot2.IdRequerimiento = r.Id and eot2.KeyValue!=1) > 0,1,0) as bit) as Inspeccionado,

                    iif(m.Tipo=2, SUBSTRING(
                        (
                            SELECT '/ '+dxr.Descripcion + ' '  AS [text()]
                            FROM DescripcionPorRequerimiento dxr
                            WHERE dxr.IdRequerimiento= r.Id
                            FOR XML PATH ('')
                        ), 3, 1000), '') as Descripcion

	                FROM
 	                Requerimiento r
 	                INNER JOIN EstadoRequerimientoHistorial eh ON eh.IdRequerimiento = r.Id
 	                INNER JOIN EstadoRequerimiento e ON eh.IdEstado = e.Id
 	                left JOIN RequerimientoPorOrdenEspecial oe ON r.Id=oe.IdRequerimiento
 	                left JOIN OrdenEspecial orden ON orden.Id=oe.IdOrdenEspecial
	                left join EstadoOrdenEspecialHistorial eoeh on eoeh.IdOrdenEspecial=orden.Id
	                left join EstadoOrdenEspecial eoe on eoeh.IdEstado=eoe.Id
 	                INNER JOIN Motivo m ON r.IdMotivo = m.Id
                    left join CategoriaMotivoArea cat on cat.Id=m.IdCategoriaMotivoArea  
	                INNER JOIN Tema t ON t.Id = m.IdTema                        
	                INNER JOIN Servicio s on s.Id  = t.IdServicio
	                INNER JOIN CerrojoArea a ON r.IdAreaCerrojoResponsable = a.Id
	                LEFT JOIN Domicilio d ON d.Id = r.IdDomicilio
	                LEFT JOIN Barrio b ON b.Id = d.IdBarrio
                    LEFT JOIN Cpc cpc on cpc.Id = d.IdCpc
	                inner join (
 	                    select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on r.Id = x.Id2
	                WHERE eh.Ultimo = 1 AND (eoeh.Ultimo = 1 or eoeh.Ultimo is null)
	                ORDER BY r.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoReporte_Requerimiento)));
                var resultado = query.List<ResultadoReporte_Requerimiento>().ToList();
                if (resultado != null && resultado.Count != 0 && resultado[0].Id != 0)
                {
                    data.AddRange(resultado);
                }

                resultadoTabla.Data = data;
                resultadoTabla.Cantidad = ids.Count();
                result.Return = resultadoTabla;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<ResultadoTabla<ResultadoTabla_RequerimientoExportar>> GetResultadoTablaByIdsExportar(List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_RequerimientoExportar>>();

            if (ids == null || ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_RequerimientoExportar>();
                resultadoTabla.Data = new List<ResultadoTabla_RequerimientoExportar>();
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_RequerimientoExportar>();
                

                var data = new List<ResultadoTabla_RequerimientoExportar>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT	              
	                r.FechaAlta as FechaAlta,	               
	                r.Numero  as Numero,
	                r.Anio as Año,
                    r.Descripcion,	          
  	                m.Nombre as MotivoNombre,  	              
 	                d.Observaciones as DomicilioObservaciones,
 	                d.Direccion as DomicilioDireccion,
                    b.Nombre as BarrioNombre
                    FROM
 	                Requerimiento r 	                
 	                INNER JOIN Motivo m ON r.IdMotivo = m.Id	               
	                LEFT JOIN Domicilio d ON d.Id = r.IdDomicilio
	                LEFT JOIN Barrio b ON b.Id = d.IdBarrio                    
	                inner join (
 	                    select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on r.Id = x.Id2	                
	                ORDER BY r.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_RequerimientoExportar)));
                var resultado = query.List<ResultadoTabla_RequerimientoExportar>().ToList();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);
                }

                resultadoTabla.Data = data;
                resultadoTabla.Cantidad = ids.Count();
                result.Return = resultadoTabla;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<ResultadoTabla<ResultadoWSExterno_Requerimiento>> GetResultadoExternoByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoWSExterno_Requerimiento>>();

            if (ids == null || ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoWSExterno_Requerimiento>();
                resultadoTabla.Data = new List<ResultadoWSExterno_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoWSExterno_Requerimiento>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoWSExterno_Requerimiento>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                r.Id as Id,
	                r.FechaAlta as FechaAlta,
	                r.Numero  as Numero,
	                r.Anio as Año,
  	                m.Id as MotivoId,
  	                m.Nombre as MotivoNombre,
  	                e.Id as EstadoId,
  	                e.Nombre as EstadoNombre,
  	                e.KeyValue as EstadoKeyValue,
	                b.Nombre as BarrioNombre,
                    cpc.Nombre as CpcNombre,
                    cpc.Numero as CpcNumero,
 	                d.Observaciones as DomicilioObservaciones,
 	                d.Direccion as DomicilioDireccion,
                    d.Longitud as DomicilioLatitud,
                    d.Latitud as DomicilioLongitud,
                    u.Id as UsuarioCreadorId,
                    u.Nombre as UsuarioCreadorNombre,
                    u.Apellido as UsuarioCreadorApellido,
                    u.TelefonoFijo as UsuarioCreadorTelefonoFijo,
                    u.TelefonoCelular as UsuarioCreadorTelefonoCelular,
                    u.Email as UsuarioCreadorEmail,
                    r.IdAreaCerrojoResponsable as AreaId,
    SUBSTRING(
        (
            SELECT '/ '+dxr.Descripcion +' '  AS [text()]
            FROM DescripcionPorRequerimiento dxr
            WHERE dxr.IdRequerimiento= r.Id
            FOR XML PATH ('')
        ), 3, 1000) as Descripcion,
	                case when u.SexoMasculino = 0 then 'Mujer' else 'Hombre' end as UsuarioCreadorGenero 
	      
	                FROM
 	                Requerimiento r
 	                INNER JOIN EstadoRequerimientoHistorial eh ON eh.IdRequerimiento = r.Id
 	                INNER JOIN EstadoRequerimiento e ON eh.IdEstado = e.Id 	         
 	                INNER JOIN Motivo m ON r.IdMotivo = m.Id
	                LEFT JOIN Domicilio d ON d.Id = r.IdDomicilio
	                LEFT JOIN Barrio b ON b.Id = d.IdBarrio
                    LEFT JOIN Cpc cpc on cpc.Id = d.IdCpc
                    left join UsuarioReferentePorRequerimiento uxrq on uxrq.Id=(select top 1 uxr2.Id from UsuarioReferentePorRequerimiento uxr2 where uxr2.IdRequerimiento=r.Id )
                    left join VecinoVirtualUsuario u on uxrq.IdUsuarioReferente=u.Id
	                inner join (
 	                    select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on r.Id = x.Id2
	                WHERE eh.Ultimo = 1
	                ORDER BY r.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoWSExterno_Requerimiento)));
                var resultado = query.List<ResultadoWSExterno_Requerimiento>().ToList();
                if (resultado != null && resultado.Count != 0 && resultado[0].Id != 0)
                {
                    data.AddRange(resultado);
                }

                resultadoTabla.Data = data;
                resultadoTabla.Cantidad = ids.Count();
                result.Return = resultadoTabla;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Resultado_Requerimiento>> GetResultadoByIds(List<int> ids)
        {
            var result = new Result<List<Resultado_Requerimiento>>();

            if (ids.Count == 0)
            {
                result.Return = new List<Resultado_Requerimiento>();
                return result;
            }

            try
            {
                result.Return = Resultado_Requerimiento.ToList(GetSession().QueryOver<Requerimiento>().Where(x => x.Id.IsIn(ids)).List().ToList());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }


    }
}