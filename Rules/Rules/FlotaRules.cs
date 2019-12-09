using System;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Model.Comandos;
using System.Collections.Generic;
using Model.Consultas;

namespace Rules.Rules
{
    public class FlotaRules : BaseRules<Flota>
    {

        private readonly FlotaDAO dao;

        public FlotaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = FlotaDAO.Instance;
        }


        /*ABM*/
        public Result<Resultado_Flota> Insertar(Comando_Flota comando)
        {
            var result = new Result<Resultado_Flota>();
            var resultValidar = Validar(comando);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            dao.Transaction(() =>
            {
                var flota = new Flota();
                var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById((int)comando.IdArea);
                flota.Observaciones = comando.Observaciones;
                flota.Area = resultArea.Return;

                //-----MOVIL-----
                var resultMovil = new MovilRules(getUsuarioLogueado()).GetById(comando.IdMovil);
                if (!resultMovil.Ok)
                {
                    result.Copy(resultMovil.Errores);
                    return false;
                }

                var movil = resultMovil.Return;

                //valido el movil
                var resultValidarMovil = ValidarMovil(movil, resultArea.Return);
                if (!resultValidarMovil.Return || !resultValidarMovil.Ok)
                {
                    result.Copy(resultValidarMovil.Errores);
                    return false;
                }

                //Genero el nombre para la flota
                var resultGenerarNombre = GenerarNombre(movil);
                if (!resultGenerarNombre.Ok)
                {
                    result.Copy(resultGenerarNombre.Errores);
                    return false;
                }
                flota.Nombre = resultGenerarNombre.Return;

                flota.Movil = movil;
                //inserto la flota
                var resultInsert = base.Insert(flota);
                if (!resultInsert.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return false;
                }


                //cambio de estado el movil
                var resultEstadoMovil = new MovilRules(getUsuarioLogueado()).EntrarEnFlota(movil, flota);
                if (!resultEstadoMovil.Return || !resultEstadoMovil.Ok)
                {
                    result.Copy(resultEstadoMovil.Errores);
                    return false;
                }

                //inserto el estado de la flota
                var resultadoCambioEstado = ProcesarCambioEstado(flota, Enums.EstadoFlota.DISPONIBLE, "Creación de la flota", false);
                if (!resultadoCambioEstado.Ok)
                {
                    result.Copy(resultadoCambioEstado.Errores);
                    return false;
                }

                //-----EMPLEADOS-----
                if (comando.IdsEmpleados != null)
                {
                    flota.Empleados = new List<EmpleadoPorFlota>();
                    foreach (int id in comando.IdsEmpleados)
                    {
                        //consulto el empleado
                        var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio(id);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        var empleado = resultEmpleado.Return;

                        //valido el empleado
                        var resultValidarEmpleado = ValidarEmpleado(empleado, resultArea.Return);
                        if (!resultValidarEmpleado.Return)
                        {
                            result.Copy(resultValidarEmpleado.Errores);
                            return false;
                        }

                        var empleadoXFlota = new EmpleadoPorFlota();
                        empleadoXFlota.Empleado = empleado;
                        empleadoXFlota.Flota = resultInsert.Return;

                        var resultUpdateEmpleado = new EmpleadoPorFlotaRules(getUsuarioLogueado()).Insert(empleadoXFlota);
                        if (!resultUpdateEmpleado.Ok)
                        {
                            result.Copy(resultUpdateEmpleado.Errores);
                            return false;
                        }

                        //cambio de estado el empleado
                        var resultEstadoEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).EntrarEnFlota(empleado, flota);
                        if (!resultEstadoEmpleado.Return)
                        {
                            result.Copy(resultEstadoEmpleado.Errores);
                            return false;
                        }

                        flota.Empleados.Add(resultUpdateEmpleado.Return);
                    }

                    //devuelvo el resultado de la flota creada
                    var r_Flota = new Resultado_Flota(resultInsert.Return);
                    result.Return = r_Flota;
                }

                return true;
            });

