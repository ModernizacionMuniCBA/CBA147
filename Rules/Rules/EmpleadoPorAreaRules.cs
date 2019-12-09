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
using Model.Consultas;
using System.Collections.Generic;
using Model.Comandos;

namespace Rules.Rules
{
    public class EmpleadoPorAreaRules : BaseRules<EmpleadoPorArea>
    {

        private readonly EmpleadoDAO dao;

        public EmpleadoPorAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EmpleadoDAO.Instance;
        }

        public Result<Resultado_EmpleadoPorArea> Insert(Comando_Empleado comando)
        {
            var result = new Result<Resultado_EmpleadoPorArea>();

            var resultUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(comando.IdUsuario);
            if (!resultUsuario.Ok)
            {
                result.Copy(resultUsuario.Errores);
                return result;
            }

            var resultValidate = ValidateInsert(comando, resultUsuario.Return);
            if (!resultValidate.Ok)
            {
                result.Copy(resultValidate.Errores);
                return result;
            }

            dao.Transaction(() =>
            {
                try
                {
                    var empleado = new EmpleadoPorArea();


                    var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
                    if (!resultArea.Ok)
                    {
                        return false;
                    }

                    if (comando.IdSeccion.HasValue)
                    {
                        var resultSeccion = new SeccionRules(getUsuarioLogueado()).GetById((int)comando.IdSeccion);
                        if (!resultSeccion.Ok)
                        {
                            return false;
                        }

                        if (resultSeccion.Return.Area != resultArea.Return)
                        {
                            result.AddErrorPublico("La sección no pertenece al área seleccionada");
                            return false;
                        }

                        empleado.Seccion = resultSeccion.Return;
                    }

                    empleado.Area = resultArea.Return;
                    empleado.UsuarioEmpleado = resultUsuario.Return;

                    var resultadoCambioEstado = ProcesarCambioEstado(empleado, Enums.EstadoEmpleado.DISPONIBLE, "Creación del empleado", false);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultInsert = base.Insert(empleado);
                    if (!resultInsert.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción del empleado.");
                        return false;
                    }

                    var funciones = new List<FuncionPorEmpleado>();
                    if (comando.IdFunciones.Count != 0)
                    {
                        foreach (int id in comando.IdFunciones)
                        {
                            var resultFuncion = new FuncionPorAreaRules(getUsuarioLogueado()).GetById(id);
                            if (!resultFuncion.Ok)
                            {
                                return false;
                            }

                            var funcionPorEmpleado = new FuncionPorEmpleado();
                            funcionPorEmpleado.Funcion = resultFuncion.Return;
                            funcionPorEmpleado.Empleado = resultInsert.Return;

                            var resultInsertFuncion = new FuncionPorEmpleadoRules(getUsuarioLogueado()).Insert(funcionPorEmpleado);
                            if (!resultInsertFuncion.Ok)
                            {
                                result.AddErrorPublico("Error en la inserción del empleado.");
                                return false;
                            }

                            funciones.Add(resultInsertFuncion.Return);
                        }
                    }

                    resultInsert.Return.FuncionesPorEmpleado = funciones;
                    result.Return = new Resultado_EmpleadoPorArea(resultInsert.Return);
                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.Message);
                    result.AddErrorPublico("Error en la inserción del empleado.");
                    return false;
                }


            });

