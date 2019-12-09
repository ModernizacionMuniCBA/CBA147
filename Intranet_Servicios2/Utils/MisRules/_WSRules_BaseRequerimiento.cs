using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Model;
using Rules.Rules.Reportes;
using Model.Resultados;
using Intranet_Servicios2;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Rules.Rules.Mails;
using System.Configuration;
using Model.Comandos;
using Intranet_Servicios2.v1.Entities.Comandos;
using Model.Entities;
using Intranet_Servicios2.v1.Entities.Resultados;
using Intranet_Servicios2.Utils.Entities.Comando;
using Intranet_Servicios.Utils;

namespace Intranet_Servicios2.Utils.MisRules
{
    public class _WSRules_BaseRequerimiento : _WSRules_Base<Requerimiento>
    {
        private readonly RequerimientoRules rules;

        public _WSRules_BaseRequerimiento(UsuarioLogueado data)
            : base(data)
        {
            rules = new RequerimientoRules(data);
        }


        public ResultadoServicio<Requerimiento> Insertar(ComandoAppBase_Requerimiento comando)
        {
            var resultado = new ResultadoServicio<Requerimiento>();

            try
            {
                //var limiteRules = new LimiteRequerimientosPorUsuarioRules(getUsuarioLogueado());
                //var resultLimite = limiteRules.ValidarLimiteRequerimientos();
                //if (!resultLimite.Ok)
                //{
                //    resultado.Error = "Error al intentar insertar el requerimiento.";
                //    return resultado;
                //}

                //var limite = resultLimite.Return;
                //var limitePermitido = ConfigurationManager.AppSettings["LIMITE_POR_USUARIO"];
                //if (limite != null && limite.Contador >= Int32.Parse(limitePermitido))
                //{
                //    resultado.Error = "Usted ha superado la cantidad máxima de requerimientos permitido por persona.";
                //    return resultado;
                //}


                if (comando == null || comando.Autenticacion == null || comando.Domicilio == null)
                {
                    resultado.Error = "Solicitud inválida";
                    return resultado;
                }


                //Valido ReCaptcha
                bool validarReCaptcha = bool.Parse(ConfigurationManager.AppSettings["RECAPTCHA_VALIDAR"]);
                bool puedoSaltarValidacionCatpcha = ConfigurationManager.AppSettings["RECAPTCHA_KEY_SALTAR_VALIDACION"] == comando.Autenticacion.KeyValidacionReCaptcha;
                if (validarReCaptcha && !puedoSaltarValidacionCatpcha)
                {
                    if (!ReCaptchaClass.Validate(comando.Autenticacion.ReCaptcha))
                    {
                        resultado.Error = "ReCaptcha inválido";
                        return resultado;
                    }
                }

                //Armo el comando para isnertar
                var comando_intranet = new Comando_RequerimientoIntranet();
                comando_intranet.Descripcion = comando.Descripcion;
                comando_intranet.Domicilio = new Comando_Domicilio()
                {
                    Direccion = comando.Domicilio.Direccion,
                    Observaciones = comando.Domicilio.Observaciones,
                    Latitud = comando.Domicilio.Latitud,
                    Longitud = comando.Domicilio.Longitud
                };
                comando_intranet.IdMotivo = comando.IdMotivo;
                comando_intranet.Imagen = comando.Imagen;

                //POR AHORA PONGO CUALQUIER USER AGENT
                comando_intranet.UserAgent = "Sin datos";
                comando_intranet.TipoDispositivo = Enums.TipoDispositivo.DESCKTOP;

                //El referente soy yo porque lo auto creo
                comando_intranet.IdUsuarioReferente = getUsuarioLogueado().Usuario.Id;

                //Origen
                comando_intranet.OrigenAlias = comando.Autenticacion.OrigenAlias;
                comando_intranet.OrigenSecret = comando.Autenticacion.OrigenKey;

                //Inserto
                var resultadoInsert = rules.Insertar(comando_intranet);
                if (!resultadoInsert.Ok)
                {
                    resultado.Error = resultadoInsert.Errores.ToStringPublico();
                    return resultado;
                }

                //if (limite == null)
                //{
                //    var limiteNuevo = new LimiteRequerimientosPorUsuario();
                //    limiteNuevo.IdUsuarioCreador = (int)comando_intranet.IdUsuarioReferente;
                //    limiteNuevo.Fecha = DateTime.Now;
                //    limiteNuevo.Contador = 1;
                //    limiteRules.Insert(limiteNuevo);
                //}
                //else
                //{
                //    limite.Contador += 1;
                //    limite.IdUsuarioCreador = (int)comando_intranet.IdUsuarioReferente;
                //    limite.Fecha = DateTime.Now;
                //    limiteRules.Update(limite);
                //}


                resultado.Return = rules.GetByIdObligatorio(resultadoInsert.Return.Id).Return;
                return resultado;
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }
        }

