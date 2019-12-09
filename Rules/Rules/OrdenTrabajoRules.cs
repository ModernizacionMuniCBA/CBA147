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
    public class OrdenTrabajoRules : BaseRules<OrdenTrabajo>
    {

        private readonly OrdenTrabajoDAO dao;

        public OrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = OrdenTrabajoDAO.Instance;
        }


        private static Random random = new Random();


        public Result<Resultado_OrdenTrabajoInit> Init(List<int> idsRequerimientos)
        {
            var resultado = new Result<Resultado_OrdenTrabajoInit>();

            //Busco los rqs
            var rqs = new List<Resultado_Requerimiento>();
            var idArea = -1;

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

                if (resultadoRequerimiento.Return == null)
                {
                    resultado.AddErrorPublico("Un requerimiento seleccionado no existe");
                    return resultado;
                }

                if (resultadoRequerimiento.Return.OrdenTrabajoActiva != null)
                {
                    resultado.AddErrorPublico("Uno de los requerimientos seleccionados está en una orden de trabajo, por lo que no puede formar parte de otra.");
                    return resultado;
                }

                if (idArea == -1)
                {
                    idArea = resultadoRequerimiento.Return.AreaResponsable.Id;
                }

                if (idArea != resultadoRequerimiento.Return.AreaResponsable.Id)
                {
                    resultado.AddErrorPublico("Los requerimientos seleccionados no son del area indicada");
                    return resultado;
                }


                rqs.Add(new Resultado_Requerimiento(resultadoRequerimiento.Return));
            }

            //Busco el area
            var rulesArea = new _CerrojoAreaRules(getUsuarioLogueado());
            var resultadoArea = rulesArea.GetById(idArea);
            if (!resultadoArea.Ok)
            {
                resultado.Copy(resultadoArea.Errores);
                return resultado;
            }

            if (resultadoArea.Return == null)
            {
                resultado.AddErrorPublico("El area indicada no existe");
                return resultado;
            }

            var area = new Resultado_Area(resultadoArea.Return);

            var estadosRQ = new List<Resultado_EstadoRequerimiento>();

            if (getUsuarioLogueado().EsAmbitoCPC())
            {
                //busco el estado inspeccion, que es el unico al que pueden pasar los rq si la ot la hace un cpc
                var resultadoEstadoInspeccion = new EstadoRequerimientoRules(getUsuarioLogueado()).GetByKeyValue(Enums.EstadoRequerimiento.INSPECCION);
                if (!resultadoEstadoInspeccion.Ok)
                {
                    resultado.Copy(resultadoEstadoInspeccion.Errores);
                    return resultado;
                }

                estadosRQ.Add(new Resultado_EstadoRequerimiento(resultadoEstadoInspeccion.Return));

                resultado.Return = new Resultado_OrdenTrabajoInit()
                {
                    Requerimientos = rqs,
                    Area = area,
                    EstadosRequerimiento = estadosRQ
                };
                return resultado;
            }

            //Busco las secciones
            var resultadoSecciones = new SeccionRules(getUsuarioLogueado()).GetByFilters(new Consulta_Seccion(null, false, idArea));
            if (!resultadoSecciones.Ok && resultadoSecciones.Return == null)
            {
                resultado.Copy(resultadoSecciones.Errores);
                return resultado;
            }

            var secciones = resultadoSecciones.Return;

            //Busco cantidad de moviles para el area
            var rulesMoviles = new MovilRules(getUsuarioLogueado());
            var resultadoCantidadMoviles = rulesMoviles.GetCantidadParaAgregarAOT(idArea);
            if (!resultadoCantidadMoviles.Ok)
            {
                resultado.Copy(resultadoCantidadMoviles.Errores);
                return resultado;
            }

            var cantidadMoviles = resultadoCantidadMoviles.Return;

            //Busco cantidad de personal para el area
            var resultadoCantidadPersonal = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetCantidadParaAgregarAOT(idArea);
            if (!resultadoCantidadPersonal.Ok)
            {
                resultado.Copy(resultadoCantidadPersonal.Errores);
                return resultado;
            }

            var cantidadPersonal = resultadoCantidadPersonal.Return;

            //Busco cantidad de flotas para el area
            var resultadoCantidadFlotas = new FlotaRules(getUsuarioLogueado()).GetCantidadParaAgregarAOT(idArea);
            if (!resultadoCantidadFlotas.Ok)
            {
                resultado.Copy(resultadoCantidadPersonal.Errores);
                return resultado;
            }

            var cantidadFlotas = resultadoCantidadFlotas.Return;


            //Busco los estados a los que puede pasar el rq luego si estoy en municiopalidad
            var resultadoEstadosRQ = new EstadoRequerimientoRules(getUsuarioLogueado()).GetEstadosAlEntrarAOT();
            if (!resultadoEstadosRQ.Ok)
            {
                resultado.Copy(resultadoEstadosRQ.Errores);
                return resultado;
            }

            estadosRQ = resultadoEstadosRQ.Return;

            //Busco los estados a los que puede pasar el rq luego si estoy en municiopalidad
            var resultadoEstadosOT = new EstadoOrdenTrabajoRules(getUsuarioLogueado()).GetEstadosValidosParaCrear();
            if (!resultadoEstadosOT.Ok)
            {
                resultado.Copy(resultadoEstadosOT.Errores);
                return resultado;
            }

            var estadosOT = Resultado_EstadoOrdenTrabajo.ToList(resultadoEstadosOT.Return);

            var resultadoOrdenTrabajo = new Resultado_OrdenTrabajoInit()
            {
                Requerimientos = rqs,
                Area = area,
                Secciones = secciones,
                CantidadMoviles = cantidadMoviles,
                CantidadPersonal = cantidadPersonal,
                CantidadFlotas = cantidadFlotas,
                EstadosOrdenTrabajo = estadosOT,
                EstadosRequerimiento = estadosRQ
            };

            var resultadoEstadoPorDefecto = new ConfiguracionEstadoCreacionOTRules(getUsuarioLogueado()).GetByIdArea(idArea);
            if (!resultadoEstadoPorDefecto.Ok)
            {
                resultado.Copy(resultadoEstadoPorDefecto.Errores);
                return resultado;
            }

            if (resultadoEstadoPorDefecto.Return != null)
            {
                resultadoOrdenTrabajo.EstadoOrdenTrabajoPorDefecto = new Resultado_EstadoOrdenTrabajo(resultadoEstadoPorDefecto.Return.EstadoCreacionOT);
            }

            resultado.Return = resultadoOrdenTrabajo;
            return resultado;
        }

        public override Result<OrdenTrabajo> Insert(OrdenTrabajo entity)
        {
            throw new Exception("Usar el otro metodo!");
        }

        public Result<OrdenTrabajo> Insert(Comando_OrdenTrabajo comando)
        {
            var result = new Result<OrdenTrabajo>();
            var entity = new OrdenTrabajo();

            dao.Transaction(() =>
            {
                //Seteo los datos del numero
                var numero = GenerarNumeroIdentificatorio();
                entity.UserAgent = comando.UserAgent;
                entity.TipoDispositivo = comando.TipoDispositivo;
                entity.Año = numero.Año;
                entity.Numero = numero.Numero;
                entity.Descripcion = comando.Descripcion;
                entity.FechaCreacion = DateTime.Now;
                entity.UsuarioCreador = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetByIdObligatorio(getUsuarioLogueado().Usuario.Id).Return;

                //Cambio a Estado en Proceso
                if (getUsuarioLogueado().EsAmbitoCPC() || !comando.KeyValueEstadoOrdenTrabajo.HasValue)
                {
                    comando.KeyValueEstadoOrdenTrabajo = Enums.EstadoOrdenTrabajo.ENPROCESO;
                }
                var resultCambiarEstado = CambiarEstado(entity, comando.KeyValueEstadoOrdenTrabajo.Value, "Orden de Trabajo creada", false);
                if (!resultCambiarEstado.Ok)
                {
                    result.Copy(resultCambiarEstado);
                    return false;
                }

                //-----------------------------------
                // Requerimientos
                //-----------------------------------

                entity.RequerimientosPorOrdenTrabajo = new List<RequerimientoPorOrdenTrabajo>();

                //Valido que tenga requerimientos
                if (comando.IdRequerimientos == null || comando.IdRequerimientos.Count == 0)
                {
                    result.AddErrorPublico("La orden de Trabajo debe contener requerimientos");
                    return false;
                }

                //Area
                var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
                if (!resultArea.Ok || resultArea.Return == null)
                {
                    result.AddErrorPublico(resultArea.Errores.ErroresPublicos);
                    return false;
                }
                entity.Area = resultArea.Return;

                //Ambito
                var resultAmbito = new _CerrojoAmbitoRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Ambito.Id);
                if (!resultAmbito.Ok || resultAmbito.Return == null)
                {
                    result.AddErrorPublico(resultAmbito.Errores.ErroresPublicos);
                    return false;
                }
                entity.Ambito = resultAmbito.Return;

                //Seccion
                if (comando.IdSeccion.HasValue)
                {
                    var secResult = new SeccionRules(getUsuarioLogueado()).GetById((int)comando.IdSeccion);
                    if (!secResult.Ok)
                    {
                        result.AddErrorPublico(secResult.Errores.ErroresPublicos);
                        return false;
                    }
                    entity.Seccion = secResult.Return;
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

                    //Valido que no debe estar en otra OT
                    if (rq.OrdenTrabajoActiva != null)
                    {
                        result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " se encuentra en la Orden de Trabajo N° " + rq.OrdenTrabajoActiva.GetNumero());
                        return false;
                    }

                    //Valido que este en un estado valido para formar parte de la OT
                    var keyValueEstado = rq.GetUltimoEstado().Estado.KeyValue;
                    var permiso = Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo;
                    var resultadoPermiso = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
                    if (!resultadoPermiso.Ok)
                    {
                        result.Copy(resultadoPermiso.Errores);
                        return false;
                    }
                    if (!resultadoPermiso.Return)
                    {
                        result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " no se encuentra en un estado valido para formar parte de la orden de trabajo.");
                        return false;
                    }


                    //Valido que el area del Reclamo sea la misma que la de la OT
                    if (rq.AreaResponsable.Id != entity.Area.Id)
                    {
                        result.AddErrorPublico("El requerimiento N° " + rq.GetNumero() + " no es del mismo area que la orden de trabajo");
                        return false;
                    }
                }

                //---------------------------------------
                // Notas
                //---------------------------------------
                var notaOrdenTrabajoRules = new NotaPorOrdenTrabajoRules(getUsuarioLogueado());
                if (entity.Notas != null && entity.Notas.Count != 0)
                {
                    foreach (NotaPorOrdenTrabajo nota in entity.Notas)
                    {
                        Result<NotaPorOrdenTrabajo> resultNota;
                        if (nota.Id == 0)
                        {
                            resultNota = notaOrdenTrabajoRules.ValidateInsert(nota);
                        }
                        else
                        {
                            resultNota = notaOrdenTrabajoRules.ValidateUpdate(nota);
                        }

                        if (!resultNota.Ok)
                        {
                            result.Copy(resultNota.Errores);
                            return false;
                        }
                    }
                }

                //Inserto
                result = base.Insert(entity);
                if (!result.Ok)
                {
                    return false;
                }

                //---------------------------------------
                // Moviles
                //---------------------------------------
                var movilXOrdenTrabajoRules = new MovilPorOrdenTrabajoRules(getUsuarioLogueado());
                var movilesList = new List<MovilPorOrdenTrabajo>();
                if (comando.Moviles != null && comando.Moviles.Count != 0)
                {
                    var movilRules = new MovilRules(getUsuarioLogueado());
                    foreach (int idMovil in comando.Moviles)
                    {
                        var resultMovil = movilRules.GetById(idMovil);
                        if (!resultMovil.Ok)
                        {
                            result.Copy(resultMovil.Errores);
                            return false;
                        }

                        var mxot = new MovilPorOrdenTrabajo();
                        mxot.Movil = resultMovil.Return;
                        mxot.OrdenTrabajo = result.Return;

                        var resultEntrar = movilRules.EntrarEnOrdenTrabajo(resultMovil.Return, result.Return.Numero + '/' + result.Return.Año);
                        if (!resultEntrar.Ok)
                        {
                            result.AddErrorPublico("Error al agregar uno de los móviles");
                            return false;
                        }

                        var resultMxot = movilXOrdenTrabajoRules.ValidateInsert(mxot);

                        if (!resultMxot.Ok)
                        {
                            result.Copy(resultMxot.Errores);
                            return false;
                        }

                        movilesList.Add(resultMxot.Return);
                    }

                    entity.MovilesPorOrdenTrabajo = movilesList;
                }



                //---------------------------------------
                // Personal
                //---------------------------------------
                var personalXOrdenTrabajoRules = new EmpleadoPorOrdenTrabajoRules(getUsuarioLogueado());
                var empleadosList = new List<EmpleadoPorOrdenTrabajo>();
                if (comando.Personal != null && comando.Personal.Count != 0)
                {
                    foreach (int idEmpleado in comando.Personal)
                    {
                        var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetById(idEmpleado);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        var resultEntrar = new EmpleadoPorAreaRules(getUsuarioLogueado()).EntrarEnOrdenTrabajo(resultEmpleado.Return, result.Return.Numero + '/' + result.Return.Año);
                        if (!resultEntrar.Ok)
                        {
                            result.AddErrorPublico("Error al agregar uno de los empleados");
                            return false;
                        }


                        var mxot = new EmpleadoPorOrdenTrabajo();
                        mxot.Empleado = resultEmpleado.Return;
                        mxot.OrdenTrabajo = result.Return;

                        var resultMxot = personalXOrdenTrabajoRules.ValidateInsert(mxot);

                        if (!resultMxot.Ok)
                        {
                            result.Copy(resultMxot.Errores);
                            return false;
                        }

                        empleadosList.Add(resultMxot.Return);
                    }

                    entity.EmpleadosPorOrdenTrabajo = empleadosList;
                }


                //---------------------------------------
                // Flotas
                //---------------------------------------
                var flotaXOrdenTrabajoRules = new FlotaPorOrdenTrabajoRules(getUsuarioLogueado());
                var flotaRules = new FlotaRules(getUsuarioLogueado());
                var flotaList = new List<FlotaPorOrdenTrabajo>();
                if (comando.Flotas != null && comando.Flotas.Count != 0)
                {
                    foreach (int idFlota in comando.Flotas)
                    {
                        var resultFlota = flotaRules.GetById(idFlota);
                        if (!resultFlota.Ok)
                        {
                            result.Copy(resultFlota.Errores);
                            return false;
                        }

                        var resultEntrar = flotaRules.EntrarEnOrdenTrabajo(resultFlota.Return, result.Return.Numero + '/' + result.Return.Año);
                        if (!resultEntrar.Ok)
                        {
                            result.AddErrorPublico("Error al agregar una de las flotas");
                            return false;
                        }

                        var fxot = new FlotaPorOrdenTrabajo();
                        fxot.Flota = resultFlota.Return;
                        fxot.OrdenTrabajo = result.Return;

                        var resultFxot = flotaXOrdenTrabajoRules.ValidateInsert(fxot);
                        if (!resultFxot.Ok)
                        {
                            result.Copy(resultFxot.Errores);
                            return false;
                        }

                        flotaList.Add(resultFxot.Return);
                    }

                    entity.FlotasPorOrdenTrabajo = flotaList;
                }

                //si estoy logueado desde un cpc, si o si el estado de los rq pasaran a inspeccion y la ot se crea en proceso
                if (getUsuarioLogueado().Ambito != null && getUsuarioLogueado().Ambito.KeyValue != 0)
                {
                    comando.KeyValueEstadoRequerimiento = Enums.EstadoRequerimiento.INSPECCION;
                    comando.KeyValueEstadoOrdenTrabajo = Enums.EstadoOrdenTrabajo.ENPROCESO;
                }

                //Una vez insertado, le seteo al Requerimiento la orden de trabajo insertada (como su OT Actual).
                //Debe ser si o si LUEGO de insertarse el objeto.
                //Ademas le cambio el estado al Reclamo, indicando que fue agregado a la OT. 
                //Tambien se llama luego de insertar, porque en la observacion del cambio de estado
                //estoy agregando el numero de la OT.
                var requerimientoXOrdenTrabajoRules = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado());
                foreach (var idRq in comando.IdRequerimientos)
                {
                    var rq = requerimientoRules.GetByIdObligatorio(idRq).Return;

                    // valido que el rq este en control del cpc o el área operativa, según corresponda
                    if (getUsuarioLogueado().EsAmbitoMunicipalidad() && rq.Marcado || getUsuarioLogueado().EsAmbitoCPC() && !rq.Marcado)
                    {
                        result.AddErrorPublico("Uno de los requerimientos no se encuentra en su control.");
                        return false;
                    }

                    //Genero el objeto RequerimientoXOrdenTrabajo
                    var rqxot = new RequerimientoPorOrdenTrabajo();
                    rqxot.Requerimiento = rq;
                    rqxot.OrdenTrabajo = result.Return;

                    //Inserto el RequerimientoXOrdenTrabajo
                    var resultInsertRQ = requerimientoXOrdenTrabajoRules.Insert(rqxot);
                    if (!resultInsertRQ.Ok)
                    {
                        result.Copy(resultInsertRQ.Errores);
                        return false;
                    }

                    //Seteo la OT activa
                    rq.OrdenTrabajoActiva = entity;

                    //si el usuario logueado es de un cpc, el estado al que pasan los rq es al de inspeccion
                    if (getUsuarioLogueado().EsAmbitoCPC())
                    {
                        comando.KeyValueEstadoRequerimiento = Enums.EstadoRequerimiento.INSPECCION;
                    }
                    else if (comando.KeyValueEstadoRequerimiento != Enums.EstadoRequerimiento.ENPROCESO && comando.KeyValueEstadoRequerimiento != Enums.EstadoRequerimiento.INSPECCION)
                    {
                        result.AddErrorPublico("Error en alguno de los datos enviados");
                        return false;
                    }

                    //Cambio el estado
                    var resultCambiarEstadoRQ = requerimientoRules.ProcesarCambioEstado(rq, comando.KeyValueEstadoRequerimiento.Value, "En Orden de Trabajo N° " + result.Return.GetNumero(), false);
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

                var hayRecurso = false;
                if (!String.IsNullOrEmpty(comando.Recursos.Material)) hayRecurso = true;
                if (!String.IsNullOrEmpty(comando.Recursos.Personal)) hayRecurso = true;
                if (!String.IsNullOrEmpty(comando.Recursos.Flota)) hayRecurso = true;
                if (!String.IsNullOrEmpty(comando.Recursos.Observaciones)) hayRecurso = true;

                if (hayRecurso)
                {
                    //Recursos
                    RecursoPorOrdenTrabajo recurso = new RecursoPorOrdenTrabajo(comando.Recursos);
                    recurso.OrdenTrabajo = result.Return;

                    var resultInsertRecurso = new RecursoPorOrdenTrabajoRules(getUsuarioLogueado()).Insert(recurso);
                    if (!resultInsertRecurso.Ok)
                    {
                        result.Copy(resultInsertRecurso.Errores);
                        return false;
                    }
                }

                return result.Ok;
            });

            return result;
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

        /* Estados */
        public Result<EstadoOrdenTrabajoHistorial> CrearEstado(OrdenTrabajo ot, Enums.EstadoOrdenTrabajo e, string observaciones)
        {
            var result = new Result<EstadoOrdenTrabajoHistorial>();

            var resultEstado = new EstadoOrdenTrabajoRules(getUsuarioLogueado()).GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                return result;
            }

            var estadoOT = new EstadoOrdenTrabajoHistorial();
            estadoOT.Fecha = DateTime.Now;
            estadoOT.FechaAlta = DateTime.Now;
            estadoOT.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoOT.Estado = resultEstado.Return;
            estadoOT.Observaciones = observaciones;
            estadoOT.OrdenTrabajo = ot;

            result.Return = estadoOT;
            return result;
        }

        public Result<OrdenTrabajo> CambiarEstado(OrdenTrabajo ot, Enums.EstadoOrdenTrabajo estado, string observaciones, bool guardarCambios)
        {
            var estadoOT = CrearEstado(ot, estado, observaciones);
            return CambiarEstado(ot, estadoOT.Return, guardarCambios);
        }

        public Result<OrdenTrabajo> CambiarEstado(OrdenTrabajo ot, EstadoOrdenTrabajoHistorial estado, bool guardarCambios)
        {

            var result = new Result<OrdenTrabajo>();

            if (ot.Estados == null)
            {
                ot.Estados = new List<EstadoOrdenTrabajoHistorial>();
            }

            if (ot.Estados != null && ot.Estados.Count != 0)
            {
                foreach (EstadoOrdenTrabajoHistorial e in ot.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estado.Ultimo = true;
            ot.Estados.Add(estado);

            if (guardarCambios)
            {
                var resultUpdate = Update(ot);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }

            result.Return = ot;
            return result;
        }

        /* Validaciones */
        public override Result<OrdenTrabajo> ValidateDatosNecesarios(OrdenTrabajo entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Numero
            if (string.IsNullOrEmpty(entity.Numero))
            {
                result.AddErrorPublico("La orden de trabajo a registrar no tiene numero");
            }

            //Area
            if (entity.Area == null)
            {
                result.AddErrorPublico("Debe indicar el area");
            }


            //Tipo Dispositivo
            if (entity.TipoDispositivo == null)
            {
                result.AddErrorPublico("Falta el tipo de dispositivo");
            }

            //User agent
            if (string.IsNullOrEmpty(entity.UserAgent))
            {
                result.AddErrorPublico("Falta el user agent");
            }

            return result;
        }

        //public Result<object> ValidarRequerimientoYNotas(OrdenTrabajo entity)
        //{
        //    var result = new Result<object>();

        //    //Requerimientos
        //    if (entity.Requerimientos == null || entity.Requerimientos.Count < 1)
        //    {
        //        result.AddErrorPublico("La Orden de Trabajo debe tener requerimientos");
        //        return result;
        //    }

        //    Area area = entity.Area;

        //    //Valido los RQ
        //    foreach (Requerimiento rq in entity.Requerimientos)
        //    {
        //        if (rq.AreaResponsable.Id != area.Id)
        //        {
        //            result.AddErrorPublico("Los requerimientos no corresponden a la misma area que la Orden de Trabajo");
        //            return result;
        //        }

        //        //Valido segun su propio Rules (datos minimos)
        //        Result<Requerimiento> resultValidacionRQ;
        //        if (rq.Id != 0)
        //        {
        //            resultValidacionRQ = RequerimientoRules.Instance.ValidateUpdate(rq);
        //        }
        //        else
        //        {
        //            resultValidacionRQ = RequerimientoRules.Instance.ValidateInsert(rq);
        //        }

        //        if (!resultValidacionRQ.Ok)
        //        {
        //            result.Copy(resultValidacionRQ);
        //            return result;
        //        }
        //    }

        //    //Valido las notas
        //    if (entity.Notas == null)
        //    {
        //        entity.Notas = new List<NotaXOrdenTrabajo>();
        //    }
        //    foreach (NotaXOrdenTrabajo nota in entity.Notas)
        //    {
        //        //Segun su propio rules
        //        Result<NotaXOrdenTrabajo> resultValidacionNota;
        //        if (nota.Id != 0)
        //        {
        //            resultValidacionNota = NotaOrdenTrabajoRules.Instance.ValidateUpdate(nota);
        //        }
        //        else
        //        {
        //            resultValidacionNota = NotaOrdenTrabajoRules.Instance.ValidateInsert(nota);
        //        }

        //        if (!resultValidacionNota.Ok)
        //        {
        //            result.Copy(resultValidacionNota);
        //            return result;
        //        }
        //    }

        //    return result;
        //}

        /* Mail */
        public Result<bool> EnviarMail(Comando_OrdenTrabajoMail comando)
        {
            var result = new Result<bool>();

            var resultOrden = new OrdenTrabajoRules(getUsuarioLogueado()).GetDetalleById(comando.Id);
            if (!resultOrden.Ok)
            {
                result.Copy(resultOrden.Errores);
                return result;
            }

            if (comando.Email == null)
            {
                result.AddErrorPublico("La orden no tiene un e-mail asociado");
                return result;
            }

            var orden = resultOrden.Return;

            try
            {
                //inicializo mi mensaje con todos los valores
                var emisorMail = "cba147@cordoba.gob.ar";
                var emisorNombre = "Municipalidad de Cordoba";
                var emisor = new MailAddress(emisorMail, emisorNombre);
                var receptorMail = comando.Email;
                var receptorNombre = "";

                //if (orden.PersonaFisica != null)
                //{
                //    receptorNombre = requerimiento.PersonaFisica.Nombre + " " + requerimiento.PersonaFisica.Apellido;
                //}
                var receptor = new MailAddress(receptorMail, receptorNombre);
                string asunto = "Orden N° " + orden.Numero + "/" + orden.Año;

                var cuerpo = "";

                var reporte = new OrdenTrabajoReporteRules(getUsuarioLogueado()).GenerarReporteOrdenTrabajoListadoRequerimientos(orden);
                if (!reporte.Ok)
                {
                    result.SetErrorPublico(reporte.MessagesPublicos);
                    result.SetErrorInterno(reporte.MessagesInternos);
                    return result;
                }

                if (reporte.Return == null)
                {
                    result.AddErrorInterno("El reporte es null");
                    return result;
                }

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult resultReporte = reportProcessor.RenderReport("PDF", reporte.Return, null);

                //Mando el mail
                MailMessage message = new MailMessage(emisor, receptor);
                message.Subject = asunto;
                message.Body = cuerpo;
                string nombreArchivo = "comprobante_" + orden.Numero + "_" + orden.Año + ".pdf";
                message.Attachments.Add(new Attachment(new MemoryStream(resultReporte.DocumentBytes), nombreArchivo));

                SmtpClient client = new SmtpClient();
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                client.Send(message);
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }

        /* Busqueda */
        public Result<List<int>> GetIds(Consulta_OrdenTrabajo consulta)
        {
            return dao.GetIds(consulta);
        }

        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaByIdEmpleado(Consulta_OrdenTrabajo consulta)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();

            var resultIds = dao.GetIdsByIdEmpleado(consulta);
            if (!resultIds.Ok)
            {
                result.Copy(resultIds.Errores);
                result.AddErrorPublico("Error al consultar los órdenes de trabajo");
                return result;
            }

            result = dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, resultIds.Return);
            if (!result.Ok)
            {
                result.AddErrorPublico("Error al consultar los órdenes de trabajo");
                return result;
            }

            return result;
        }
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetByIdFlota(int idFlota)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();
            var consulta = new Consulta_OrdenTrabajo();
            consulta.IdFlota = idFlota;

            var resultIds = dao.GetIds(consulta);
            if (!resultIds.Ok)
            {
                result.Copy(resultIds.Errores);
                result.AddErrorPublico("Error al consultar los órdenes de trabajo");
                return result;
            }

            result = dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, resultIds.Return);
            if (!result.Ok)
            {
                result.AddErrorPublico("Error al consultar los órdenes de trabajo");
                return result;
            }

            return result;
        }

        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetUltimas(int cantidad)
        {
            var consulta = new Consulta_OrdenTrabajo();
            consulta.IdAmbito = getUsuarioLogueado().Ambito.Id;
            consulta.IdsArea = getUsuarioLogueado().IdsAreas;
            var estados = new List<Enums.EstadoOrdenTrabajo>();
            estados.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            consulta.EstadosKeyValue = estados;
            return dao.GetUltimas(cantidad, consulta);
        }

        int LIMITE_CANTIDAD_TABLA = 5000;
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaByIds(List<int> ids)
        {
            return dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, ids);
        }

        public Result<ResultadoTabla_OrdenTrabajo> GetDatosTablaById(int id)
        {

            var resultado = new Result<ResultadoTabla_OrdenTrabajo>();
            var resultadoConsulta = dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, new List<int>() { id });
            if (!resultado.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return.Data != null && resultadoConsulta.Return.Data.Count != 0)
            {
                resultado.Return = resultadoConsulta.Return.Data[0];
            }

            return resultado;
        }

        //Detalle
        public Result<Resultado_OrdenTrabajoDetalle> GetDetalleById(int id)
        {
            var resultado = new Result<Resultado_OrdenTrabajoDetalle>();

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

            //Comentarios
            var resultadoComentarios = GetDetalleNotasById(id);
            if (!resultadoComentarios.Ok)
            {
                resultado.Copy(resultadoComentarios.Errores);
                return resultado;
            }
            resultado.Return.Notas = resultadoComentarios.Return;

            //Requerimientos 
            var resultadoRequerimientos = GetDetalleRequerimientosById(id);
            if (!resultadoRequerimientos.Ok)
            {
                resultado.Copy(resultadoRequerimientos.Errores);
                return resultado;
            }
            resultado.Return.Requerimientos = resultadoRequerimientos.Return;

            //Moviles 
            var resultadoMoviles = GetDetalleMovilesById(id);
            if (!resultadoMoviles.Ok)
            {
                resultado.Copy(resultadoMoviles.Errores);
                return resultado;
            }
            resultado.Return.Moviles = resultadoMoviles.Return;

            //Empleados 
            var resultadoEmpleados = GetDetalleEmpleadosById(id);
            if (!resultadoEmpleados.Ok)
            {
                resultado.Copy(resultadoEmpleados.Errores);
                return resultado;
            }
            resultado.Return.Empleados = resultadoEmpleados.Return;

            //Flotas 
            var resultadoFlotas = GetDetalleFlotasById(id);
            if (!resultadoFlotas.Ok)
            {
                resultado.Copy(resultadoFlotas.Errores);
                return resultado;
            }
            resultado.Return.Flotas = resultadoFlotas.Return;

            //Barrios 
            var resultadoBarrios = GetDetalleBarriosById(id);
            if (!resultadoBarrios.Ok)
            {
                resultado.Copy(resultadoBarrios.Errores);
                return resultado;
            }
            resultado.Return.Barrios = resultadoBarrios.Return;

            return resultado;

        }
        public Result<List<Resultado_OrdenTrabajoDetalle_EstadoHistorial>> GetDetalleHistorialEstadosById(int id)
        {
            return dao.GetDetalleHistorialEstadosById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Nota>> GetDetalleNotasById(int id)
        {
            return dao.GetDetalleNotasById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Requerimiento>> GetDetalleRequerimientosById(int id)
        {
            return dao.GetDetalleRequerimientosById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Movil>> GetDetalleMovilesById(int id)
        {
            return dao.GetDetalleMovilesById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Empleado>> GetDetalleEmpleadosById(int id)
        {
            return dao.GetDetalleEmpleadosById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Flota>> GetDetalleFlotasById(int id)
        {
            return dao.GetDetalleFlotasById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Barrio>> GetDetalleBarriosById(int id)
        {
            return dao.GetDetalleBarriosById(id);
        }
        public Result<List<Resultado_OrdenTrabajoDetalle_Zona>> GetDetalleZonasById(int id)
        {
            return dao.GetDetalleZonasById(id);
        }
        public Result<Resultado_OrdenTrabajoPanelMasInfo> GetResultadoPanelMasInfoById(int idOt)
        {
            var resultado = new Result<Resultado_OrdenTrabajoPanelMasInfo>();
            var resultadoZonas = GetDetalleZonasById(idOt);
            if (!resultadoZonas.Ok)
            {
                resultado.Copy(resultadoZonas.Errores);
                return resultado;
            }
            resultado.Return = new Resultado_OrdenTrabajoPanelMasInfo();
            resultado.Return.Zonas = resultadoZonas.Return;

            var resultadoMoviles = GetDetalleMovilesById(idOt);
            if (!resultadoMoviles.Ok)
            {
                resultado.Copy(resultadoMoviles.Errores);
                return resultado;
            }
            resultado.Return.Moviles = resultadoMoviles.Return;

            var resultadoCantidadRQ = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado()).GetCantidadRequerimientosByIdOT(idOt);
            if (!resultadoMoviles.Ok)
            {
                resultado.Copy(resultadoMoviles.Errores);
                return resultado;
            }
            resultado.Return.CantidadRequerimientos = resultadoCantidadRQ.Return;
            return resultado;
        }

        //NUEVOS METODOS
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTabla(Consulta_OrdenTrabajo consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();
            consulta.IdAmbito = getUsuarioLogueado().Ambito.Id;
            var resultadoIds = dao.GetIds(consulta);

            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetDatosTablaByIds(resultadoIds.Return);
        }


        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaMisTrabajos(Consulta_OrdenTrabajo consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();

            //consulto con mi id de usuario, en que áreas soy empleado
            var resultEmpleados = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetIdsByFilters(new Consulta_Empleado() { IdUsuario = getUsuarioLogueado().Usuario.Id, DadosDeBaja = false });
            if (!resultEmpleados.Ok)
            {
                resultado.Copy(resultEmpleados.Errores);
                return resultado;
            }

            consulta.IdsEmpleado = resultEmpleados.Return;
            return GetDatosTabla(consulta);
        }

        public Result<int> GetCantidadMisTrabajos()
        {
            var resultado = new Result<int>();
            var consulta = new Consulta_OrdenTrabajo();

            consulta.IdsArea = getUsuarioLogueado().IdsAreas;
            consulta.IdAmbito = getUsuarioLogueado().Ambito.Id;
             consulta.EstadosKeyValue = new EstadoOrdenTrabajoRules(getUsuarioLogueado()).GetEstadosPorDefectoMisTrabajos_KeyValue();

            //consulto con mi id de usuario, en que áreas soy empleado
            var resultEmpleados = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetIdsByFilters(new Consulta_Empleado() { IdUsuario = getUsuarioLogueado().Usuario.Id, DadosDeBaja = false });
            if (!resultEmpleados.Ok)
            {
                resultado.Copy(resultEmpleados.Errores);
                return resultado;
            }

            if (resultEmpleados.Return.Count == 0)
            {
                return resultado;
            }

            consulta.IdsEmpleado = resultEmpleados.Return;
            return dao.GetCantidad(consulta);
        }

        public Result<bool> EditarDescripcion(Comando_OrdenTrabajo_Descripcion comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.EditarDescripcion);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Lo busco
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
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
        public Result<bool> AgregarRequerimientos(Comando_OrdenTrabajo_Requerimientos comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.AgregarRequerimiento);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdateOt = base.Update(ot);
                if (!resultUpdateOt.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
                var permisoRequerimientoRules = new PermisoEstadoRequerimientoRules(getUsuarioLogueado());

                foreach (var idRequerimiento in comando.IdsRequerimientos)
                {

                    var contiene = ot.ContieneRequerimiento(idRequerimiento);
                    if (contiene)
                    {
                        resultado.AddErrorPublico("El requerimiento ya se encuentra en la orden de trabajo.");
                        return false;
                    }


                    //Busco el requerimiento
                    var consultaRQ = requerimientoRules.GetByIdObligatorio(idRequerimiento);
                    if (!consultaRQ.Ok)
                    {
                        resultado.Copy(consultaRQ.Errores);
                        return false;
                    }

                    //valido que la ot y el rq sean del mismo area 
                    var rq = consultaRQ.Return;
                    if (rq.AreaResponsable.Id != ot.Area.Id)
                    {
                        resultado.AddErrorPublico("El requerimiento no es del mismo área que la orden de trabajo.");
                        return false;
                    }

                    //Estados Requerimiento Para Ot
                    var consultaEstadosRequerimientoParaOT = permisoRequerimientoRules.GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento.AgregarEnOrdenDeTrabajo);
                    if (!consultaEstadosRequerimientoParaOT.Ok)
                    {
                        resultado.Copy(consultaEstadosRequerimientoParaOT.Errores);
                        return false;
                    }

                    var estadoValido = false;
                    foreach (var estado in consultaEstadosRequerimientoParaOT.Return)
                    {
                        if (rq.GetUltimoEstado().Estado.KeyValue == estado)
                        {
                            estadoValido = true;
                        }
                    }

                    if (!estadoValido)
                    {
                        resultado.AddErrorPublico("El requerimiento no puede agregarse a la orden de trabajo ya que se encuentra en estado " + rq.GetUltimoEstado().Estado.Nombre);
                        return false;
                    }

                    //Una vez insertado, le seteo al Requerimiento la orden de trabajo insertada (como su OT Actual).
                    //Debe ser si o si LUEGO de insertarse el objeto.
                    //Ademas le cambio el estado al Reclamo, indicando que fue agregado a la OT. 
                    //Tambien se llama luego de insertar, porque en la observacion del cambio de estado
                    //estoy agregando el numero de la OT.
                    var requerimientoXOrdenTrabajoRules = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado());
                    //Genero el objeto RequerimientoXOrdenTrabajo
                    var rqxot = new RequerimientoPorOrdenTrabajo();
                    rqxot.Requerimiento = rq;
                    rqxot.OrdenTrabajo = ot;

                    //Inserto el RequerimientoXOrdenTrabajo
                    var resultInsertRQ = requerimientoXOrdenTrabajoRules.Insert(rqxot);
                    if (!resultInsertRQ.Ok)
                    {
                        resultado.Copy(resultInsertRQ.Errores);
                        return false;
                    }

                    //Seteo la OT activa
                    rq.OrdenTrabajoActiva = ot;
                    ot.ContieneRequerimiento(idRequerimiento);

                    //Cambio el estado
                    var resultCambiarEstadoRQ = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.ENPROCESO, "En Orden de Trabajo N° " + ot.GetNumero(), false);
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
            var resultadoPermiso = ValidarPemiso((int)comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.QuitarRequerimiento);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio((int)comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            if (ot.Requerimientos().Count() == 1)
            {
                resultado.AddErrorPublico("No se puede eliminar el único requerimiento de la orden de trabajo.");
                return resultado;
            }

            var contiene = ot.ContieneRequerimiento(comando.IdRequerimiento);
            if (!contiene)
            {
                resultado.AddErrorPublico("El requerimiento no se encuentra en la orden de trabajo.");
                return resultado;
            }

            var resultadoTransaccion = dao.Transaction(() =>
             {
                 //Le hago un update a la ot para que se le ponga fecha de modificación 
                 var resultUpdateOt = base.Update(ot);
                 if (!resultUpdateOt.Ok)
                 {
                     resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
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

                 //Quito la OT activa
                 rq.OrdenTrabajoActiva = null;

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

                 var rqXOT = ot.GetRequerimientoPorOrdenTrabajo(rq.Id);
                 if (rqXOT == null)
                 {
                     resultado.AddErrorPublico("Error al actualizar la orden de trabajo.");
                     return false;
                 }

                 var resultDelete = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado()).Delete(rqXOT);
                 if (rqXOT == null)
                 {
                     resultado.AddErrorPublico("Error al eliminar el requerimiento de la orden de trabajo.");
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
        public Result<bool> EditarRecursos(Comando_OrdenTrabajo_Recursos comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.EditarRecursos);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdateOt = base.Update(ot);
                if (!resultUpdateOt.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                var resultRecurso = new RecursoPorOrdenTrabajoRules(getUsuarioLogueado()).GetByIdOrdenTrabajo(comando.IdOrdenTrabajo);
                if (!resultRecurso.Ok)
                {
                    resultado.Copy(resultRecurso.Errores);
                    return false;
                }

                var recursos = new RecursoPorOrdenTrabajo();
                var insert = true;
                if (resultRecurso.Return != null && resultRecurso.Return.Count != 0)
                {
                    recursos = resultRecurso.Return[0];
                    insert = false;
                }

                recursos.Material = comando.Material;
                recursos.Observaciones = comando.Observaciones;
                recursos.Personal = comando.Personal;
                recursos.Observaciones = comando.Observaciones;

                var resultUpdate = new Result<RecursoPorOrdenTrabajo>();
                if (insert)
                {
                    recursos.OrdenTrabajo = ot;
                    resultUpdate = new RecursoPorOrdenTrabajoRules(getUsuarioLogueado()).Insert(recursos);
                }
                else
                {
                    resultUpdate = new RecursoPorOrdenTrabajoRules(getUsuarioLogueado()).Update(recursos);
                }

                if (!resultUpdate.Ok)
                {
                    resultado.Copy(resultUpdate.Errores);
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
        public Result<bool> AgregarNota(Comando_OrdenTrabajo_Nota comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.AgregarNota);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            //Consulto el usuario
            var resultUser = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
            if (!resultUser.Ok)
            {
                resultado.Copy(resultUser.Errores);
                return resultado;
            }

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(ot);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                //si no tiene requerimiento, la nota es para la orden
                if (!comando.IdRequerimiento.HasValue)
                {
                    var nota = new NotaPorOrdenTrabajo();
                    nota.Observaciones = comando.Observaciones;
                    nota.Usuario = resultUser.Return;
                    nota.OrdenTrabajo = ot;

                    var resultInsert = new NotaPorOrdenTrabajoRules(getUsuarioLogueado()).Insert(nota);
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

                if (ot.Requerimientos().Where(x => x.Id == rq.Id).ToList().Count() == 0)
                {
                    resultado.AddErrorPublico("El requerimiento no se encuentra en la orden de trabajo");
                    return false;
                }

                //si tiene requerimiento, la nota es para el requerimiento desde la orden de trabajo
                var notaRq = new NotaPorRequerimiento();
                notaRq.Requerimiento = rq;
                notaRq.Observaciones = comando.Observaciones;
                notaRq.Usuario = resultUser.Return;
                notaRq.OrdenTrabajo = ot;

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
        public Result<bool> EditarMoviles(Comando_OrdenTrabajo_Moviles comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.EditarMoviles);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(ot);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                var movilesXOTRules = new MovilPorOrdenTrabajoRules(getUsuarioLogueado());
                var movilRules = new MovilRules(getUsuarioLogueado());

                var idsMovilesAgregar = comando.IdMoviles.Where(x => ot.MovilesPorOrdenTrabajoActivos().All(y => y.Movil.Id != x)).ToList();
                if (idsMovilesAgregar.Count > 0)
                {
                    foreach (int m in idsMovilesAgregar)
                    {
                        var resultConsulta = movilRules.GetByIdObligatorio(m);
                        if (!resultConsulta.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los móviles");
                            return false;
                        }

                        if (resultConsulta.Return.Area.Id != ot.Area.Id)
                        {
                            resultado.AddErrorPublico("Uno de los móviles no pertenece al área de la orden de trabajo");
                            return false;
                        }

                        var resultEntrar = movilRules.EntrarEnOrdenTrabajo(resultConsulta.Return, ot.Numero + '/' + ot.Año);
                        if (!resultEntrar.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los móviles");
                            return false;
                        }

                        var mxot = new MovilPorOrdenTrabajo();
                        mxot.Movil = resultConsulta.Return;
                        mxot.OrdenTrabajo = ot;

                        var resultInsert = movilesXOTRules.Insert(mxot);
                        if (!resultInsert.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los móviles");
                            return false;
                        }
                    }
                }

                var movilesQuitar = ot.MovilesPorOrdenTrabajoActivos().Where(x => comando.IdMoviles.All(y => y != x.Movil.Id)).ToList();
                if (movilesQuitar.Count > 0)
                {
                    foreach (MovilPorOrdenTrabajo m in movilesQuitar)
                    {
                        var resultSalir = movilRules.SalirDeOrdenTrabajo(m.Movil, Enums.EstadoOrdenTrabajo.ENPROCESO, ot);
                        if (!resultSalir.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar uno de los móviles");
                            return false;
                        }

                        var resultQuitar = movilesXOTRules.Delete(m);
                        if (!resultQuitar.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar uno de los móviles");
                            return false;
                        }
                    }
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            resultado.Return = true;
            return resultado;
        }
        public Result<bool> QuitarMovil(Comando_OrdenTrabajo_QuitarMovil comando)
        {
            var resultado = new Result<bool>();

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var movilesActivos = consulta.Return.MovilesPorOrdenTrabajoActivos();

            var movilesQuitar = movilesActivos.Where(x => comando.IdMovil != x.Movil.Id).Select(z => z.Movil.Id).ToList();
            if (movilesActivos.Count() - movilesQuitar.Count() != 1)
            {
                resultado.AddErrorPublico("Error al intentar eliminar el móvil");
                return resultado;
            }

            return EditarMoviles(new Comando_OrdenTrabajo_Moviles() { IdOrdenTrabajo = comando.IdOrdenTrabajo, IdMoviles = movilesQuitar });
        }
        public Result<bool> EditarEmpleados(Comando_OrdenTrabajo_Empleados comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.EditarEmpleados);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(ot);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                var empleadoXOTRules = new EmpleadoPorOrdenTrabajoRules(getUsuarioLogueado());
                var empleadoRules = new EmpleadoPorAreaRules(getUsuarioLogueado());

                var idsAgregar = comando.IdEmpleados.Where(x => ot.EmpleadosPorOrdenTrabajoActivos().All(y => y.Empleado.Id != x)).ToList();
                if (idsAgregar.Count > 0)
                {
                    foreach (int m in idsAgregar)
                    {
                        var resultConsulta = empleadoRules.GetByIdObligatorio(m);
                        if (!resultConsulta.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los empleados");
                            return false;
                        }

                        if (resultConsulta.Return.Area.Id != ot.Area.Id)
                        {
                            resultado.AddErrorPublico("Uno de los empleados no pertenece al área de la orden de trabajo");
                            return false;
                        }

                        var resultEntrar = empleadoRules.EntrarEnOrdenTrabajo(resultConsulta.Return, ot.Numero + '/' + ot.Año);
                        if (!resultEntrar.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los empleados");
                            return false;
                        }

                        var exot = new EmpleadoPorOrdenTrabajo();
                        exot.Empleado = resultConsulta.Return;
                        exot.OrdenTrabajo = ot;
                        exot.Seccion = resultConsulta.Return.Seccion;

                        var resultInsert = empleadoXOTRules.Insert(exot);
                        if (!resultInsert.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los empleados");
                            return false;
                        }
                    }
                }

                var empeadosQuitar = ot.EmpleadosPorOrdenTrabajoActivos().Where(x => comando.IdEmpleados.All(y => y != x.Empleado.Id)).ToList();
                if (empeadosQuitar.Count > 0)
                {
                    foreach (EmpleadoPorOrdenTrabajo m in empeadosQuitar)
                    {
                        var resultSalir = empleadoRules.SalirDeOrdenTrabajo(m.Empleado, Enums.EstadoOrdenTrabajo.ENPROCESO, ot);
                        if (!resultSalir.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar uno de los empleados");
                            return false;
                        }

                        var resultQuitar = empleadoXOTRules.Delete(m);
                        if (!resultQuitar.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar uno de los empleados");
                            return false;
                        }
                    }
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            resultado.Return = true;
            return resultado;
        }
        public Result<bool> QuitarEmpleado(Comando_OrdenTrabajo_QuitarEmpleado comando)
        {
            var resultado = new Result<bool>();

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var movilesActivos = consulta.Return.EmpleadosPorOrdenTrabajoActivos();

            var movilesQuitar = movilesActivos.Where(x => comando.IdEmpleado != x.Empleado.Id).Select(z => z.Empleado.Id).ToList();
            if (movilesActivos.Count() - movilesQuitar.Count() != 1)
            {
                resultado.AddErrorPublico("Error al intentar eliminar el empleado");
                return resultado;
            }

            return EditarEmpleados(new Comando_OrdenTrabajo_Empleados() { IdOrdenTrabajo = comando.IdOrdenTrabajo, IdEmpleados = movilesQuitar });
        }
        public Result<bool> EditarFlotas(Comando_OrdenTrabajo_Flotas comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.EditarFlotas);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;

            var resultadoTransaccion = dao.Transaction(() =>
            {
                //Le hago un update a la ot para que se le ponga fecha de modificación 
                var resultUpdate = base.Update(ot);
                if (!resultUpdate.Ok)
                {
                    resultado.AddErrorPublico("Error al actualizar la orden de trabajo");
                    return false;
                }

                var flotaXOTRules = new FlotaPorOrdenTrabajoRules(getUsuarioLogueado());
                var flotaRules = new FlotaRules(getUsuarioLogueado());

                var idsAgregar = comando.IdFlotas.Where(x => ot.FlotasPorOrdenTrabajoActivos().All(y => y.Flota.Id != x)).ToList();
                if (idsAgregar.Count > 0)
                {
                    foreach (int m in idsAgregar)
                    {
                        var resultConsulta = flotaRules.GetByIdObligatorio(m);
                        if (!resultConsulta.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de las flotas");
                            return false;
                        }

                        if (resultConsulta.Return.Area.Id != ot.Area.Id)
                        {
                            resultado.AddErrorPublico("Uno de las flotas no pertenece al área de la orden de trabajo");
                            return false;
                        }

                        var resultEntrar = flotaRules.EntrarEnOrdenTrabajo(resultConsulta.Return, ot.Numero + '/' + ot.Año);
                        if (!resultEntrar.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar uno de los moviles");
                            return false;
                        }

                        var fxot = new FlotaPorOrdenTrabajo();
                        fxot.Flota = resultConsulta.Return;
                        fxot.OrdenTrabajo = ot;

                        var resultInsert = flotaXOTRules.Insert(fxot);
                        if (!resultInsert.Ok)
                        {
                            resultado.AddErrorPublico("Error al agregar una de los flotas");
                            return false;
                        }
                    }
                }

                var flotasQuitar = ot.FlotasPorOrdenTrabajoActivos().Where(x => comando.IdFlotas.All(y => y != x.Flota.Id)).ToList();
                if (flotasQuitar.Count > 0)
                {
                    foreach (FlotaPorOrdenTrabajo m in flotasQuitar)
                    {
                        var resultSalir = flotaRules.SalirDeOrdenTrabajo(m.Flota, Enums.EstadoOrdenTrabajo.ENPROCESO, ot);
                        if (!resultSalir.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar una de los flotas");
                            return false;
                        }

                        var resultQuitar = flotaXOTRules.Delete(m);
                        if (!resultQuitar.Ok)
                        {
                            resultado.AddErrorPublico("Error al quitar uno de los moviles");
                            return false;
                        }
                    }
                }

                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud.");
            }

            resultado.Return = true;
            return resultado;
        }
        public Result<bool> QuitarFlota(Comando_OrdenTrabajo_QuitarFlota comando)
        {
            var resultado = new Result<bool>();

            //Busco la orden de trabajo
            var consulta = GetByIdObligatorio(comando.IdOrdenTrabajo);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var flotasActivas = consulta.Return.FlotasPorOrdenTrabajoActivos();

            var flotasQuitar = flotasActivas.Where(x => comando.IdFlota != x.Flota.Id).Select(z => z.Flota.Id).ToList();
            if (flotasActivas.Count() - flotasQuitar.Count() != 1)
            {
                resultado.AddErrorPublico("Error al intentar eliminar la flota");
                return resultado;
            }

            return EditarFlotas(new Comando_OrdenTrabajo_Flotas() { IdOrdenTrabajo = comando.IdOrdenTrabajo, IdFlotas = flotasQuitar });
        }
        public Result<bool> Completar(Comando_OrdenTrabajo_Cerrar comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.Id, Enums.PermisoEstadoOrdenTrabajo.Cerrar);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
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

                 //Valido si hay algun rq que no está asociado a la ot
                 var rqsNoAsociados = comando.Requerimientos.Where(x => ot.RequerimientosPorOrdenTrabajoActivos().All(y => y.Requerimiento.Id != x.IdRequerimiento)).ToList();
                 if (rqsNoAsociados.Count != 0)
                 {
                     resultado.AddErrorPublico("Hay algunos requerimientos que no están asociados a la Orden de Trabajo.");
                     return false;
                 }

                 //Rq que quedan 
                 var rqs = ot.RequerimientosPorOrdenTrabajoActivos().Where(x => comando.Requerimientos.Any(y => y.IdRequerimiento == x.Requerimiento.Id)).ToList();

                 //Paso cada RQ al estado que el usuario selecciono
                 foreach (RequerimientoPorOrdenTrabajo rxot in rqs)
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
                         var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, enumEstado, "Por haberse completado la Orden de Trabajo N° " + ot.Numero, false);
                         if (!resultCambioEstado.Ok)
                         {
                             resultado.Copy(resultCambioEstado.Errores);
                             return false;
                         }
                     }

                     //Quito la OT activa
                     rq.OrdenTrabajoActiva = null;

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
                 var rqsQuitar = ot.RequerimientosPorOrdenTrabajoActivos().Where(x => comando.Requerimientos.All(y => y.IdRequerimiento != x.Requerimiento.Id)).ToList();

                 //Quito los RQ
                 foreach (RequerimientoPorOrdenTrabajo rxot in rqsQuitar)
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
                         var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, enumEstado, "Estado cambiado por haberse completado de Orden de Trabajo", false);
                         if (!resultCambioEstado.Ok)
                         {
                             resultado.Copy(resultCambioEstado.Errores);
                             return false;
                         }
                     }

                     var requerimientoXOrdenTrabajoRules = new RequerimientoPorOrdenTrabajoRules(getUsuarioLogueado());
                     //Quito la fila de RequerimientoPorOrdenTrabajo
                     var rqsActivosPorOt = ot.RequerimientosPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList();
                     foreach (var rqActivo in rqsActivosPorOt)
                     {
                         if (rqActivo.Requerimiento.Id == rq.Id)
                         {
                             var resultDelete = requerimientoXOrdenTrabajoRules.Delete(rqActivo);
                             if (!resultDelete.Ok)
                             {
                                 resultado.Copy(resultDelete.Errores);
                                 return false;
                             }
                         }
                     }

                     //Quito la OT activa
                     rq.OrdenTrabajoActiva = null;

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

                 //cambio el estado de los empleados
                 var empleadoRules = new EmpleadoPorAreaRules(getUsuarioLogueado());
                 foreach (EmpleadoPorArea e in ot.EmpleadosPorOrdenTrabajo.Select(x => x.Empleado))
                 {
                     var resultCambiarEstadoEmpleados = empleadoRules.SalirDeOrdenTrabajo(e, Enums.EstadoOrdenTrabajo.COMPLETADO, ot);
                     if (!resultCambiarEstadoEmpleados.Ok)
                     {
                         resultado.Copy(resultCambiarEstadoEmpleados.Errores);
                         return false;
                     }
                 }

                 //cambio el estado de los moviles
                 var movilRules = new MovilRules(getUsuarioLogueado());
                 foreach (Movil m in ot.MovilesPorOrdenTrabajo.Select(x => x.Movil))
                 {
                     var resultCambiarEstadoMoviles = movilRules.SalirDeOrdenTrabajo(m, Enums.EstadoOrdenTrabajo.COMPLETADO, ot);
                     if (!resultCambiarEstadoMoviles.Ok)
                     {
                         resultado.Copy(resultCambiarEstadoMoviles.Errores);
                         return false;
                     }
                 }

                 //cambio el estado de las flotas
                 var flotaRules = new FlotaRules(getUsuarioLogueado());
                 foreach (Flota f in ot.FlotasPorOrdenTrabajoActivos().Select(x => x.Flota))
                 {
                     var resultCambiarEstadoFlota = flotaRules.SalirDeOrdenTrabajo(f, Enums.EstadoOrdenTrabajo.COMPLETADO, ot);
                     if (!resultCambiarEstadoFlota.Ok)
                     {
                         resultado.Copy(resultCambiarEstadoFlota.Errores);
                         return false;
                     }
                 }


                 //Cambio el estado de la OT
                 var resultCambiarEstadoOT = CambiarEstado(ot, Enums.EstadoOrdenTrabajo.COMPLETADO, comando.Observaciones, false);
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
        public Result<bool> Cancelar(Comando_OrdenTrabajo_Cancelar comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.Id, Enums.PermisoEstadoOrdenTrabajo.Cancelar);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden de trabajo
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
                foreach (RequerimientoPorOrdenTrabajo rxot in ot.RequerimientosPorOrdenTrabajoActivos())
                {

                    var rq = rxot.Requerimiento;
                    //Solo si el RQ no esta ya en ese estado, lo cambio
                    if (rq.GetUltimoEstado().Estado.KeyValue != Enums.EstadoRequerimiento.PENDIENTE)
                    {
                        var resultCambioEstado = requerimientoRules.ProcesarCambioEstado(rq, Enums.EstadoRequerimiento.PENDIENTE, "Por cancelación de Orden de Trabajo N° " + ot.Numero, false);
                        if (!resultCambioEstado.Ok)
                        {
                            resultado.Copy(resultCambioEstado.Errores);
                            return false;
                        }
                    }

                    //Quito la OT activa
                    rq.OrdenTrabajoActiva = null;

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

                //cambio el estado de los empleados
                var empleadoRules = new EmpleadoPorAreaRules(getUsuarioLogueado());
                foreach (EmpleadoPorArea e in ot.EmpleadosPorOrdenTrabajo.Select(x => x.Empleado))
                {
                    var resultCambiarEstadoEmpleados = empleadoRules.SalirDeOrdenTrabajo(e, Enums.EstadoOrdenTrabajo.CANCELADO, ot);
                    if (!resultCambiarEstadoEmpleados.Ok)
                    {
                        resultado.Copy(resultCambiarEstadoEmpleados.Errores);
                        return false;
                    }
                }

                //cambio el estado de los moviles
                var movilRules = new MovilRules(getUsuarioLogueado());
                foreach (Movil m in ot.MovilesPorOrdenTrabajo.Select(x => x.Movil))
                {
                    var resultCambiarEstadoMoviles = movilRules.SalirDeOrdenTrabajo(m, Enums.EstadoOrdenTrabajo.CANCELADO, ot);
                    if (!resultCambiarEstadoMoviles.Ok)
                    {
                        resultado.Copy(resultCambiarEstadoMoviles.Errores);
                        return false;
                    }
                }


                //cambio el estado de las flotas
                var flotaRules = new FlotaRules(getUsuarioLogueado());
                foreach (Flota f in ot.FlotasPorOrdenTrabajoActivos().Select(x => x.Flota))
                {
                    var resultCambiarEstadoFlota = flotaRules.SalirDeOrdenTrabajo(f, Enums.EstadoOrdenTrabajo.COMPLETADO, ot);
                    if (!resultCambiarEstadoFlota.Ok)
                    {
                        resultado.Copy(resultCambiarEstadoFlota.Errores);
                        return false;
                    }
                }

                //Cambio el estado de la OT
                var resultCambiarEstadoOT = CambiarEstado(ot, Enums.EstadoOrdenTrabajo.CANCELADO, comando.Motivo, false);
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
        public Result<bool> CambiarSeccion(Comando_OrdenTrabajo_Seccion comando)
        {
            var resultado = new Result<bool>();

            //Validar permiso
            var resultadoPermiso = ValidarPemiso(comando.IdOrdenTrabajo, Enums.PermisoEstadoOrdenTrabajo.CambiarSeccion);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }

            //Busco la orden
            var resultadoOrden = GetById(comando.IdOrdenTrabajo);
            if (!resultadoOrden.Ok)
            {
                resultado.Copy(resultadoOrden.Errores);
                return resultado;
            }
            var ot = resultadoOrden.Return;

            //Busco la seccion
            var resultadoSeccion = new SeccionRules(getUsuarioLogueado()).GetById(comando.IdSeccion);
            if (!resultadoSeccion.Ok)
            {
                resultado.Copy(resultadoSeccion.Errores);
                return resultado;
            }
            ot.Seccion = resultadoSeccion.Return;

            //Actualizo la orden
            var resultadoUpdate = base.Update(ot);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            return resultado;
        }

        /* Permiso */
        public Result<bool> ValidarPemiso(int id, Enums.PermisoEstadoOrdenTrabajo permiso)
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
                resultado.AddErrorPublico("La orden de trabajo no existe");
                return resultado;
            }
            if (ot.FechaBaja != null)
            {
                resultado.AddErrorPublico("La orden de trabajo se encuentra dado de baja");
                return resultado;
            }

            //Valido el permiso
            var keyValueEstado = ot.GetUltimoEstado().Estado.KeyValue;
            var resultadoPermiso = new PermisoEstadoOrdenTrabajoRules(getUsuarioLogueado()).TienePermiso(keyValueEstado, permiso);
            if (!resultadoPermiso.Ok)
            {
                resultado.Copy(resultadoPermiso.Errores);
                return resultado;
            }
            if (!resultadoPermiso.Return)
            {
                resultado.AddErrorPublico("La orden de trabajo no se encuentra en un estado válido para realizar esta accion");
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }
    }
}