            return result;
        }
        public Result<int> Editar(Comando_Flota comando)
        {
            var result = new Result<int>();
            var resultValidar = Validar(comando);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            dao.Transaction(() =>
            {
                var resultFlota = GetById((int)comando.Id);
                if (!resultFlota.Ok)
                {
                    result.Copy(resultFlota.Errores);
                    return false;
                }

                var flota = resultFlota.Return;
                flota.Observaciones = comando.Observaciones;

                //-----MOVIL-----
                if (flota.Movil.Id != comando.IdMovil)
                {
                    var movilAnterior = flota.Movil;

                    //cambio de estado el movil anterior
                    var resultEstadoMovilAnterior = new MovilRules(getUsuarioLogueado()).SalirDeFlota(movilAnterior, flota);
                    if (!resultEstadoMovilAnterior.Ok)
                    {
                        result.Copy(resultEstadoMovilAnterior.Errores);
                        return false;
                    }

                    //consulto el nuevo movil
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetById(comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.Copy(resultMovil.Errores);
                        return false;
                    }

                    var movil = resultMovil.Return;

                    //valido el movil
                    var resultValidarMovil = ValidarMovil(movil, flota.Area);
                    if (!resultValidarMovil.Return)
                    {
                        result.Copy(resultValidarMovil.Errores);
                        return false;
                    }

                    //cambio de estado el movil
                    var resultEstadoMovil = new MovilRules(getUsuarioLogueado()).EntrarEnFlota(movil, flota);
                    if (!resultEstadoMovil.Return)
                    {
                        result.Copy(resultValidarMovil.Errores);
                        return false;
                    }

                    flota.Movil = movil;
                }

                var resultUpdate = Update(flota);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate.Errores);
                    return false;
                }

                flota = resultUpdate.Return;

                if (comando.IdsEmpleados != null)
                {
                    //elimino los empleados
                    var idsEmpleadosEliminar = flota.GetEmpleados().Select(x => x.Empleado.Id).ToList().Where(z => !comando.IdsEmpleados.Contains(z)).ToList();
                    foreach (int id in idsEmpleadosEliminar)
                    {
                        //consulto el empleado
                        var resultEmpleado = new EmpleadoPorFlotaRules(getUsuarioLogueado()).GetByIdEmpleado(id);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        //cambio de estado el empleado
                        var resultEstadoEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).SalirDeFlota(resultEmpleado.Return.Empleado, flota);
                        if (!resultEstadoEmpleado.Ok)
                        {
                            result.Copy(resultEstadoEmpleado.Errores);
                            return false;
                        }

                        var resultUpdateEmpleado = new EmpleadoPorFlotaRules(getUsuarioLogueado()).Delete(resultEmpleado.Return);
                        if (!resultUpdateEmpleado.Ok)
                        {
                            result.Copy(resultUpdateEmpleado.Errores);
                            return false;
                        }
                    }

                    //agrego los empleados
                    var idsEmpleadosAgregar = comando.IdsEmpleados.Where(z => !flota.GetEmpleados().Select(x => x.Empleado.Id).ToList().Contains(z)).ToList();

                    foreach (int id in idsEmpleadosAgregar)
                    {
                        //consulto el empleado
                        var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio(id);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        var empleado = resultEmpleado.Return;

                        //valido el empleado
                        var resultValidarEmpleado = ValidarEmpleado(empleado, flota.Area);
                        if (!resultValidarEmpleado.Return)
                        {
                            result.Copy(resultValidarEmpleado.Errores);
                            return false;
                        }

                        //cambio de estado el empleado
                        var resultEstadoEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).EntrarEnFlota(empleado, flota);
                        if (!resultEstadoEmpleado.Return)
                        {
                            result.Copy(resultEstadoEmpleado.Errores);
                            return false;
                        }

                        var empleadoXFlota = new EmpleadoPorFlota();
                        empleadoXFlota.Flota = flota;
                        empleadoXFlota.Empleado = empleado;

                        var resultUpdateEmpleado = new EmpleadoPorFlotaRules(getUsuarioLogueado()).Insert(empleadoXFlota);
                        if (!resultUpdateEmpleado.Ok)
                        {
                            result.Copy(resultUpdateEmpleado.Errores);
                            return false;
                        }
                    }

                    //devuelvo el id de la flota editada
                    result.Return = resultUpdate.Return.Id;
                }

                return true;
            });

            return result;
        }

        /*Getters*/
        public Result<List<Resultado_Flota>> GetByFilters(Consulta_Flota consulta)
        {
            var result = new Result<List<Resultado_Flota>>();
            var resultConsulta = dao.GetByFilters(consulta);
            if (!result.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_Flota.ToList(resultConsulta.Return);
            return result;
        }

        public Result<List<Resultado_Flota>> GetPanel(Consulta_Flota consulta)
        {
            var result = new Result<List<Resultado_Flota>>();
            var estados = new List<Enums.EstadoFlota>();
            estados.Add(Enums.EstadoFlota.DISPONIBLE);
            estados.Add(Enums.EstadoFlota.OCUPADO);

            consulta.Estados = estados;
            return GetByFilters(consulta);
        }
        public Result<Resultado_Flota> GetResultadoById(int id)
        {
            var result = new Result<Resultado_Flota>();
            var resultConsulta = GetById(id);
            if (!result.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = new Resultado_Flota(resultConsulta.Return);
            return result;
        }
        public Result<List<Resultado_Flota>> GetResultadoByIdOrdenTrabajo(int idOrden)
        {
            var resultado = new Result<List<Resultado_Flota>>();
            var consulta = new Consulta_Flota()
            {
                IdOT = idOrden
            };

            consulta.IdOT = idOrden;
            return GetByFilters(consulta);
        }
        public Result<int> GetCantidadByFilters(Model.Consultas.Consulta_Flota consulta)
        {
            return dao.GetCantidadByFilters(consulta);
        }
        public Result<int> GetCantidadParaAgregarAOT(int idArea)
        {
            var result = new Result<int>();

            var resultEstados = new EstadoFlotaRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Flota()
            {
                Estados = resultEstados.Return,
                IdArea = idArea,
                DadosDeBaja = false
            };

            return GetCantidadByFilters(consulta);
        }

        /*Validaciones*/
        public Result<Flota> Validar(Comando_Flota comando)
        {
            var result = new Result<Flota>();
            if (getUsuarioLogueado().Areas.Where(x => x.Id == comando.IdArea).Count() == 0)
            {
                result.AddErrorPublico("No tiene permisos para agregar flotas a esa área.");
                return result;
            }

            return result;
        }
        public Result<bool> ValidarMovil(Movil movil, CerrojoArea area)
        {
            var result = new Result<bool>();
            if (movil.Area != area)
            {
                result.AddErrorPublico("El área de la flota no es la misma que la del móvil seleccionado.");
                return result;
            }

            //consulto si el movil ya pertenece a una flota
            if (movil.FlotaActiva != null)
            {
                result.AddErrorPublico("El móvil ya pertenece a una flota.");
                return result;
            }

            //consulto si el movil está disponible para agregar
            if (!new EstadoMovilRules(getUsuarioLogueado()).GetEstadosParaOT().Return.Any(x => x == movil.UltimoEstado().KeyValue))
            {
                result.AddErrorPublico("El móvil no se encuentra en un estado válido para formar parte de una flota.");
                return result;
            }

            result.Return = true;
            return result;
        }
        public Result<bool> ValidarEmpleado(EmpleadoPorArea empleado, CerrojoArea area)
        {
            var result = new Result<bool>();
            //consulto si ya pertenece a una flota
            if (empleado.FlotaActiva != null)
            {
                result.AddErrorPublico("El empleado ya pertenece a una flota.");
                return result;
            }

            //consulto si el area del movil y la flota coinciden
            if (empleado.Area != area)
            {
                result.AddErrorPublico("Uno de los empleados no pertenece al área del móvil seleccionado");
                return result;
            }

            //consulto si el movil está en un estado disponible para agregar
            if (!new EstadoEmpleadoRules(getUsuarioLogueado()).GetEstadosParaOT().Return.Any(x => x == empleado.UltimoEstado().KeyValue))
            {
                result.AddErrorPublico("El empleado no se encuentra en un estado válido para formar parte de una flota.");
                return result;
            }

            result.Return = true;
            return result;
        }
        public Result<string> GenerarNombre(Movil m)
        {
            var result = new Result<string>();
            var resultConsulta = dao.GetByFilters(new Consulta_Flota()
            {
                Hoy = true,
                IdMovil = m.Id
            });
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = "F" + DateTime.Now.Day + "M" + m.NumeroInterno + "-" + (resultConsulta.Return.Count + 1);
            return result;
        }

        /*Orden de trabajo*/
        public Result<List<Resultado_Flota>> GetParaAgregarAOT(int idArea, int? idOT)
        {
            var result = new Result<List<Resultado_Flota>>();
            var resultEstados = new EstadoFlotaRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Flota()
            {
                Estados = resultEstados.Return,
                IdArea = idArea,
                DadosDeBaja = false
            };

            var resultConsulta = dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var flotas = resultConsulta.Return;

            if (consulta.IdOT.HasValue)
            {
                //agrego las flotas que están desde antes en la OT, sin importar su estado
                consulta = new Consulta_Flota()
                {
                    IdOT = idOT,
                    IdArea = idArea,
                    DadosDeBaja = false
                };

                resultConsulta = dao.GetByFilters(consulta);
                if (!resultConsulta.Ok)
                {
                    result.AddErrorPublico("Error al realizar la consulta");
                    return result;
                }

                flotas = flotas.Union(resultConsulta.Return).ToList();
            }

            result.Return = Resultado_Flota.ToList(flotas);
            return result;
        }
        public Result<bool> EntrarEnOrdenTrabajo(Flota flota, string numeroOT)
        {
            var result = new Result<bool>();
            var list = new List<Enums.EstadoOrdenTrabajo>();
            list.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);

            var comando = new Comando_Flota_CambioEstado();
            comando.EstadoKeyValue = Enums.EstadoFlota.OCUPADO;
            comando.IdFlota = flota.Id;
            comando.Observaciones = "Por entrar en Orden de Trabajo N°" + numeroOT;

            result = CambiarEstado(comando);
            return result;
        }

        public Result<bool> SalirDeOrdenTrabajo(Flota flota, Enums.EstadoOrdenTrabajo estadoOT, OrdenTrabajo ot)
        {

            var result = new Result<bool>();

            //si el estado de la OT es en proceso
            var mensaje = "Por salir de ";
            switch (estadoOT)
            {
                case Enums.EstadoOrdenTrabajo.COMPLETADO:
                    mensaje = "Por haberse completado ";
                    break;
                case Enums.EstadoOrdenTrabajo.CANCELADO:
                    mensaje = "Por haberse cancelado ";
                    break;
            }

            var resultEstado = ProcesarCambioEstado(flota, Enums.EstadoFlota.DISPONIBLE, mensaje + " la Orden de Trabajo N° " + ot.Numero + "/" + ot.Año);
            if (!resultEstado.Ok)
            {
                result.Copy(resultEstado.Errores);
                return result;
            }

            result.Return = true;
            return result;
        }

        /* Estados */
        private Result<EstadoFlotaHistorial> CrearEstado(Flota flota, Enums.EstadoFlota e, string observaciones)
        {
            var result = new Result<EstadoFlotaHistorial>();
            var estadoFlotaPorAreaRules = new EstadoFlotaRules(getUsuarioLogueado());
            var resultEstado = estadoFlotaPorAreaRules.GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                return result;
            }

            var estado = resultEstado.Return;

            var estadoFlotaPorArea = new EstadoFlotaHistorial();
            estadoFlotaPorArea.Fecha = DateTime.Now;
            estadoFlotaPorArea.FechaAlta = DateTime.Now;
            estadoFlotaPorArea.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoFlotaPorArea.Estado = estado;
            estadoFlotaPorArea.Observaciones = observaciones;
            estadoFlotaPorArea.Flota = flota;

            result.Return = estadoFlotaPorArea;
            return result;
        }
        public Result<bool> CambiarEstado(Comando_Flota_CambioEstado comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultFlota = GetByIdObligatorio((int)comando.IdFlota);
                    if (!resultFlota.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultadoCambioEstado = ProcesarCambioEstado(resultFlota.Return, (Enums.EstadoFlota)comando.EstadoKeyValue, comando.Observaciones, true);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultUpdateFlota = Update(resultFlota.Return);
                    if (!resultUpdateFlota.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del estado.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del estado.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }
        public Result<Flota> ProcesarCambioEstado(Flota flota, Enums.EstadoFlota estado, string observaciones)
        {
            return ProcesarCambioEstado(flota, estado, observaciones, true);
        }
        public Result<Flota> ProcesarCambioEstado(Flota flota, Enums.EstadoFlota estado, string observaciones, bool guardarCambios)
        {
            var estadoFlotaPorArea = CrearEstado(flota, estado, observaciones);

            var result = new Result<Flota>();

            if (flota.Estados == null)
            {
                flota.Estados = new List<EstadoFlotaHistorial>();
            }

            if (flota.Estados != null && flota.Estados.Count != 0)
            {
                foreach (EstadoFlotaHistorial e in flota.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estadoFlotaPorArea.Return.Ultimo = true;
            flota.Estados.Add(estadoFlotaPorArea.Return);

            if (guardarCambios)
            {
                var resultUpdate = ValidateUpdate(flota);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }

                resultUpdate = dao.Update(flota);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }

            result.Return = flota;
            return result;
        }

        /*Terminar turno*/
        public Result<bool> TerminarTurno(List<int> idsFlotas)
        {
            var result = new Result<bool>();
            result.Return = dao.Transaction(() =>
            {
                foreach (int idFlota in idsFlotas)
                {
                    var resultTurno = TerminarTurno(idFlota);
                    if (!result.Ok)
                    {
                        return false;
                    }
                }

                return true;
            });

            return result;
        }

        public Result<bool> TerminarTurno(int idFlota)
        {
            var result = new Result<bool>();

            var resultFlota = GetByIdObligatorio(idFlota);
            if (!resultFlota.Ok)
            {
                result.Copy(resultFlota.Errores);
                result.AddErrorPublico("Error al terminar el turno. Puede que no se haya podido cambiar el estado de alguna de las flotas del turno.");
                return result;
            }

            var flota = resultFlota.Return;
            if ((int)flota.GetUltimoEstado().KeyValue == new EstadoFlotaRules(getUsuarioLogueado()).GetKeyValueEstadoOcupado().Return)
            {
                result.AddErrorPublico("No puede terminar el turno si alguna de las flotas se encuentra realizando un trabajo");
                return result;
            }

            dao.Transaction(() =>
            {
                var resultCambiarEstado = CambiarEstado(new Comando_Flota_CambioEstado()
                {
                    IdFlota = flota.Id,
                    EstadoKeyValue = Enums.EstadoFlota.TURNOTERMINADO,
                    Observaciones = "Terminado el turno el " + DateTime.Now + ""
                });

                if (!resultCambiarEstado.Ok)
                {
                    result.Copy(resultFlota.Errores);
                    result.AddErrorPublico("Error al terminar el turno. Puede que no se haya podido cambiar el estado de alguna de las flotas del turno.");
                    return false;
                }

                //cambio de estado del movil 
                var resultEstadoMovil = new MovilRules(getUsuarioLogueado()).SalirDeFlota(flota.Movil, flota);
                if (!resultEstadoMovil.Return)
                {
                    result.Copy(resultEstadoMovil.Errores);
                    return false;
                }

                //le cambio el estado a los empleados
                foreach (EmpleadoPorFlota empleado in flota.Empleados)
                {
                    var resultEstadoEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).SalirDeFlota(empleado.Empleado, flota);
                    if (!resultEstadoEmpleado.Return)
                    {
                        result.Copy(resultEstadoEmpleado.Errores);
                        return false;
                    }
                }
                return true;
            });

            return result;
        }

        public Result<bool> CambiarEstado(Comando_CambioEstado comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultFlota = GetByIdObligatorio((int)comando.Id);
                    if (!resultFlota.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultadoCambioEstado = ProcesarCambioEstado(resultFlota.Return, (Enums.EstadoFlota)comando.EstadoKeyValue, comando.Observaciones, true);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultUpdateMovil = Update(resultFlota.Return);
                    if (!resultUpdateMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del estado.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del estado.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }
    }
}