        public ResultadoServicio<bool?> EnviarEmailComprobante(int idRequerimiento)
        {
            var result = new ResultadoServicio<bool?>();
            result.Return = false;

            //Valido que pueda manipular
            var resultPuedeManipular = PuedeManipularRequerimiento(getUsuarioLogueado().Usuario.Id, idRequerimiento);
            if (!resultPuedeManipular.Ok)
            {
                result.Error = resultPuedeManipular.Error;
                return result;
            }

            if (!resultPuedeManipular.Return)
            {
                result.Error = "No tiene permisos para manipular el requerimiento";
                return result;
            }

            try
            {
                var listInt = new List<int>();
                var idUsuario = getUsuarioLogueado().Usuario.Id;
                listInt.Add(idUsuario);
                var resultMail = new RequerimientoMailRules(getUsuarioLogueado()).EnviarComprobanteAtencion(idRequerimiento, listInt, null);
                if (!resultMail.Ok)
                {
                    result.Error = resultMail.ToStringPublico();
                    return result;
                }

                result.Return = true;
            }
            catch (Exception e)
            {
                result.Return = false;
            }
            return result;
        }

        public ResultadoServicio<bool?> Cancelar(int id)
        {
            var result = new ResultadoServicio<bool?>();

            //Valido que pueda manipular
            try
            {
                var resultPuedeManipular = PuedeManipularRequerimiento(getUsuarioLogueado().Usuario.Id, id);
                if (!resultPuedeManipular.Ok)
                {
                    result.Error = resultPuedeManipular.Error;
                    return result;
                }

                if (!resultPuedeManipular.Return)
                {
                    result.Error = "No tiene permisos para manipular el requerimiento";
                    return result;
                }

                //Consulto
                var resultConsulta = rules.GetById(id);
                if (!resultConsulta.Ok)
                {
                    result.Error = resultConsulta.Errores.ToStringPublico();
                    return result;
                }

                var rq = resultConsulta.Return;
                if (rq == null)
                {
                    result.Error = "El requerimiento no existe";
                    return result;
                }

                //Valido el estado que sea Nuevo
                if (rq.GetUltimoEstado() == null || rq.GetUltimoEstado().Estado.KeyValue != Enums.EstadoRequerimiento.NUEVO)
                {
                    result.Error = "El requerimiento no puede darse de baja porque su estado no lo permite";
                    return result;
                }

                //Doy de baja
                var resultDarDeBaja = rules.Cancelar(rq.Id, "El requerimiento ha sido cancelador por su creador.");
                if (!resultDarDeBaja.Ok)
                {
                    result.Error = resultDarDeBaja.Errores.ToStringPublico();
                    return result;
                }

                result.Return = true;

            }
            catch (Exception e)
            {
                result.Error = "Error procesando la solicitud";
                MiLog_WS.Info(e);
            }
            return result;
        }

        public ResultadoServicio<bool> PuedeManipularRequerimiento(int idUsuarioReferente, int idRq)
        {
            var result = new ResultadoServicio<bool>();

            //Valido que el id del reclamo pertenezce a alguno de mis reclamos
            var resultConsulta = rules.GetIdsByFilters(new Model.Consultas.Consulta_Requerimiento()
            {
                IdsUsuarioReferente = new List<int>() { idUsuarioReferente }
            }, null);

            if (!resultConsulta.Ok)
            {
                result.Error = resultConsulta.Errores.ToStringPublico();
                return result;
            }

            if (!resultConsulta.Return.Contains(idRq))
            {
                result.Error = "El requerimiento indicado no le pertenece al usuario logeado";
                return result;
            }

            result.Return = true;
            return result;
        }