            return result;
        }

        public Result<bool> ValidateInsert(Comando_Empleado comando, _VecinoVirtualUsuario usuario)
        {
            var result = new Result<bool>();

            //veo si el area es mia
            var area = getUsuarioLogueado().Areas.Where(x => x.Id == comando.IdArea);
            if (area == null)
            {
                result.AddErrorPublico("El usuario no tiene permiso para registrar empleados para el área seleccionada.");
                return result;
            }

            //veo si el usuario es empleado 
            var esEmpleado = usuario.Empleado;
            if (!esEmpleado)
            {
                result.AddErrorPublico("El usuario no es empleado.");
                return result;
            }

            //veo si ya está configurado como empleado en la mia
            var resultConfigurado = GetIdsByFilters(new Consulta_Empleado() { IdUsuario = comando.IdUsuario, DadosDeBaja = false , IdArea=comando.IdArea});
            if (!resultConfigurado.Ok)
            {
                result.Copy(resultConfigurado.Errores);
                return result;
            }

            //BUSCAR EL DETALLE Y PONER EN QUE AREA SE ENCUENTRA
            if (resultConfigurado.Return.Count != 0)
            {
                result.AddErrorPublico("El usuario ya se encuentra perfilado en el área.");
                return result;
            }

            return result;
        }

        //Ids
        public Result<List<int>> GetIdsByFilters(Consulta_Empleado consulta)
        {
            return dao.GetIdsByFilters(consulta);
        }

        //ResultadoTabla
        public Result<ResultadoTabla_Empleado> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_Empleado>();
            var resultadoConsulta = GetResultadoTablaByIds(new List<int>() { id });
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null && resultadoConsulta.Return.Data.Count != 0)
            {
                resultado.Return = resultadoConsulta.Return.Data[0];
            }

            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByIds(List<int> ids)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Empleado>>();
            var resultTabla = dao.GetResultadoTablaByIds(limite, ids);
            if (!resultTabla.Ok)
            {
                resultado.Copy(resultTabla.Errores);
                return resultado;
            }

            var resultadoFunciones = new FuncionPorEmpleadoRules(getUsuarioLogueado()).GetByIdsEmpleados(ids);
            if (!resultadoFunciones.Ok)
            {
                resultado.Copy(resultadoFunciones.Errores);
                return resultado;
            }

            //seteo las funciones a los empleados
            foreach (ResultadoTabla_Empleado empleado in resultTabla.Return.Data)
            {
                empleado.IdsFunciones = resultadoFunciones.Return.Where(x => x.IdEmpleado == empleado.Id).Select(x => x.IdFuncion).ToList();
            }

            resultado.Return = resultTabla.Return;
            return resultado;
        }

        private int limite = 5000;
        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByFilters(Consulta_Empleado consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Empleado>>();
            var resultadoIds = GetIdsByFilters(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaByIds(resultadoIds.Return);
        }

        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByIdOrdenTrabajo(int idOrden)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Empleado>>();
            var consulta = new Consulta_Empleado();
            consulta.IdOT = idOrden;
            var resultadoIds = GetIdsByFilters(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaByIds(resultadoIds.Return);
        }

        //Resultado
        public Result<Resultado_EmpleadoPorArea> GetResultadoById(int id)
        {
            var resultado = new Result<Resultado_EmpleadoPorArea>();
            var resultDetalle = GetDetalleById(id);
            if (!resultDetalle.Ok)
            {
                resultado.Copy(resultDetalle.Errores);
                return resultado;
            }

            resultado.Return = resultDetalle.Return;

            //Historial Estados
            var resultadoEstados = GetDetalleHistorialEstadosById(id);
            if (!resultadoEstados.Ok)
            {
                resultado.Copy(resultadoEstados.Errores);
                return resultado;
            }
            resultado.Return.Estados = resultadoEstados.Return;

            //Historial Ordenes
            var resultadoOrdenes = GetDetalleHistorialEstadosById(id);
            if (!resultadoOrdenes.Ok)
            {
                resultado.Copy(resultadoOrdenes.Errores);
                return resultado;
            }
            resultado.Return.Estados = resultadoOrdenes.Return;

            //Funciones
            var resultadoFunciones = GetDetalleFuncionesById(id);
            if (!resultadoFunciones.Ok)
            {
                resultado.Copy(resultadoFunciones.Errores);
                return resultado;
            }
            resultado.Return.Funciones = resultadoFunciones.Return;

            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>> GetResultadoTablaPanelByFilters(Consulta_Empleado consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>>();
            var resultadoIds = GetIdsByFilters(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return GetResultadoTablaPanelByIds(resultadoIds.Return);
        }

        public Result<ResultadoTabla_EmpleadoPanel> GetResultadoTablaPanelById(int id)
        {
            var resultado = new Result<ResultadoTabla_EmpleadoPanel>();
            var lista = new List<int>();
            lista.Add(id);
            var resultadoLista = GetResultadoTablaPanelByIds(lista);
            if (!resultadoLista.Ok)
            {
                resultado.Copy(resultadoLista.Errores);
                return resultado;
            }

            if (resultadoLista.Return.Cantidad == 0)
            {
                resultado.AddErrorPublico("El empleado no existe");
                return resultado;
            }

            resultado.Return = resultadoLista.Return.Data.FirstOrDefault();
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>> GetResultadoTablaPanelByIds(List<int> ids)
        {
            var resultado = dao.GetResultadoTablaPanelByIds(limite, ids);
            if (!resultado.Ok)
            {
                resultado.Copy(resultado.Errores);
                return resultado;
            }

            //var resultOrdenes = dao.GetResultadoTablaPanelByIds(limite, ids);
            //if (!resultUsuario.Ok)
            //{
            //    resultado.Copy(resultUsuario.Errores);
            //    return resultado;
            //}
            return resultado;
        }

        public Result<Resultado_EmpleadoPorArea> GetResultadoByIdObligatorio(int id)
        {
            var resultado = new Result<Resultado_EmpleadoPorArea>();
            var resultUsuario = dao.GetByIdObligatorio(id);
            if (!resultUsuario.Ok)
            {
                resultado.Copy(resultUsuario.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_EmpleadoPorArea(resultUsuario.Return);
            return resultado;
        }

        public Result<Resultado_EmpleadoPorArea> GetDetalleById(int id)
        {
            return dao.GetDetalleById(id);
        }

        public Result<List<Resultado_EmpleadoPorArea.Resultado_Empleado_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            return dao.GetDetalleHistorialEstadosById(id);
        }

        public Result<List<Resultado_EmpleadoPorArea.Resultado_FuncionPorEmpleado>> GetDetalleFuncionesById(int id)
        {
            return dao.GetDetalleFuncionesById(id);
        }

        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetParaAgregarAOT(int idArea, int? idOT)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_Empleado>>();

            var resultEstados = new EstadoEmpleadoRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Empleado()
            {
                Estados = resultEstados.Return,
                IdArea = idArea,
                DadosDeBaja = false
            };

            var resultConsulta = dao.GetIdsByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var ids = new List<int>();
            ids = resultConsulta.Return;

            //agrego los moviles que están desde antes en la OT, sin importar su estado
            consulta = new Consulta_Empleado()
            {
                IdOT = idOT,
                IdArea = idArea,
                DadosDeBaja = false
            };

            resultConsulta = dao.GetIdsByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            ids = ids.Union(resultConsulta.Return).ToList();

            var resultConsultaTabla = dao.GetResultadoTablaByIds(limite, ids);
            if (!resultConsultaTabla.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var resultadoFunciones = new FuncionPorEmpleadoRules(getUsuarioLogueado()).GetByIdsEmpleados(ids);
            if (!resultadoFunciones.Ok)
            {
                result.Copy(resultadoFunciones.Errores);
                return result;
            }

            //seteo las funciones a los empleados
            foreach (ResultadoTabla_Empleado empleado in resultConsultaTabla.Return.Data)
            {
                empleado.IdsFunciones = resultadoFunciones.Return.Where(x => x.IdEmpleado == empleado.Id).Select(x => x.IdFuncion).ToList();
            }

            result.Return = resultConsultaTabla.Return;
            return result;

        }

        public Result<int> GetCantidadParaAgregarAOT(int idArea)
        {
            var result = new Result<int>();

            var resultEstados = new EstadoEmpleadoRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Empleado()
            {
                Estados = resultEstados.Return,
                IdArea = idArea,
                DadosDeBaja = false
            };

            return GetCantidadByFilters(consulta);
        }

        public Result<int> GetCantidadByFilters(Model.Consultas.Consulta_Empleado consulta)
        {
            return dao.GetCantidadByFilters(consulta);
        }

        /*Acciones*/
        public Result<bool> EditarFunciones(Comando_Empleado_EditarFunciones comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdEmpleado);
                    if (!resultEmpleado.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultEmpleado.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar empleados para el área " + resultEmpleado.Return.Area.Nombre);
                        return false;
                    }

                    //Elimino las funciones anteriores
                    var funcionesAnteriores = resultEmpleado.Return.GetFunciones();
                    if (funcionesAnteriores != null)
                    {
                        foreach (FuncionPorEmpleado funcion in funcionesAnteriores)
                        {
                            var resultDelete = new FuncionPorEmpleadoRules(getUsuarioLogueado()).Delete(funcion);
                            if (!resultDelete.Ok)
                            {
                                result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                                return false;
                            }
                        }
                    }

                    if (comando.IdFunciones.Count == 0)
                    {
                        return true;
                    }

                    //Inserto cada una de las funciones nuevas
                    foreach (int idFuncion in comando.IdFunciones)
                    {
                        var resultFuncion = new FuncionPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio(idFuncion);
                        if (!resultFuncion.Ok)
                        {
                            result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                            return false;
                        }

                        var funcionPorEmpleado = new FuncionPorEmpleado();
                        funcionPorEmpleado.Empleado = resultEmpleado.Return;
                        funcionPorEmpleado.Funcion = resultFuncion.Return;

                        var resultInsert = new FuncionPorEmpleadoRules(getUsuarioLogueado()).Insert(funcionPorEmpleado);
                        if (!resultInsert.Ok)
                        {
                            result.AddErrorPublico("Error en la actualización de las funciones del empleado.");
                            return false;
                        }
                    }

                    var resultUpdateEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).Update(resultEmpleado.Return);
                    if (!resultUpdateEmpleado.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización de las funciones del empleado.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización de las funciones del empleado.");
                    return false;
                }
            });

            result.Return = trans;
            return result;

        }

        public Result<bool> DarDeBaja(int idEmpleado)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio((int)idEmpleado);
                    if (!resultEmpleado.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultEmpleado.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar empleados para el área " + resultEmpleado.Return.Area.Nombre);
                        return false;
                    }

                    var resultDeleteEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).Delete(resultEmpleado.Return);
                    if (!resultDeleteEmpleado.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización de las funciones del empleado.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización de las funciones del empleado.");
                    return false;
                }
            });

            result.Return = trans;
            return result;

        }

        public Result<bool> CambiarEstado(Comando_Empleado_CambioEstado comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultEmpleado = GetByIdObligatorio((int)comando.IdEmpleado);
                    if (!resultEmpleado.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultadoCambioEstado = ProcesarCambioEstado(resultEmpleado.Return, (Enums.EstadoEmpleado)comando.EstadoKeyValue, comando.Observaciones, true);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultUpdateEmpleado = Update(resultEmpleado.Return);
                    if (!resultUpdateEmpleado.Ok)
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

        /*Orden de trabajo*/
        public Result<bool> EntrarEnOrdenTrabajo(EmpleadoPorArea empleado, string numeroOT)
        {
            var result = new Result<bool>();
            var list = new List<Enums.EstadoOrdenTrabajo>();
            list.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);

            var comando = new Comando_Empleado_CambioEstado();
            comando.EstadoKeyValue = Enums.EstadoEmpleado.OCUPADO;
            comando.IdEmpleado = empleado.Id;
            comando.Observaciones = "Por entrar en Orden de Trabajo N°" + numeroOT;

            result = CambiarEstado(comando);
            return result;
        }

        public Result<bool> SalirDeOrdenTrabajo(EmpleadoPorArea empleado, Enums.EstadoOrdenTrabajo estadoOT, OrdenTrabajo ot)
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

            var resultEstado = ProcesarCambioEstado(empleado, Enums.EstadoEmpleado.DISPONIBLE, mensaje + " la Orden de Trabajo N° " + ot.Numero + "/" + ot.Año);
            if (!resultEstado.Ok)
            {
                result.Copy(resultEstado.Errores);
                return result;
            }

            result.Return = true;
            return result;


        }

        /*Flota*/

        /*Flota*/
        public Result<bool> EntrarEnFlota(EmpleadoPorArea empleado, Flota flota)
        {
            var result = new Result<bool>();
            var comando = new Comando_Empleado_CambioEstado();
            comando.EstadoKeyValue = Enums.EstadoEmpleado.ENFLOTA;
            comando.IdEmpleado = empleado.Id;
            comando.Observaciones = "Por entrar en Flota " + flota.Nombre;

            result = CambiarEstado(comando);
            if (!result.Ok)
            {
                return result;
            }

            empleado.FlotaActiva = flota;
            var resultUpdate = Update(empleado);
            if (!resultUpdate.Ok)
            {
                result.Copy(resultUpdate.Errores);
                return result;
            }

            result.Return = true;
            return result;
        }

        public Result<bool> SalirDeFlota(EmpleadoPorArea empleado, Flota flota)
        {
            var result = new Result<bool>();
            var comando = new Comando_Empleado_CambioEstado();
            comando.EstadoKeyValue = Enums.EstadoEmpleado.DISPONIBLE;
            comando.IdEmpleado = empleado.Id;
            comando.Observaciones = "Por haberse terminado el turno de la flota " + flota.Nombre;

            result = CambiarEstado(comando);
            if (!result.Ok)
            {
                return result;
            }

            empleado.FlotaActiva = null;
            var resultUpdate = Update(empleado);
            if (!resultUpdate.Ok)
            {
                result.Copy(resultUpdate.Errores);
                return result;
            }

            result.Return = true;
            return result;

        }

        /* Estados */
        private Result<EstadoEmpleadoHistorial> CrearEstado(EmpleadoPorArea empleado, Enums.EstadoEmpleado e, string observaciones)
        {
            var result = new Result<EstadoEmpleadoHistorial>();
            var estadoEmpleadoPorAreaRules = new EstadoEmpleadoRules(getUsuarioLogueado());
            var resultEstado = estadoEmpleadoPorAreaRules.GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                return result;
            }

            var estado = resultEstado.Return;

            var estadoEmpleadoPorArea = new EstadoEmpleadoHistorial();
            estadoEmpleadoPorArea.Fecha = DateTime.Now;
            estadoEmpleadoPorArea.FechaAlta = DateTime.Now;
            estadoEmpleadoPorArea.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoEmpleadoPorArea.Estado = estado;
            estadoEmpleadoPorArea.Observaciones = observaciones;
            estadoEmpleadoPorArea.Empleado = empleado;

            result.Return = estadoEmpleadoPorArea;
            return result;
        }

        public Result<EmpleadoPorArea> ProcesarCambioEstado(EmpleadoPorArea empleado, Enums.EstadoEmpleado estado, string observaciones)
        {
            return ProcesarCambioEstado(empleado, estado, observaciones, true);
        }

        public Result<EmpleadoPorArea> ProcesarCambioEstado(EmpleadoPorArea empleado, Enums.EstadoEmpleado estado, string observaciones, bool guardarCambios)
        {
            var estadoEmpleadoPorArea = CrearEstado(empleado, estado, observaciones);

            var result = new Result<EmpleadoPorArea>();

            if (empleado.Estados == null)
            {
                empleado.Estados = new List<EstadoEmpleadoHistorial>();
            }

            if (empleado.Estados != null && empleado.Estados.Count != 0)
            {
                foreach (EstadoEmpleadoHistorial e in empleado.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estadoEmpleadoPorArea.Return.Ultimo = true;
            empleado.Estados.Add(estadoEmpleadoPorArea.Return);

            if (guardarCambios)
            {
                var resultUpdate = ValidateUpdate(empleado);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }

                resultUpdate = dao.Update(empleado);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }

            result.Return = empleado;
            return result;
        }

    }
}
