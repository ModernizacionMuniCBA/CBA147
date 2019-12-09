using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace Rules.Rules
{
    public class MovilRules : BaseRules<Movil>
    {

        private readonly MovilDAO dao;

        public MovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = MovilDAO.Instance;
        }

        public Result<Resultado_Movil> Insert(Comando_Movil comando)
        {
            var result = new Result<Resultado_Movil>();

            dao.Transaction(() =>
            {
                try
                {
                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == comando.IdArea);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para registrar móviles para el área seleccionada.");
                        return false;
                    }

                    var movil = new Movil(comando);

                    var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
                    if (!resultArea.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    movil.Area = resultArea.Return;

                    var resultTipo = new TipoMovilRules(getUsuarioLogueado()).GetById(comando.IdTipo);
                    if (!resultTipo.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }
                    movil.Tipo = resultTipo.Return;

                    var resultadoCambioEstado = ProcesarCambioEstado(movil, (Enums.EstadoMovil)comando.IdEstado, "Creación del móvil", false);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultInsert = base.Insert(movil);
                    if (!resultInsert.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción del móvil.");
                        return false;
                    }

                    if (comando.Valuacion.HasValue)
                    {
                        var valuacion = new ValuacionPorMovil();
                        valuacion.Valor = (int)comando.Valuacion;
                        valuacion.Movil = resultInsert.Return;
                        valuacion.FechaValuacion = comando.FechaValuacion == null ? DateTime.Now : (DateTime)comando.FechaValuacion;

                        var resultValuacion = new ValuacionPorMovilRules(getUsuarioLogueado()).Insert(valuacion);
                        if (!resultValuacion.Ok)
                        {
                            result.AddErrorPublico("Error en la inserción del móvil.");
                            result.AddErrorInterno("Error en la inserción de la valuacion.");
                            return false;
                        }
                    }

                    if (comando.Kilometraje.HasValue)
                    {
                        var kilometraje = new KilometrajePorMovil();
                        kilometraje.Kilometraje = (int)comando.Valuacion;
                        kilometraje.Movil = resultInsert.Return;
                        kilometraje.FechaKilometraje = comando.FechaKilometraje == null ? DateTime.Now : (DateTime)comando.FechaKilometraje;

                        var resultKilometraje = new KilometrajePorMovilRules(getUsuarioLogueado()).Insert(kilometraje);
                        if (!resultKilometraje.Ok)
                        {
                            result.AddErrorPublico("Error en la inserción del móvil.");
                            result.AddErrorInterno("Error en la inserción dEL kilometraje.");
                            return false;
                        }
                    }

                    if (comando.VencimientoITV.HasValue)
                    {
                        var vencimientoITV = new ITVPorMovil();
                        vencimientoITV.FechaVencimientoITV = (DateTime)comando.VencimientoITV;
                        vencimientoITV.Movil = resultInsert.Return;

                        var resultITV = new ITVPorMovilRules(getUsuarioLogueado()).Insert(vencimientoITV);
                        if (!resultITV.Ok)
                        {
                            result.AddErrorPublico("Error en la inserción del móvil.");
                            result.AddErrorInterno("Error en la inserción del itv.");
                            return false;
                        }
                    }

                    if (comando.VencimientoTUV.HasValue)
                    {
                        var vencimientoTUV = new TUVPorMovil();
                        vencimientoTUV.FechaVencimientoTUV = (DateTime)comando.VencimientoTUV;
                        vencimientoTUV.Movil = resultInsert.Return;

                        var resultTUV = new TUVPorMovilRules(getUsuarioLogueado()).Insert(vencimientoTUV);
                        if (!resultTUV.Ok)
                        {
                            result.AddErrorPublico("Error en la inserción del móvil.");
                            result.AddErrorInterno("Error en la inserción del itv.");
                            return false;
                        }
                    }

                    result.Return = new Resultado_Movil(resultInsert.Return);
                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorPublico(e.Message != null ? e.Message : "Error procesando la solicitud");
                    return false;
                }
            });

            return result;
        }

        public Result<bool> EditarInformacionBasica(Comando_Movil comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
              {
                  try
                  {

                      var area = getUsuarioLogueado().Areas.Where(x => x.Id == comando.IdArea);
                      if (area == null)
                      {
                          result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área seleccionada.");
                          return false;
                      }

                      var resultMovil = new MovilRules(getUsuarioLogueado()).GetById((int)comando.Id);
                      if (!resultMovil.Ok)
                      {
                          result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                          return false;
                      }

                      var movil = resultMovil.Return;

                      var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
                      if (!resultArea.Ok)
                      {
                          result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                          return false;
                      }

                      movil.Area = resultArea.Return;
                      movil.Asientos = comando.Asientos;
                      movil.Año = comando.Año;
                      movil.TipoCombustible = comando.IdTipoCombustible;
                      movil.Carga = comando.Carga;
                      movil.Dominio = comando.Dominio;
                      movil.FechaIncorporacion = comando.FechaIncorporacion;
                      movil.Marca = comando.Marca;
                      movil.Modelo = comando.Modelo;
                      movil.NumeroInterno = comando.NumeroInterno;

                      var resultTipo = new TipoMovilRules(getUsuarioLogueado()).GetById(comando.IdTipo);
                      if (!resultTipo.Ok)
                      {
                          result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                          return false;
                      }
                      movil.Tipo = resultTipo.Return;

                      var resultUpdate = new MovilRules(getUsuarioLogueado()).Update(movil);
                      if (!resultUpdate.Ok)
                      {
                          result.AddErrorPublico("Error en la edición de los datos básicos del móvil.");
                          return false;
                      }
                  }
                  catch (Exception e)
                  {
                      result.AddErrorInterno(e.ToString());
                      result.AddErrorPublico("Error en la edición de los datos básicos del móvil.");
                      return false;
                  }

                  return true;
              });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarValuacion(Comando_Movil_Valuacion comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var valuacionPorMovil = new ValuacionPorMovil();
                    valuacionPorMovil.Valor = comando.Valuacion;
                    if (comando.FechaValuacion.HasValue)
                        valuacionPorMovil.FechaValuacion = (DateTime)comando.FechaValuacion;
                    valuacionPorMovil.Observaciones = comando.Observaciones;
                    valuacionPorMovil.Movil = resultMovil.Return;

                    var resultUpdate = new ValuacionPorMovilRules(getUsuarioLogueado()).Insert(valuacionPorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización de la valuación del móvil.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización de la valuación del móvil.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización de la valuación del móvil.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarKilometraje(Comando_Movil_Kilometraje comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var kilometrajePorMovil = new KilometrajePorMovil();
                    kilometrajePorMovil.Kilometraje = comando.Kilometraje;
                    if (comando.FechaKilometraje.HasValue)
                        kilometrajePorMovil.FechaKilometraje = (DateTime)comando.FechaKilometraje;
                    kilometrajePorMovil.Observaciones = comando.Observaciones;
                    kilometrajePorMovil.Movil = resultMovil.Return;

                    var resultUpdate = new KilometrajePorMovilRules(getUsuarioLogueado()).Insert(kilometrajePorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del kilometraje del móvil.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del kilometraje del móvil.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del kilometraje del móvil.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarITV(Comando_Movil_ITV comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var itvPorMovil = new ITVPorMovil();
                    if (comando.FechaUltimoITV.HasValue)
                        itvPorMovil.FechaUltimoITV = comando.FechaUltimoITV;
                    itvPorMovil.FechaVencimientoITV = comando.FechaVencimientoITV;
                    itvPorMovil.Observaciones = comando.Observaciones;
                    itvPorMovil.Movil = resultMovil.Return;

                    var resultUpdate = new ITVPorMovilRules(getUsuarioLogueado()).Insert(itvPorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del ITV.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del ITV.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del ITV.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarTUV(Comando_Movil_TUV comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var tuvPorMovil = new TUVPorMovil();
                    if (comando.FechaUltimoTUV.HasValue)
                        tuvPorMovil.FechaUltimoTUV = comando.FechaUltimoTUV;
                    tuvPorMovil.FechaVencimientoTUV = comando.FechaVencimientoTUV;
                    tuvPorMovil.Observaciones = comando.Observaciones;
                    tuvPorMovil.Movil = resultMovil.Return;

                    var resultUpdate = new TUVPorMovilRules(getUsuarioLogueado()).Insert(tuvPorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del TUV.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la actualización del TUV.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del TUV.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarCaracteristicas(Comando_Movil_Caracteristicas comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var movil = resultMovil.Return;
                    movil.Caracteristicas = comando.Caracteristicas;

                    var resultUpdate = new MovilRules(getUsuarioLogueado()).Insert(movil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción del TUV.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización de las características del móvil.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> EditarCondicion(Comando_Movil_Condicion comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var movil = resultMovil.Return;
                    movil.Condicion = comando.Condicion;

                    var resultUpdate = new MovilRules(getUsuarioLogueado()).Update(movil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción del TUV.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la actualización del ITV.");
                    return false;
                }
            });


            result.Return = true;
            return result;
        }

        public Result<bool> CambiarEstado(Comando_CambioEstado comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = GetByIdObligatorio((int)comando.Id);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultadoCambioEstado = ProcesarCambioEstado(resultMovil.Return, (Enums.EstadoMovil)comando.EstadoKeyValue, comando.Observaciones, true);
                    if (!resultadoCambioEstado.Ok)
                    {
                        result.Copy(resultadoCambioEstado.Errores);
                        return false;
                    }

                    var resultUpdateMovil = Update(resultMovil.Return);
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

        public Result<bool> AgregarNota(Comando_Movil_Nota comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var resultUsuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
                    if (!resultUsuario.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var notaPorMovil = new NotaPorMovil();
                    notaPorMovil.Observaciones = comando.Contenido;
                    notaPorMovil.Movil = resultMovil.Return;
                    notaPorMovil.UsuarioCreador = resultUsuario.Return;

                    var resultUpdate = new NotaPorMovilRules(getUsuarioLogueado()).Insert(notaPorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la nota.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la nota.");
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

        public Result<bool> VisarNota(Comando_Movil_VisarNota comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var resultUsuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
                    if (!resultUsuario.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultNota = new NotaPorMovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdNota);
                    if (!resultNota.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var notaPorMovil = resultNota.Return;
                    var visto = !notaPorMovil.Visto;
                    notaPorMovil.Visto = visto;
                    notaPorMovil.UsuarioVisto = visto ? resultUsuario.Return : null;

                    var resultUpdate = new NotaPorMovilRules(getUsuarioLogueado()).Update(notaPorMovil);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la nota.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdate.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la nota.");
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

        public Result<bool> AgregarReparacion(Comando_Movil_Reparacion comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultMovil = new MovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.IdMovil);
                    if (!resultMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == resultMovil.Return.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + resultMovil.Return.Area.Nombre);
                        return false;
                    }

                    var resultUsuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
                    if (!resultUsuario.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var reparacionPorMovil = new ReparacionPorMovil();
                    reparacionPorMovil.MontoReparacion = comando.MontoReparacion;
                    reparacionPorMovil.FechaReparacion = comando.FechaReparacion;
                    reparacionPorMovil.Motivo = comando.Motivo;
                    reparacionPorMovil.Movil = resultMovil.Return;
                    reparacionPorMovil.Observaciones = comando.Observaciones;
                    reparacionPorMovil.Taller = comando.Taller;

                    var resultInsert = new ReparacionPorMovilRules(getUsuarioLogueado()).Insert(reparacionPorMovil);
                    if (!resultInsert.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la reparación.");
                        return false;
                    }

                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(resultMovil.Return);
                    if (!resultUpdateMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la inserción de la reparación.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en la inserción de la reparación.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<bool> BorrarReparacion(Comando_Movil_Reparacion comando)
        {
            var result = new Result<bool>();

            var trans = dao.Transaction(() =>
            {
                try
                {
                    var resultReparacionPorMovil = new ReparacionPorMovilRules(getUsuarioLogueado()).GetByIdObligatorio((int)comando.Id);
                    if (!resultReparacionPorMovil.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var reparacionPorMovil = resultReparacionPorMovil.Return;
                    var area = getUsuarioLogueado().Areas.Where(x => x.Id == reparacionPorMovil.Movil.Area.Id);
                    if (area == null)
                    {
                        result.AddErrorPublico("El usuario no tiene permiso para editar móviles para el área " + reparacionPorMovil.Movil.Area.Nombre);
                        return false;
                    }

                    var resultUsuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id);
                    if (!resultUsuario.Ok)
                    {
                        result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                        return false;
                    }

                    var resultDelete = new ReparacionPorMovilRules(getUsuarioLogueado()).Delete(reparacionPorMovil);
                    if (!resultDelete.Ok)
                    {
                        result.AddErrorPublico("Error en el borrado de la reparación.");
                        return false;
                    }

                    var movil = resultReparacionPorMovil.Return.Movil;
                    var resultUpdateMovil = new MovilRules(getUsuarioLogueado()).Update(movil);
                    if (!resultUpdateMovil.Ok)
                    {
                        result.AddErrorPublico("Error en el borrado de la reparación.");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.ToString());
                    result.AddErrorPublico("Error en el borrado de la reparación.");
                    return false;
                }
            });

            result.Return = trans;
            return result;
        }

        public Result<Resultado_Movil> Delete(Comando_Movil comando)
        {
            var result = new Result<Resultado_Movil>();

            var resultMovil = new MovilRules(getUsuarioLogueado()).GetById((int)comando.Id);
            if (!resultMovil.Ok)
            {
                result.AddErrorPublico("Error en la consulta de alguno de los datos.");
                return result;
            }

            //var resultEstado = new EstadoMovilRules(getUsuarioLogueado()).GetByKeyValue(Enums.EstadoMovil.DEBAJA);
            //if (!resultEstado.Ok)
            //{
            //    result.AddErrorPublico("Error en la consulta de alguno de los datos.");
            //    return result;
            //}

            //var movil = resultMovil.Return;
            //movil.Estado = resultEstado.Return;

            var otConsulta = new Consulta_OrdenTrabajo();
            otConsulta.IdMovil = comando.Id;

            var resultOrdenesConMovil = new OrdenTrabajoRules(getUsuarioLogueado()).GetIds(otConsulta);
            if (!resultOrdenesConMovil.Ok)
            {
                result.AddErrorPublico("Error al eliminar el móvil.");
                return result;
            }

            if (resultOrdenesConMovil.Return.Count > 0)
            {
                result.AddErrorPublico("No se puede eliminar el móvil porque se encuentra en una o más ordenes de trabajo.");
                return result;
            }

            var resultDelete = new MovilRules(getUsuarioLogueado()).Delete(resultMovil.Return);
            if (!resultDelete.Ok)
            {
                result.AddErrorPublico("Error al eliminar el móvil.");
                return result;
            }

            result.Return = new Resultado_Movil(resultDelete.Return);
            return result;
        }

        public Result<Resultado_Movil> GetResultadoById(int id)
        {
            var resultado = new Result<Resultado_Movil>();
            var resultMovil = base.GetById(id);
            if (!resultMovil.Ok || resultMovil.Return == null)
            {
                resultado.Copy(resultMovil.Errores);
                return resultado;
            }
            resultado.Return = new Resultado_Movil(resultMovil.Return);

            //Historial Estados
            var resultadoEstados = GetDetalleHistorialEstadosById(id);
            if (!resultadoEstados.Ok)
            {
                resultado.Copy(resultadoEstados.Errores);
                return resultado;
            }
            resultado.Return.Estados = resultadoEstados.Return;

            //Notas
            var resultadoNotas = GetDetalleNotasById(id);
            if (!resultadoNotas.Ok)
            {
                resultado.Copy(resultadoNotas.Errores);
                return resultado;
            }
            resultado.Return.Notas = resultadoNotas.Return;


            //Reparaciones
            var resultadoReparaciones = GetDetalleReparacionesById(id);
            if (!resultadoReparaciones.Ok)
            {
                resultado.Copy(resultadoReparaciones.Errores);
                return resultado;
            }
            resultado.Return.Reparaciones = resultadoReparaciones.Return;

            return resultado;
        }

        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            return dao.GetDetalleHistorialEstadosById(id);
        }

        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Nota>> GetDetalleNotasById(int id)
        {
            return dao.GetDetalleNotasById(id);
        }

        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Reparacion>> GetDetalleReparacionesById(int id)
        {
            return dao.GetDetalleReparacionesById(id);
        }

        public Result<List<Movil>> GetByFilters(Model.Consultas.Consulta_Movil consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<List<int>> GetIdsByFilters(Model.Consultas.Consulta_Movil consulta)
        {
            return dao.GetIdsByFilters(consulta);
        }

        public Result<int> GetCantidadByFilters(Model.Consultas.Consulta_Movil consulta)
        {
            return dao.GetCantidadByFilters(consulta);
        }


        public Result<List<Resultado_Movil>> GetResultadoByFilters(Model.Consultas.Consulta_Movil consulta)
        {
            var result = new Result<List<Resultado_Movil>>();

            var resultConsulta = dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            result.Return = Resultado_Movil.ToList(resultConsulta.Return);
            return result;
        }

        private int limite = 5000;
        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByFilters(Model.Consultas.Consulta_Movil consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Movil>>();
            var resultadoIds = GetIdsByFilters(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return dao.GetResultadoTablaByIds(limite, resultadoIds.Return);
        }

        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByIds(List<int> ids)
        {
            return dao.GetResultadoTablaByIds(limite, ids);
        }

        public Result<ResultadoTabla_Movil> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_Movil>();
            var resultadoConsulta = dao.GetResultadoTablaByIds(limite, new List<int>() { id });
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

        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByIdOrdenTrabajo(int idOT)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Movil>>();
            var resultadoConsulta = new MovilXOrdenTrabajoDAO().GetIdsByIdOrdenTrabajo(idOT, false);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            return dao.GetResultadoTablaByIds(limite, resultadoConsulta.Return);
        }

        public Result<List<Resultado_Movil>> GetParaAgregarAOT(int idArea, int? idOT)
        {
            var result = new Result<List<Resultado_Movil>>();

            var resultEstados = new EstadoMovilRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Movil()
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

            result.Return = Resultado_Movil.ToList(resultConsulta.Return);
            //agrego los moviles que están desde antes en la OT, sin importar su estado
            consulta = new Consulta_Movil()
            {
                IdOT=idOT
            };

            resultConsulta = dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            result.Return.Union(Resultado_Movil.ToList(resultConsulta.Return));
            return result;

        }


        class Compare : IEqualityComparer<Resultado_Movil>
        {
            public bool Equals(Resultado_Movil x, Resultado_Movil y)
            {
                return x.Id == y.Id;
            }
            public int GetHashCode(Resultado_Movil codeh)
            {
                return 0;
            }

        }

        public Result<int> GetCantidadParaAgregarAOT(int idArea)
        {
            var result = new Result<int>();

            var resultEstados = new EstadoMovilRules(getUsuarioLogueado()).GetEstadosParaOT();
            if (!resultEstados.Ok)
            {
                result.AddErrorPublico("Error al realizar la consulta");
                return result;
            }

            var consulta = new Consulta_Movil()
            {
                Estados = resultEstados.Return,
                IdArea = idArea,
                DadosDeBaja = false
            };

            return GetCantidadByFilters(consulta);
        }

        public Result<bool> HayMovilesConTipo(int idTipo)
        {
            return dao.HayMovilesConTipo(idTipo);
        }


        /* Estados */
        private Result<EstadoMovilHistorial> CrearEstado(Movil movil, Enums.EstadoMovil e, string observaciones)
        {
            var result = new Result<EstadoMovilHistorial>();
            var estadoMovilRules = new EstadoMovilRules(getUsuarioLogueado());
            var resultEstado = estadoMovilRules.GetByKeyValue(e);
            if (!resultEstado.Ok)
            {
                result.AddErrorInterno(resultEstado.Errores.ErroresInternos);
                result.AddErrorPublico(resultEstado.Errores.ErroresPublicos);
                return result;
            }

            var estado = resultEstado.Return;

            var estadoMovil = new EstadoMovilHistorial();
            estadoMovil.Fecha = DateTime.Now;
            estadoMovil.FechaAlta = DateTime.Now;
            estadoMovil.Usuario = new BaseRules<_VecinoVirtualUsuario>(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
            estadoMovil.Estado = estado;
            estadoMovil.Observaciones = observaciones;
            estadoMovil.Movil = movil;

            result.Return = estadoMovil;
            return result;
        }

        public Result<Movil> ProcesarCambioEstado(Movil movil, Enums.EstadoMovil estado, string observaciones)
        {
            return ProcesarCambioEstado(movil, estado, observaciones, true);
        }

        public Result<Movil> ProcesarCambioEstado(Movil movil, Enums.EstadoMovil estado, string observaciones, bool guardarCambios)
        {
            var estadoMovil = CrearEstado(movil, estado, observaciones);

            var result = new Result<Movil>();

            if (movil.Estados == null)
            {
                movil.Estados = new List<EstadoMovilHistorial>();
            }

            if (movil.Estados != null && movil.Estados.Count != 0)
            {
                foreach (EstadoMovilHistorial e in movil.Estados)
                {
                    e.Ultimo = false;
                }
            }

            estadoMovil.Return.Ultimo = true;
            movil.Estados.Add(estadoMovil.Return);

            if (guardarCambios)
            {
                var resultUpdate = ValidateUpdate(movil);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }

                resultUpdate = dao.Update(movil);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate);
                    return result;
                }
            }

            result.Return = movil;
            return result;
        }

        /* Dar de alta */
        public Result<Resultado_Movil> DarDeAlta(Comando_Movil comando)
        {
            var result = new Result<Resultado_Movil>();

            if (!comando.Id.HasValue)
            {
                result.AddErrorPublico("Error al actualizar el móvil.");
                return result;
            }

            var r = GetById((int)comando.Id);
            if (!r.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud.");
                return result;
            }

            var movil = r.Return;
            //var resultValidar = ValidarDarDeBaja(movil);
            //if (!resultValidar.Ok)
            //{
            //    result.Copy(resultValidar.Errores);
            //    return result;
            //}

            movil.FechaBaja = null;

            var resultInsert = base.Update(movil);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            //devuelvo el resultado del movil 
            var r_movil = new Resultado_Movil(resultInsert.Return);
            result.Return = r_movil;
            return result;
        }

        /*Orden de trabajo*/
        public Result<bool> EntrarEnOrdenTrabajo(Movil movil, string numeroOT)
        {
            var result = new Result<bool>();
            var list = new List<Enums.EstadoOrdenTrabajo>();
            list.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);

            var comando = new Comando_CambioEstado();
            comando.EstadoKeyValue = (int)Enums.EstadoMovil.OCUPADO;
            comando.Id = movil.Id;
            comando.Observaciones = "Por entrar en Orden de Trabajo N°" + numeroOT;

            result = CambiarEstado(comando);
            return result;
        }

        public Result<bool> SalirDeOrdenTrabajo(Movil movil, Enums.EstadoOrdenTrabajo estadoOT, OrdenTrabajo ot)
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

            var resultEstado = ProcesarCambioEstado(movil, Enums.EstadoMovil.DISPONIBLE, mensaje + " la Orden de Trabajo N° " + ot.Numero + "/" + ot.Año);
            if (!resultEstado.Ok)
            {
                result.Copy(resultEstado.Errores);
                return result;
            }

            result.Return = true;
            return result;


        }


        /*Flota*/
        public Result<bool> EntrarEnFlota(Movil movil, Flota flota)
        {
            var result = new Result<bool>();
            var comando = new Comando_CambioEstado();
            comando.EstadoKeyValue = (int)Enums.EstadoMovil.ENFLOTA;
            comando.Id = movil.Id;
            comando.Observaciones = "Por entrar en Flota " + flota.Nombre;

            result = CambiarEstado(comando);
            if (!result.Ok)
            {
                return result;
            }

            movil.FlotaActiva = flota;
            var resultUpdate = Update(movil);
            if (!resultUpdate.Ok)
            {
                result.Copy(resultUpdate.Errores);
                return result;
            }

            result.Return = true;
            return result;
        }

        public Result<bool> SalirDeFlota(Movil movil,Flota flota)
        {
            var result = new Result<bool>();
            var comando = new Comando_CambioEstado();
            comando.EstadoKeyValue =(int)Enums.EstadoMovil.DISPONIBLE;
            comando.Id = movil.Id;
            comando.Observaciones = "Por haberse terminado el turno de la flota "+flota.Nombre;

            result = CambiarEstado(comando);
            if (!result.Ok)
            {
                return result;
            }

            movil.FlotaActiva = null;
            var resultUpdate = Update(movil);
            if (!resultUpdate.Ok)
            {
                result.Copy(resultUpdate.Errores);
                return result;
            }

            result.Return = true;
            return result;

        }

    }
}