        #region Utils

        //private ResultServicio<List<Resultado_Requerimiento>> TransformarEstadosPublicos(List<Resultado_Requerimiento> data)
        //{
        //    var resultado = new ResultServicio<List<Resultado_Requerimiento>>();

        //    var resultadoEstados = new EstadoRequerimientoRules(getUsuarioLogueado()).GetAll();
        //    if (!resultadoEstados.Ok)
        //    {
        //        resultado.Error = resultadoEstados.Error;
        //        return resultado;
        //    }

        //    var estados = resultadoEstados.Return.Select(x => new Resultado_EstadoRequerimiento(x)).ToList();

        //    foreach (var rq in data)
        //    {
        //        if (rq.Estado.Estado.KeyValuePublico.HasValue)
        //        {
        //            rq.Estado.Estado = TransformarEstadoPublico(estados, rq.Estado.Estado);
        //        }
        //    }

        //    resultado.Return = data;
        //    return resultado;
        //}

        //private ResultServicio<List<Resultado_RequerimientoDetalle>> TransformarEstadosPublicos(List<Resultado_RequerimientoDetalle> data)
        //{
        //    var resultado = new ResultServicio<List<Resultado_RequerimientoDetalle>>();

        //    var resultadoEstados = new EstadoRequerimientoRules(getUsuarioLogueado()).GetAll();
        //    if (!resultadoEstados.Ok)
        //    {
        //        resultado.Error = resultadoEstados.Error;
        //        return resultado;
        //    }

        //    var estados = resultadoEstados.Return.Select(x => new Resultado_EstadoRequerimiento(x)).ToList();

        //    foreach (var rq in data)
        //    {
        //        //Transformo el estado actual
        //        if (rq.Estado.Estado.KeyValuePublico.HasValue)
        //        {
        //            rq.Estado.Estado = TransformarEstadoPublico(estados, rq.Estado.Estado);
        //        }

        //        //Transformo el historial
        //        if (rq.HistorialEstados != null && rq.HistorialEstados.Count != 0)
        //        {
        //            foreach (var estado in rq.HistorialEstados)
        //            {
        //                estado.Estado = TransformarEstadoPublico(estados, estado.Estado);
        //            }
        //        }
        //    }

        //    resultado.Return = data;
        //    return resultado;
        //}

        //private EstadoRequerimiento TransformarEstadoPublico(int keyValue)
        //{
        //    if (estado.KeyValuePublico.HasValue)
        //    {
        //        //Cambio nombre y color del estado
        //        var estadoNuevo = estados.Where(x => x.KeyValue == estado.KeyValuePublico.Value).FirstOrDefault();
        //        estado.Nombre = estadoNuevo.Nombre;
        //        estado.Color = estadoNuevo.Color;
        //    }

        //    return estado;

        //}

        #endregion


        //public ResultServicio<Resultado_Ajustes> GetAjustes()
        //{
        //    var resultado = new ResultServicio<Resultado_Ajustes>();

        //    var resultConsulta = new AjustesRules(getUsuarioLogueado()).Get();
        //    if (!resultConsulta.Ok)
        //    {
        //        resultado.Error = resultConsulta.Errores.ToStringPublico();
        //        return resultado;
        //    }

        //    resultado.Return = new Resultado_Ajustes(resultConsulta.Return);
        //    return resultado;
        //}

        //public ResultServicio<bool> ValidarVersionAppAndroid(string version)
        //{
        //    var resultado = new ResultServicio<bool>();
        //    var resultAjustes = GetAjustes();
        //    if (!resultAjustes.Ok)
        //    {
        //        resultado.Error = resultAjustes.Error;
        //        return resultado;
        //    }

        //    resultado.Return = resultAjustes.Return.AppAndroid == version;
        //    return resultado;
        //}

        //public ResultServicio<bool> ValidarVersionAppIOS(string version)
        //{
        //    var resultado = new ResultServicio<bool>();
        //    var resultAjustes = GetAjustes();
        //    if (!resultAjustes.Ok)
        //    {
        //        resultado.Error = resultAjustes.Error;
        //        return resultado;
        //    }

        //    resultado.Return = resultAjustes.Return.AppIOS == version;
        //    return resultado;
        //}

    }
}
