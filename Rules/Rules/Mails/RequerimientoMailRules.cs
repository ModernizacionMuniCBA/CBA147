using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Telerik.Reporting.Processing;
using Rules.Rules.Reportes;

namespace Rules.Rules.Mails
{
    public class RequerimientoMailRules : BaseMailRules
    {
        public RequerimientoMailRules(UsuarioLogueado data)
            : base(data)
        {
        }

        /** Si mail es null manda al e-mail del usuario referente o persona fisica **/
        //public Result<bool> EnviarComprobanteAtencion(int id)
        //{
        //    return EnviarComprobanteAtencion(id, null);
        //}

        //public Result<bool> EnviarComprobanteAtencion(int id, int idUsuario, string mail = null)
        //{
        //    MiLog.Info("Por empezar a enviar e-mail comprobante");
        //    MiLog.Info("Id: " + id);
        //    MiLog.Info("Email: " + mail);

        //    var resultado = new Result<bool>();

        //    MiLog.Info("Por buscar el rq");
        //    var resultRq = new RequerimientoRules(getUsuarioLogueado()).GetById(id);
        //    if (!resultRq.Ok)
        //    {
        //        MiLog.Info("Error: " + resultRq.Errores.ToStringCompleto());

        //        resultado.Copy(resultRq.Errores);
        //        return resultado;
        //    }


        //    var rq = resultRq.Return;
        //    if (rq == null)
        //    {
        //        MiLog.Info("Error: No existe el rq");

        //        resultado.AddErrorPublico("Requerimiento no encontrado");
        //        return resultado;
        //    }

        //    try
        //    {

        //        if (rq.UsuariosReferentes == null || rq.UsuariosReferentes.Count == 1)
        //        {
        //            resultado.AddErrorPublico("El requerimiento no tiene asociado ningun contacto");
        //            return resultado;
        //        }

        //        var comando = new ComandoMail();

        //        //Receptor
        //        string receptoNombre, receptorEmail, receptorUsername = null;
        //        var usuario = rq.UsuariosReferentes.Where(x => x.UsuarioReferente.Id == idUsuario).FirstOrDefault();
        //        if (usuario != null)
        //        {
        //            receptoNombre = usuario.UsuarioReferente.Nombre + " " + usuario.UsuarioReferente.Apellido;
        //            receptorEmail = usuario.UsuarioReferente.Email;
        //            receptorUsername = usuario.UsuarioReferente.Username;
        //        }
        //        else
        //        {
        //            receptoNombre = rq.PersonaFisica.Nombre + " " + rq.PersonaFisica.Apellido;
        //            receptorEmail = rq.PersonaFisica.Mail;
        //        }

        //        if (mail != null)
        //        {
        //            receptorEmail = mail;
        //        }

        //        if (string.IsNullOrEmpty(receptoNombre))
        //        {
        //            receptoNombre = "Vecino";
        //        }

        //        if (string.IsNullOrEmpty(receptorEmail))
        //        {
        //            resultado.AddErrorPublico("El requerimiento no tiene asociado ningun e-mail al cual enviar el comprobante de atención");
        //            return resultado;
        //        }

        //        if (string.IsNullOrEmpty(receptorUsername))
        //        {
        //            receptorUsername = "Sin usuario";
        //            return resultado;
        //        }

        //        comando.ReceptorNombre = receptoNombre;
        //        comando.ReceptorMail = receptorEmail;

        //        //Asunto
        //        comando.Asunto = "Requerimiento N° " + rq.GetNumero() + " - Motivo: " + Utils.toTitleCase(rq.Motivo.Nombre);

        //        //Cuerpo
        //        var cuerpo = GetHtmlComprobanteAtencion(rq.GetNumero(), receptoNombre, receptorUsername);
        //        comando.Contenido = cuerpo;
        //        comando.EsHTML = true;



        //        var resultConsultaRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetDetalleById(id);

        //        var reporte = new RequerimientoReporteRules(getUsuarioLogueado()).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return, idUsuario, true);

        //        if (!reporte.Ok)
        //        {
        //            resultado.Copy(reporte.Errores);
        //            return resultado;
        //        }

        //        if (reporte.Return == null)
        //        {
        //            resultado.AddErrorInterno("El reporte es null");
        //            return resultado;
        //        }

        //        Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
        //        RenderingResult resultReporte = reportProcessor.RenderReport("PDF", reporte.Return, null);
        //        comando.Adjuntos = new List<AdjuntoMail>();
        //        comando.Adjuntos.Add(new AdjuntoMail()
        //        {
        //            Data = resultReporte.DocumentBytes,
        //            Nombre = "comprobante_" + rq.Numero + "_" + rq.Año + ".pdf"
        //        });

        //        var resultadoEnviar = EnviarEmail(comando);
        //        if (!resultadoEnviar.Ok)
        //        {
        //            resultado.AddErrorPublico("Error procesando al operacion");
        //            return resultado;
        //        }

        //        resultado.Return = true;
        //    }
        //    catch (Exception e)
        //    {
        //        MiLog.Info("Error: " + e.Message);

        //        resultado.AddErrorInterno(e);
        //    }

        //    return resultado;
        //}

        public Result<bool> EnviarComprobanteAtencion(int id, IList<int> idsUsuarios, string mail = null)
        {
            MiLog.Info("Por empezar a enviar e-mail comprobante");
            MiLog.Info("Id: " + id);

            var resultado = new Result<bool>();

            MiLog.Info("Por buscar el rq");
            var resultRq = new RequerimientoRules(getUsuarioLogueado()).GetById(id);
            if (!resultRq.Ok)
            {
                MiLog.Info("Error: " + resultRq.Errores.ToStringCompleto());

                resultado.Copy(resultRq.Errores);
                return resultado;
            }


            var rq = resultRq.Return;
            if (rq == null)
            {
                MiLog.Info("Error: No existe el rq");

                resultado.AddErrorPublico("Requerimiento no encontrado");
                return resultado;
            }

            try
            {


                if ((rq.UsuariosReferentes == null || rq.UsuariosReferentes.Count == 0) && mail == null)
                {
                    resultado.AddErrorPublico("El requerimiento no tiene asociado ningún usuario");
                    return resultado;
                }

                if (!String.IsNullOrWhiteSpace(mail))
                {
                    var result = EnviarComprobante(rq, "Vecino", mail, null, null);
                    if (!result.Ok)
                    {
                        resultado.Copy(result.Errores);
                        return resultado;
                    }
                }

                foreach (int idUsuario in idsUsuarios)
                {
                    var usuario = rq.UsuariosReferentes.Where(x => x.UsuarioReferente.Id == idUsuario).FirstOrDefault();
                    var result = EnviarComprobanteAUsuario(rq, usuario);
                    if (!result.Ok)
                    {
                        resultado.Copy(result.Errores);
                        return resultado;
                    }
                }


                resultado.Return = true;
            }
            catch (Exception e)
            {
                MiLog.Info("Error: " + e.Message);

                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<bool> EnviarComprobanteAUsuario(Requerimiento rq, UsuarioReferentePorRequerimiento usuario)
        {
            var resultado = new Result<bool>();
            //Receptor
            string receptoNombre, receptorEmail, receptorUsername = null;
            if (usuario != null)
            {
                receptoNombre = usuario.UsuarioReferente.Nombre + " " + usuario.UsuarioReferente.Apellido;
                receptorEmail = usuario.UsuarioReferente.Email;
                receptorUsername = usuario.UsuarioReferente.Username;
            }
            else
            {
                receptoNombre = rq.PersonaFisica.Nombre + " " + rq.PersonaFisica.Apellido;
                receptorEmail = rq.PersonaFisica.Mail;
            }

            if (string.IsNullOrEmpty(receptoNombre))
            {
                receptoNombre = "Vecino";
            }

            if (string.IsNullOrEmpty(receptorEmail))
            {
                resultado.AddErrorPublico("El requerimiento no tiene asociado ningun e-mail al cual enviar el comprobante de atención");
                return resultado;
            }

            if (string.IsNullOrEmpty(receptorUsername))
            {
                receptorUsername = "Sin usuario";
            }

            return EnviarComprobante(rq, receptoNombre, receptorEmail, receptorUsername, usuario.UsuarioReferente.Id);
        }

        public Result<bool> EnviarComprobante(Requerimiento rq, string receptorNombre, string receptorEmail, string receptorUsername, int? idUsuarioReferente)
        {
            var resultado = new Result<bool>();
            var comando = new ComandoMail();
            comando.ReceptorNombre = receptorNombre;
            comando.ReceptorMail = receptorEmail;

            //Asunto
            comando.Asunto = "Requerimiento N° " + rq.GetNumero() + " - Motivo: " + Utils.toTitleCase(rq.Motivo.Nombre);

            //Cuerpo
            var cuerpo = GetHtmlComprobanteAtencion(rq.GetNumero(), receptorNombre, receptorUsername);
            comando.Contenido = cuerpo;
            comando.EsHTML = true;

            var resultConsultaRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetDetalleById(rq.Id);

            var reporte = new RequerimientoReporteRules(getUsuarioLogueado()).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return, idUsuarioReferente, false);

            if (!reporte.Ok)
            {
                resultado.Copy(reporte.Errores);
                return resultado;
            }

            if (reporte.Return == null)
            {
                resultado.AddErrorInterno("El reporte es null");
                return resultado;
            }

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult resultReporte = reportProcessor.RenderReport("PDF", reporte.Return, null);
            comando.Adjuntos = new List<AdjuntoMail>();
            comando.Adjuntos.Add(new AdjuntoMail()
            {
                Data = resultReporte.DocumentBytes,
                Nombre = "comprobante_" + rq.Numero + "_" + rq.Año + ".pdf"
            });

            var resultadoEnviar = EnviarEmail(comando);
            if (!resultadoEnviar.Ok)
            {
                resultado.AddErrorPublico("Error al enviar el comprobante al alguno de los emails");
                return resultado;
            }

            return resultado;
        }

        public Result<bool> EnviarMensajeAUsuarios(int id, string mensaje)
        {
            var resultado = new Result<bool>();

            if (string.IsNullOrEmpty(mensaje))
            {
                resultado.AddErrorPublico("Debe ingresar algún mensaje");
                return resultado;
            }

            var resultRq = new RequerimientoRules(getUsuarioLogueado()).GetById(id);
            if (!resultRq.Ok)
            {
                resultado.Copy(resultRq.Errores);
                return resultado;
            }

            var rq = resultRq.Return;
            if (rq == null)
            {
                resultado.AddErrorPublico("Requerimiento no encontrado");
                return resultado;
            }

            try
            {
                var comando = new ComandoMail();

                //Receptor
                string receptorNombre = null;
                string receptorEmail = null;
                string receptorUsername = null;

                foreach (UsuarioReferentePorRequerimiento urxr in rq.UsuariosReferentes)
                {
                    var user = urxr.UsuarioReferente;
                    if (user != null)
                    {
                        receptorNombre = user.Nombre + " " + user.Apellido;
                        receptorEmail = user.Email;
                        receptorUsername = user.Username;
                    }
                    else
                    {
                        if (rq.PersonaFisica != null)
                        {
                            receptorNombre = rq.PersonaFisica.Nombre + " " + rq.PersonaFisica.Apellido;
                            receptorEmail = rq.PersonaFisica.Mail;
                        }
                    }


                    //Si no hay un e-mail asociado, error
                    if (string.IsNullOrEmpty(receptorEmail))
                    {
                        resultado.AddErrorPublico("El requerimiento no tiene asociado ningun e-mail al cual enviar el comprobante de atención");
                        return resultado;
                    }

                    //Si no hay un nombbre asociado
                    if (string.IsNullOrEmpty(receptorNombre))
                    {
                        receptorNombre = "Vecino";
                    }

                    //Si no hay un username asociado
                    if (string.IsNullOrEmpty(receptorUsername))
                    {
                        receptorUsername = "Sin usuario";
                    }

                    comando.ReceptorNombre = receptorNombre;
                    comando.ReceptorMail = receptorEmail;

                    //Asunto
                    comando.Asunto = "Nuevo mensaje en requerimiento N° " + rq.GetNumero();

                    //Cuerpo
                    var cuerpo = GetHtmlMensaje(rq.GetNumero(), receptorNombre, receptorUsername, mensaje);
                    comando.Contenido = cuerpo;
                    comando.EsHTML = true;

                    var entity = new MensajePorRequerimiento();
                    entity.Texto = mensaje;
                    entity.RequerimientoAsociado = rq;
                    entity.UsuarioEmisor = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
                    entity.UsuarioReceptor = user;
                    entity.EmailReceptor = receptorEmail;
                    //por defecto, se pone como que no se envió
                    entity.Enviado = false;

                    //Inserto
                    var resultInsertar = new BaseRules<MensajePorRequerimiento>(getUsuarioLogueado()).Insert(entity);
                    if (!resultInsertar.Ok)
                    {
                        resultado.AddErrorPublico("Error al enviar el mensaje");
                        return resultado;
                    }

                    //Mando
                    var resultadoEnviar = EnviarEmail(comando);
                    if (!resultadoEnviar.Ok)
                    {
                        resultado.AddErrorPublico("Error procesando al operacion");
                        return resultado;
                    }

                    resultado.Return = true;

                    //Actualizo 
                    resultInsertar.Return.Enviado = true;
                    var resultActualizar = new BaseRules<MensajePorRequerimiento>(getUsuarioLogueado()).Update(entity);
                }

                return resultado;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<bool> EnviarCambioEstado(int id)
        {
            var resultado = new Result<bool>();

            try
            {

                var resultRq = new RequerimientoRules(getUsuarioLogueado()).GetById(id);
                if (!resultRq.Ok)
                {
                    resultado.Copy(resultRq.Errores);
                    return resultado;
                }

                var rq = resultRq.Return;
                if (rq == null)
                {
                    resultado.AddErrorPublico("Requerimiento no encontrado");
                    return resultado;
                }


                if (rq.UsuariosReferentes == null && rq.UsuariosReferentes.Count == 0)
                {
                    resultado.AddErrorPublico("El requerimiento no tiene asociado ningun contacto");
                    return resultado;
                }

                var comando = new ComandoMail();


                //Receptor
                string receptorNombre, receptorEmail, receptorUsername = null;
                foreach (UsuarioReferentePorRequerimiento urxr in rq.UsuariosReferentes)
                {
                    if (urxr.UsuarioReferente != null)
                    {
                        receptorNombre = urxr.UsuarioReferente.Nombre + " " + urxr.UsuarioReferente.Apellido;
                        receptorEmail = urxr.UsuarioReferente.Email;
                        receptorUsername = urxr.UsuarioReferente.Username;
                    }
                    else
                    {
                        receptorNombre = rq.PersonaFisica.Nombre + " " + rq.PersonaFisica.Apellido;
                        receptorEmail = rq.PersonaFisica.Mail;
                    }

                    if (string.IsNullOrEmpty(receptorNombre))
                    {
                        receptorNombre = "Vecino";
                    }

                    if (string.IsNullOrEmpty(receptorEmail))
                    {
                        resultado.AddErrorPublico("El requerimiento no tiene asociado ningun e-mail al cual enviar el comprobante de atención");
                        return resultado;
                    }

                    if (string.IsNullOrEmpty(receptorUsername))
                    {
                        receptorUsername = "Sin usuario";
                        return resultado;
                    }

                    comando.ReceptorNombre = receptorNombre;
                    comando.ReceptorMail = receptorEmail;

                    //Estado
                    var ultimoEstadoHistorial = rq.GetUltimoEstado();
                    if (ultimoEstadoHistorial == null)
                    {
                        resultado.AddErrorPublico("El requerimiento no tiene ningun estado");
                        return resultado;
                    }

                    var estado = ultimoEstadoHistorial.Estado;
                    if (estado == null)
                    {
                        resultado.AddErrorPublico("El requerimiento no tiene ningun estado");
                        return resultado;
                    }

                    //Asunto
                    comando.Asunto = "Cambios en el Requerimiento N° " + rq.GetNumero();

                    //Cuerpo
                    var cuerpo = GetHtmlCambioEstado(rq.GetNumero(), receptorNombre, receptorUsername, Utils.DateTimeToString(ultimoEstadoHistorial.Fecha), Utils.toTitleCase(estado.Nombre), ultimoEstadoHistorial.Observaciones);
                    comando.Contenido = cuerpo;
                    comando.EsHTML = true;


                    var resultConsultaRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetDetalleById(id);

                    var reporte = new RequerimientoReporteRules(getUsuarioLogueado()).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return,urxr.Id, true);

                    if (!reporte.Ok)
                    {
                        resultado.Copy(reporte.Errores);
                        return resultado;
                    }

                    if (reporte.Return == null)
                    {
                        resultado.AddErrorInterno("El reporte es null");
                        return resultado;
                    }

                    Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                    RenderingResult resultReporte = reportProcessor.RenderReport("PDF", reporte.Return, null);
                    comando.Adjuntos = new List<AdjuntoMail>();
                    comando.Adjuntos.Add(new AdjuntoMail()
                    {
                        Data = resultReporte.DocumentBytes,
                        Nombre = "comprobante_" + rq.Numero + "_" + rq.Año + ".pdf"
                    });

                    //Mando
                    var resultadoEnviar = EnviarEmail(comando);
                    if (!resultadoEnviar.Ok)
                    {
                        resultado.AddErrorPublico("Error procesando la operacion");
                        return resultado;
                    }
                }

                resultado.Return = true;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
                MiLog.Info(e.Message);
                if (e.InnerException != null)
                {
                    MiLog.Info(e.InnerException.ToString());
                }
            }

            return resultado;
        }


        #region HTMLS
        private string GetHtmlComprobanteAtencion(string numeroRequerimiento, string nombreUsuario, string usernameUsuario)
        {
            var html = GetHtmlPuroComprobanteAtencion();
            html = ReemplazarDatosBasicosEnHTML(html);

            html = html.Replace("{numero-requerimiento}", numeroRequerimiento);
            html = html.Replace("{nombre-usuario}", nombreUsuario);
            html = html.Replace("{username-usuario}", usernameUsuario);
            return html;
        }
        private string GetHtmlPuroComprobanteAtencion()
        {
            return @"
<html style='margin: 0px; padding: 0px;' xmlns='http://www.w3.org/1999/xhtml'>
<head>
  <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
  <!--[if !mso]><!-->
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <!--<![endif]-->
  <meta name='viewport' content='width=device-width'>
  <style type='text/css'>
    @media only screen and (min-width: 620px) {
      .wrapper {
        min-width: 600px !important;
      }
      .wrapper h1 {}
      .wrapper h1 {
        font-size: 36px !important;
        line-height: 43px !important;
      }
      .wrapper h2 {}
      .wrapper h2 {
        font-size: 22px !important;
        line-height: 31px !important;
      }
      .wrapper h3 {}
      .wrapper h3 {
        font-size: 18px !important;
        line-height: 26px !important;
      }
      .column {}
      .wrapper .size-8 {
        font-size: 8px !important;
        line-height: 14px !important;
      }
      .wrapper .size-9 {
        font-size: 9px !important;
        line-height: 16px !important;
      }
      .wrapper .size-10 {
        font-size: 10px !important;
        line-height: 18px !important;
      }
      .wrapper .size-11 {
        font-size: 11px !important;
        line-height: 19px !important;
      }
      .wrapper .size-12 {
        font-size: 12px !important;
        line-height: 19px !important;
      }
      .wrapper .size-13 {
        font-size: 13px !important;
        line-height: 21px !important;
      }
      .wrapper .size-14 {
        font-size: 14px !important;
        line-height: 21px !important;
      }
      .wrapper .size-15 {
        font-size: 15px !important;
        line-height: 23px !important;
      }
      .wrapper .size-16 {
        font-size: 16px !important;
        line-height: 24px !important;
      }
      .wrapper .size-17 {
        font-size: 17px !important;
        line-height: 26px !important;
      }
      .wrapper .size-18 {
        font-size: 18px !important;
        line-height: 26px !important;
      }
      .wrapper .size-20 {
        font-size: 20px !important;
        line-height: 28px !important;
      }
      .wrapper .size-22 {
        font-size: 22px !important;
        line-height: 31px !important;
      }
      .wrapper .size-24 {
        font-size: 24px !important;
        line-height: 32px !important;
      }
      .wrapper .size-26 {
        font-size: 26px !important;
        line-height: 34px !important;
      }
      .wrapper .size-28 {
        font-size: 28px !important;
        line-height: 36px !important;
      }
      .wrapper .size-30 {
        font-size: 30px !important;
        line-height: 38px !important;
      }
      .wrapper .size-32 {
        font-size: 32px !important;
        line-height: 40px !important;
      }
      .wrapper .size-34 {
        font-size: 34px !important;
        line-height: 43px !important;
      }
      .wrapper .size-36 {
        font-size: 36px !important;
        line-height: 43px !important;
      }
      .wrapper .size-40 {
        font-size: 40px !important;
        line-height: 47px !important;
      }
      .wrapper .size-44 {
        font-size: 44px !important;
        line-height: 50px !important;
      }
      .wrapper .size-48 {
        font-size: 48px !important;
        line-height: 54px !important;
      }
      .wrapper .size-56 {
        font-size: 56px !important;
        line-height: 60px !important;
      }
      .wrapper .size-64 {
        font-size: 64px !important;
        line-height: 63px !important;
      }
    }
  </style>
  <style type='text/css'>
    body {
      margin: 0;
      padding: 0;
    }
    
    table {
      border-collapse: collapse;
      table-layout: fixed;
    }
    
    * {
      line-height: inherit;
    }
    
    [x-apple-data-detectors],
    [href^='tel'],
    [href^='sms'] {
      color: inherit !important;
      text-decoration: none !important;
    }
    
    .wrapper .footer__share-button a:hover,
    .wrapper .footer__share-button a:focus {
      color: #ffffff !important;
    }
    
    .btn a:hover,
    .btn a:focus,
    .footer__share-button a:hover,
    .footer__share-button a:focus,
    .email-footer__links a:hover,
    .email-footer__links a:focus {
      opacity: 0.8;
    }
    
    .preheader,
    .header,
    .layout,
    .column {
      transition: width 0.25s ease-in-out, max-width 0.25s ease-in-out;
    }
    
    .preheader td {
      padding-bottom: 8px;
    }
    
    .layout,
    div.header {
      max-width: 400px !important;
      -fallback-width: 95% !important;
      width: calc(100% - 20px) !important;
    }
    
    div.preheader {
      max-width: 360px !important;
      -fallback-width: 90% !important;
      width: calc(100% - 60px) !important;
    }
    
    .snippet,
    .webversion {
      Float: none !important;
    }
    
    .column {
      max-width: 400px !important;
      width: 100% !important;
    }
    
    .fixed-width.has-border {
      max-width: 402px !important;
    }
    
    .fixed-width.has-border .layout__inner {
      box-sizing: border-box;
    }
    
    .snippet,
    .webversion {
      width: 50% !important;
    }
    
    .ie .btn {
      width: 100%;
    }
    
    [owa] .column div,
    [owa] .column button {
      display: block !important;
    }
    
    .ie .column,
    [owa] .column,
    .ie .gutter,
    [owa] .gutter {
      display: table-cell;
      float: none !important;
      vertical-align: top;
    }
    
    .ie div.preheader,
    [owa] div.preheader,
    .ie .email-footer,
    [owa] .email-footer {
      max-width: 560px !important;
      width: 560px !important;
    }
    
    .ie .snippet,
    [owa] .snippet,
    .ie .webversion,
    [owa] .webversion {
      width: 280px !important;
    }
    
    .ie div.header,
    [owa] div.header,
    .ie .layout,
    [owa] .layout,
    .ie .one-col .column,
    [owa] .one-col .column {
      max-width: 600px !important;
      width: 600px !important;
    }
    
    .ie .fixed-width.has-border,
    [owa] .fixed-width.has-border,
    .ie .has-gutter.has-border,
    [owa] .has-gutter.has-border {
      max-width: 602px !important;
      width: 602px !important;
    }
    
    .ie .two-col .column,
    [owa] .two-col .column {
      max-width: 300px !important;
      width: 300px !important;
    }
    
    .ie .three-col .column,
    [owa] .three-col .column,
    .ie .narrow,
    [owa] .narrow {
      max-width: 200px !important;
      width: 200px !important;
    }
    
    .ie .wide,
    [owa] .wide {
      width: 400px !important;
    }
    
    .ie .two-col.has-gutter .column,
    [owa] .two-col.x_has-gutter .column {
      max-width: 290px !important;
      width: 290px !important;
    }
    
    .ie .three-col.has-gutter .column,
    [owa] .three-col.x_has-gutter .column,
    .ie .has-gutter .narrow,
    [owa] .has-gutter .narrow {
      max-width: 188px !important;
      width: 188px !important;
    }
    
    .ie .has-gutter .wide,
    [owa] .has-gutter .wide {
      max-width: 394px !important;
      width: 394px !important;
    }
    
    .ie .two-col.has-gutter.has-border .column,
    [owa] .two-col.x_has-gutter.x_has-border .column {
      max-width: 292px !important;
      width: 292px !important;
    }
    
    .ie .three-col.has-gutter.has-border .column,
    [owa] .three-col.x_has-gutter.x_has-border .column,
    .ie .has-gutter.has-border .narrow,
    [owa] .has-gutter.x_has-border .narrow {
      max-width: 190px !important;
      width: 190px !important;
    }
    
    .ie .has-gutter.has-border .wide,
    [owa] .has-gutter.x_has-border .wide {
      max-width: 396px !important;
      width: 396px !important;
    }
    
    .ie .fixed-width .layout__inner {
      border-left: 0 none white !important;
      border-right: 0 none white !important;
    }
    
    .ie .layout__edges {
      display: none;
    }
    
    .mso .layout__edges {
      font-size: 0;
    }
    
    .layout-fixed-width,
    .mso .layout-full-width {
      background-color: #ffffff;
    }
    
    @media only screen and (min-width: 620px) {
      .column,
      .gutter {
        display: table-cell;
        Float: none !important;
        vertical-align: top;
      }
      div.preheader,
      .email-footer {
        max-width: 560px !important;
        width: 560px !important;
      }
      .snippet,
      .webversion {
        width: 280px !important;
      }
      div.header,
      .layout,
      .one-col .column {
        max-width: 600px !important;
        width: 600px !important;
      }
      .fixed-width.has-border,
      .fixed-width.ecxhas-border,
      .has-gutter.has-border,
      .has-gutter.ecxhas-border {
        max-width: 602px !important;
        width: 602px !important;
      }
      .two-col .column {
        max-width: 300px !important;
        width: 300px !important;
      }
      .three-col .column,
      .column.narrow {
        max-width: 200px !important;
        width: 200px !important;
      }
      .column.wide {
        width: 400px !important;
      }
      .two-col.has-gutter .column,
      .two-col.ecxhas-gutter .column {
        max-width: 290px !important;
        width: 290px !important;
      }
      .three-col.has-gutter .column,
      .three-col.ecxhas-gutter .column,
      .has-gutter .narrow {
        max-width: 188px !important;
        width: 188px !important;
      }
      .has-gutter .wide {
        max-width: 394px !important;
        width: 394px !important;
      }
      .two-col.has-gutter.has-border .column,
      .two-col.ecxhas-gutter.ecxhas-border .column {
        max-width: 292px !important;
        width: 292px !important;
      }
      .three-col.has-gutter.has-border .column,
      .three-col.ecxhas-gutter.ecxhas-border .column,
      .has-gutter.has-border .narrow,
      .has-gutter.ecxhas-border .narrow {
        max-width: 190px !important;
        width: 190px !important;
      }
      .has-gutter.has-border .wide,
      .has-gutter.ecxhas-border .wide {
        max-width: 396px !important;
        width: 396px !important;
      }
    }
    
    @media only screen and (-webkit-min-device-pixel-ratio: 2),
    only screen and (min--moz-device-pixel-ratio: 2),
    only screen and (-o-min-device-pixel-ratio: 2/1),
    only screen and (min-device-pixel-ratio: 2),
    only screen and (min-resolution: 192dpi),
    only screen and (min-resolution: 2dppx) {
      .fblike {
        background-image: url(http://i7.cmail20.com/static/eb/master/13-the-blueprint-3/images/fblike@2x.png) !important;
      }
      .tweet {
        background-image: url(http://i8.cmail20.com/static/eb/master/13-the-blueprint-3/images/tweet@2x.png) !important;
      }
      .linkedinshare {
        background-image: url(http://i10.cmail20.com/static/eb/master/13-the-blueprint-3/images/lishare@2x.png) !important;
      }
      .forwardtoafriend {
        background-image: url(http://i9.cmail20.com/static/eb/master/13-the-blueprint-3/images/forward@2x.png) !important;
      }
    }
    
    @media (max-width: 321px) {
      .fixed-width.has-border .layout__inner {
        border-width: 1px 0 !important;
      }
      .layout,
      .column {
        min-width: 320px !important;
        width: 320px !important;
      }
      .border {
        display: none;
      }
    }
    
    .mso div {
      border: 0 none white !important;
    }
    
    .mso .w560 .divider {
      Margin-left: 260px !important;
      Margin-right: 260px !important;
    }
    
    .mso .w360 .divider {
      Margin-left: 160px !important;
      Margin-right: 160px !important;
    }
    
    .mso .w260 .divider {
      Margin-left: 110px !important;
      Margin-right: 110px !important;
    }
    
    .mso .w160 .divider {
      Margin-left: 60px !important;
      Margin-right: 60px !important;
    }
    
    .mso .w354 .divider {
      Margin-left: 157px !important;
      Margin-right: 157px !important;
    }
    
    .mso .w250 .divider {
      Margin-left: 105px !important;
      Margin-right: 105px !important;
    }
    
    .mso .w148 .divider {
      Margin-left: 54px !important;
      Margin-right: 54px !important;
    }
    
    .mso .size-8,
    .ie .size-8 {
      font-size: 8px !important;
      line-height: 14px !important;
    }
    
    .mso .size-9,
    .ie .size-9 {
      font-size: 9px !important;
      line-height: 16px !important;
    }
    
    .mso .size-10,
    .ie .size-10 {
      font-size: 10px !important;
      line-height: 18px !important;
    }
    
    .mso .size-11,
    .ie .size-11 {
      font-size: 11px !important;
      line-height: 19px !important;
    }
    
    .mso .size-12,
    .ie .size-12 {
      font-size: 12px !important;
      line-height: 19px !important;
    }
    
    .mso .size-13,
    .ie .size-13 {
      font-size: 13px !important;
      line-height: 21px !important;
    }
    
    .mso .size-14,
    .ie .size-14 {
      font-size: 14px !important;
      line-height: 21px !important;
    }
    
    .mso .size-15,
    .ie .size-15 {
      font-size: 15px !important;
      line-height: 23px !important;
    }
    
    .mso .size-16,
    .ie .size-16 {
      font-size: 16px !important;
      line-height: 24px !important;
    }
    
    .mso .size-17,
    .ie .size-17 {
      font-size: 17px !important;
      line-height: 26px !important;
    }
    
    .mso .size-18,
    .ie .size-18 {
      font-size: 18px !important;
      line-height: 26px !important;
    }
    
    .mso .size-20,
    .ie .size-20 {
      font-size: 20px !important;
      line-height: 28px !important;
    }
    
    .mso .size-22,
    .ie .size-22 {
      font-size: 22px !important;
      line-height: 31px !important;
    }
    
    .mso .size-24,
    .ie .size-24 {
      font-size: 24px !important;
      line-height: 32px !important;
    }
    
    .mso .size-26,
    .ie .size-26 {
      font-size: 26px !important;
      line-height: 34px !important;
    }
    
    .mso .size-28,
    .ie .size-28 {
      font-size: 28px !important;
      line-height: 36px !important;
    }
    
    .mso .size-30,
    .ie .size-30 {
      font-size: 30px !important;
      line-height: 38px !important;
    }
    
    .mso .size-32,
    .ie .size-32 {
      font-size: 32px !important;
      line-height: 40px !important;
    }
    
    .mso .size-34,
    .ie .size-34 {
      font-size: 34px !important;
      line-height: 43px !important;
    }
    
    .mso .size-36,
    .ie .size-36 {
      font-size: 36px !important;
      line-height: 43px !important;
    }
    
    .mso .size-40,
    .ie .size-40 {
      font-size: 40px !important;
      line-height: 47px !important;
    }
    
    .mso .size-44,
    .ie .size-44 {
      font-size: 44px !important;
      line-height: 50px !important;
    }
    
    .mso .size-48,
    .ie .size-48 {
      font-size: 48px !important;
      line-height: 54px !important;
    }
    
    .mso .size-56,
    .ie .size-56 {
      font-size: 56px !important;
      line-height: 60px !important;
    }
    
    .mso .size-64,
    .ie .size-64 {
      font-size: 64px !important;
      line-height: 63px !important;
    }
  </style>

  <!--[if !mso]><!-->
  <style type='text/css'>
    @import url(https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic|Ubuntu:400,700,400italic,700italic);
  </style>
  <link href='https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic|Ubuntu:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
  <!--<![endif]-->

  <style type='text/css'>
    body {
      background-color: #00a665;
    }
    
    .logo a:hover,
    .logo a:focus {
      color: #fff !important;
    }
    
    .mso .layout-has-border {
      border-top: 1px solid #004027;
      border-bottom: 1px solid #004027;
    }
    
    .mso .layout-has-bottom-border {
      border-bottom: 1px solid #004027;
    }
    
    .mso .border,
    .ie .border {
      background-color: #004027;
    }
    
    .mso h1,
    .ie h1 {}
    
    .mso h1,
    .ie h1 {
      font-size: 36px !important;
      line-height: 43px !important;
    }
    
    .mso h2,
    .ie h2 {}
    
    .mso h2,
    .ie h2 {
      font-size: 22px !important;
      line-height: 31px !important;
    }
    
    .mso h3,
    .ie h3 {}
    
    .mso h3,
    .ie h3 {
      font-size: 18px !important;
      line-height: 26px !important;
    }
    
    .mso .layout__inner,
    .ie .layout__inner {}
    
    .mso .footer__share-button p {}
    
    .mso .footer__share-button p {
      font-family: Ubuntu, sans-serif;
    }
  </style>
</head>
                      
<!--[if !mso]-->
<body class='full-padding' style='margin: 0px; padding: 0px; -webkit-text-size-adjust: 100%;'>
<!--<![endif]-->
<!--[if mso]>
<body class='mso'>
<![endif]-->
  <table class='wrapper' role='presentation' style='width: 100%; border-collapse: collapse; table-layout: fixed; min-width: 320px; background-color: rgb(0, 166, 101);' cellspacing='0' cellpadding='0'>
    <tbody>
      <tr>
        <td>
          <div role='banner'>
            <div class='preheader' style='margin: 0px auto; width: calc(28000% - 167440px); min-width: 280px; max-width: 560px;'>
              <div style='width: 100%; display: table; border-collapse: collapse;'>
                <!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 280px' valign='top'><![endif]-->
                <div class='snippet' style='padding: 10px 0px 5px; width: calc(14000% - 78120px); color: rgb(189, 189, 189); line-height: 19px; font-family: Ubuntu,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 140px; max-width: 280px;'>
                </div>
                <!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]-->
                <div class='webversion' style='padding: 10px 0px 5px; width: calc(14100% - 78680px); text-align: right; color: rgb(189, 189, 189); line-height: 19px; font-family: Ubuntu,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 139px; max-width: 280px;'>
                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>
            <div class='header' id='emb-email-header-container' style='margin: 0px auto; width: calc(28000% - 167400px); min-width: 320px; max-width: 600px;'>
              <!--[if (mso)|(IE)]><table align='center' class='header' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 600px'><![endif]-->
              <div align='center' class='logo emb-logo-margin-box' style='margin: 6px 20px 20px; color: rgb(195, 206, 217); line-height: 32px; font-family: Roboto,Tahoma,sans-serif; font-size: 26px;'>
                <div align='center' class='logo-center' id='emb-email-header'>
                  <a style='text-decoration: underline;' href='{url-muni}'>
                    <img width='243' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 243px;' alt='Municipalidad de Cordoba' src='{url-imagen-muni}'></div>
                  </a>
              </div>
              <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
            </div>
          </div>
          <div role='section'>
            <div class='layout one-col fixed-width' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
              <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse; background-color: rgb(255, 255, 255); border-radius: 8px;' emb-background-style=''>
                <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-fixed-width' emb-background-style><td style='width: 600px' class='w560'><![endif]-->
                <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(120, 119, 120); line-height: 24px; font-family: Lato,Tahoma,sans-serif; font-size: 16px; min-width: 320px; max-width: 600px;'>

                  <div align='center' style='line-height: 19px; font-size: 12px; font-style: normal; font-weight: normal;'>
                    <a style='text-decoration: underline;' href='{url-web}'>
                      <img width='123' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 123px;' alt='CBA147' src='{url-imagen-cba147}'>
                    </a>
                  </div>

                  <div style='margin-top: 20px; margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Comprobante de&nbsp;</strong><strong>atención</strong></h1>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 20px; margin-bottom: 0px;'><span style='color: rgb(0, 166, 101);'>{numero-requerimiento}&nbsp;</span></h1>
                      <p style='margin-top: 20px; margin-bottom: 0px;'>¡Hola {nombre-usuario}!</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'>
                        Registramos con éxito tu requerimiento&nbsp;con número&nbsp;<span style='color: rgb(0, 166, 101);'><strong>{numero-requerimiento}</strong></span>&nbsp;y su comprobante va adjunto en este e-mail.&nbsp;
                        <br> Te mantendremos informado de los avances del requerimiento.
                      </p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 20px;'>Recordá que podés conocer el estado de tus requerimientos a través de la aplicación móvil o web.&nbsp; Tu nombre de usuario es&nbsp;<span style='color: rgb(0, 166, 101);'><strong>{username-usuario}</strong></span>.</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 10px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='text-align: center; margin-right: 20px; margin-left: 20px;'>
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-android}'>Aplicación Android</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-android}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación Android</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-ios}'>Aplicación iOS</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-ios}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación iOS</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: rgb(0, 166, 101);' href='{url-web}/MisRequerimientos'>Ir a la Web</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-web}' style='width:200px' arcsize='9%' fillcolor='#00A665' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Ir a la Web</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 10px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 0px;'>Muchas gracias.</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'>
                        <strong>#CBA147</strong> Atención al ciudadano
                        <br> Municipalidad de Córdoba
                      </p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 60px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-bottom: 24px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p class='size-14' lang='x-size-14' style='line-height: 21px; font-size: 14px; margin-top: 0px; margin-bottom: 0px;'>
                        Este e-mail ha sigo generado de forma automática a través del sistema #CBA147. Por favor, no responda este mensaje.
                        <br> {fecha}
                      </p>
                    </div>
                  </div>

                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>

            <div style='line-height: 10px; font-size: 10px; mso-line-height-rule: exactly;'>&nbsp;</div>

            <div role='contentinfo' style='mso-line-height-rule: exactly;'>
              <div class='layout email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]-->
                  <div class='column wide' style='width: calc(8000% - 47600px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Ubuntu,sans-serif; font-size: 12px; float: left; min-width: 320px; max-width: 400px;'>
                    <div style='margin: 10px 20px;'>
                      <table class='email-footer__links emb-web-links' role='presentation' style='border-collapse: collapse; table-layout: fixed;'>
                        <tbody>
                          <tr role='navigation'>
                            <td class='emb-web-links' style='padding: 0px; width: 26px;'>
                              <a style='transition: opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-facebook}'>
                                <img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Facebook' src='http://i2.cmail20.com/static/eb/master/13-the-blueprint-3/images/facebook.png'>
                              </a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition: opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-twitter}'>
                                <img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Twitter' src='http://i3.cmail20.com/static/eb/master/13-the-blueprint-3/images/twitter.png'>
                              </a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition: opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-instagram}'>
                                <img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Instagram' src='http://i5.cmail20.com/static/eb/master/13-the-blueprint-3/images/instagram.png'>
                              </a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition: opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-youtube}'>
                                <img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='YouTube' src='http://i4.cmail20.com/static/eb/master/13-the-blueprint-3/images/youtube.png'>
                              </a>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 20px;'>
                        <div>&nbsp; &nbsp;</div>
                      </div>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 18px;'>
                      </div>
                      <!--[if mso]>&nbsp;<![endif]-->
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]-->
                  <div class='column narrow' style='width: calc(72200px - 12000%); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Ubuntu,sans-serif; font-size: 12px; float: left; min-width: 200px; max-width: 320px;'>
                    <div style='margin: 10px 20px;'>
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
              <div class='layout one-col email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 600px;' class='w560'><![endif]-->
                  <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Ubuntu,sans-serif; font-size: 12px; min-width: 320px; max-width: 600px;'>
                    <div style='margin: 10px 20px;'>
                      <div style='line-height: 19px; font-size: 12px;'>
                      </div>
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
            </div>
            <div style='line-height: 40px; font-size: 40px; mso-line-height-rule: exactly;'>&nbsp;</div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
  <img width='1' height='1' style='margin: 0px !important; padding: 0px !important; border: 0px currentColor !important; border-image: none !important; width: 1px !important; height: 1px !important; overflow: hidden; display: block !important; visibility: hidden !important; position: fixed;' alt='' src='https://amura.cmail20.com/t/j-o-ozuun-l/o.gif' border='0'>
</body>
</html>
";
        }

        private string GetHtmlMensaje(string numeroRequerimiento, string nombreUsuario, string usernameUsuario, string contenidoMensaje)
        {
            string html = GetHtmlPuroMensaje();
            html = ReemplazarDatosBasicosEnHTML(html);

            html = html.Replace("{numero-requerimiento}", numeroRequerimiento);
            html = html.Replace("{nombre-usuario}", nombreUsuario);
            html = html.Replace("{username-usuario}", usernameUsuario);
            html = html.Replace("{contenido-mensaje}", contenidoMensaje);
            return html;
        }
        private string GetHtmlPuroMensaje()
        {
            return @"
<html style='margin: 0px; padding: 0px;' xmlns='http://www.w3.org/1999/xhtml'>
<head>
  <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
  <!--[if !mso]><!-->
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <!--<![endif]-->
  <meta name='viewport' content='width=device-width'>
  <style type='text/css'>
    @media only screen and (min-width: 620px) {
      .wrapper {
        min-width: 600px !important
      }
      .wrapper h1 {}
      .wrapper h1 {
        font-size: 36px !important;
        line-height: 43px !important
      }
      .wrapper h2 {}
      .wrapper h2 {
        font-size: 22px !important;
        line-height: 31px !important
      }
      .wrapper h3 {}
      .wrapper h3 {
        font-size: 18px !important;
        line-height: 26px !important
      }
      .column {}
      .wrapper .size-8 {
        font-size: 8px !important;
        line-height: 14px !important
      }
      .wrapper .size-9 {
        font-size: 9px !important;
        line-height: 16px !important
      }
      .wrapper .size-10 {
        font-size: 10px !important;
        line-height: 18px !important
      }
      .wrapper .size-11 {
        font-size: 11px !important;
        line-height: 19px !important
      }
      .wrapper .size-12 {
        font-size: 12px !important;
        line-height: 19px !important
      }
      .wrapper .size-13 {
        font-size: 13px !important;
        line-height: 21px !important
      }
      .wrapper .size-14 {
        font-size: 14px !important;
        line-height: 21px !important
      }
      .wrapper .size-15 {
        font-size: 15px !important;
        line-height: 23px !important
      }
      .wrapper .size-16 {
        font-size: 16px !important;
        line-height: 24px !important
      }
      .wrapper .size-17 {
        font-size: 17px !important;
        line-height: 26px !important
      }
      .wrapper .size-18 {
        font-size: 18px !important;
        line-height: 26px !important
      }
      .wrapper .size-20 {
        font-size: 20px !important;
        line-height: 28px !important
      }
      .wrapper .size-22 {
        font-size: 22px !important;
        line-height: 31px !important
      }
      .wrapper .size-24 {
        font-size: 24px !important;
        line-height: 32px !important
      }
      .wrapper .size-26 {
        font-size: 26px !important;
        line-height: 34px !important
      }
      .wrapper .size-28 {
        font-size: 28px !important;
        line-height: 36px !important
      }
      .wrapper .size-30 {
        font-size: 30px !important;
        line-height: 38px !important
      }
      .wrapper .size-32 {
        font-size: 32px !important;
        line-height: 40px !important
      }
      .wrapper .size-34 {
        font-size: 34px !important;
        line-height: 43px !important
      }
      .wrapper .size-36 {
        font-size: 36px !important;
        line-height: 43px !important
      }
      .wrapper .size-40 {
        font-size: 40px !important;
        line-height: 47px !important
      }
      .wrapper .size-44 {
        font-size: 44px !important;
        line-height: 50px !important
      }
      .wrapper .size-48 {
        font-size: 48px !important;
        line-height: 54px !important
      }
      .wrapper .size-56 {
        font-size: 56px !important;
        line-height: 60px !important
      }
      .wrapper .size-64 {
        font-size: 64px !important;
        line-height: 63px !important
      }
    }
  </style>
  <style type='text/css'>
    body {
      margin: 0;
      padding: 0;
    }
    
    table {
      border-collapse: collapse;
      table-layout: fixed;
    }
    
    * {
      line-height: inherit;
    }
    
    [x-apple-data-detectors],
    [href^='tel'],
    [href^='sms'] {
      color: inherit !important;
      text-decoration: none !important;
    }
    
    .wrapper .footer__share-button a:hover,
    .wrapper .footer__share-button a:focus {
      color: #ffffff !important;
    }
    
    .btn a:hover,
    .btn a:focus,
    .footer__share-button a:hover,
    .footer__share-button a:focus,
    .email-footer__links a:hover,
    .email-footer__links a:focus {
      opacity: 0.8;
    }
    
    .preheader,
    .header,
    .layout,
    .column {
      transition: width 0.25s ease-in-out, max-width 0.25s ease-in-out;
    }
    
    .preheader td {
      padding-bottom: 8px;
    }
    
    .layout,
    div.header {
      max-width: 400px !important;
      -fallback-width: 95% !important;
      width: calc(100% - 20px) !important;
    }
    
    div.preheader {
      max-width: 360px !important;
      -fallback-width: 90% !important;
      width: calc(100% - 60px) !important;
    }
    
    .snippet,
    .webversion {
      Float: none !important;
    }
    
    .column {
      max-width: 400px !important;
      width: 100% !important;
    }
    
    .fixed-width.has-border {
      max-width: 402px !important;
    }
    
    .fixed-width.has-border .layout__inner {
      box-sizing: border-box;
    }
    
    .snippet,
    .webversion {
      width: 50% !important;
    }
    
    .ie .btn {
      width: 100%;
    }
    
    [owa] .column div,
    [owa] .column button {
      display: block !important;
    }
    
    .ie .column,
    [owa] .column,
    .ie .gutter,
    [owa] .gutter {
      display: table-cell;
      float: none !important;
      vertical-align: top;
    }
    
    .ie div.preheader,
    [owa] div.preheader,
    .ie .email-footer,
    [owa] .email-footer {
      max-width: 560px !important;
      width: 560px !important;
    }
    
    .ie .snippet,
    [owa] .snippet,
    .ie .webversion,
    [owa] .webversion {
      width: 280px !important;
    }
    
    .ie div.header,
    [owa] div.header,
    .ie .layout,
    [owa] .layout,
    .ie .one-col .column,
    [owa] .one-col .column {
      max-width: 600px !important;
      width: 600px !important;
    }
    
    .ie .fixed-width.has-border,
    [owa] .fixed-width.has-border,
    .ie .has-gutter.has-border,
    [owa] .has-gutter.has-border {
      max-width: 602px !important;
      width: 602px !important;
    }
    
    .ie .two-col .column,
    [owa] .two-col .column {
      max-width: 300px !important;
      width: 300px !important;
    }
    
    .ie .three-col .column,
    [owa] .three-col .column,
    .ie .narrow,
    [owa] .narrow {
      max-width: 200px !important;
      width: 200px !important;
    }
    
    .ie .wide,
    [owa] .wide {
      width: 400px !important;
    }
    
    .ie .two-col.has-gutter .column,
    [owa] .two-col.x_has-gutter .column {
      max-width: 290px !important;
      width: 290px !important;
    }
    
    .ie .three-col.has-gutter .column,
    [owa] .three-col.x_has-gutter .column,
    .ie .has-gutter .narrow,
    [owa] .has-gutter .narrow {
      max-width: 188px !important;
      width: 188px !important;
    }
    
    .ie .has-gutter .wide,
    [owa] .has-gutter .wide {
      max-width: 394px !important;
      width: 394px !important;
    }
    
    .ie .two-col.has-gutter.has-border .column,
    [owa] .two-col.x_has-gutter.x_has-border .column {
      max-width: 292px !important;
      width: 292px !important;
    }
    
    .ie .three-col.has-gutter.has-border .column,
    [owa] .three-col.x_has-gutter.x_has-border .column,
    .ie .has-gutter.has-border .narrow,
    [owa] .has-gutter.x_has-border .narrow {
      max-width: 190px !important;
      width: 190px !important;
    }
    
    .ie .has-gutter.has-border .wide,
    [owa] .has-gutter.x_has-border .wide {
      max-width: 396px !important;
      width: 396px !important;
    }
    
    .ie .fixed-width .layout__inner {
      border-left: 0 none white !important;
      border-right: 0 none white !important;
    }
    
    .ie .layout__edges {
      display: none;
    }
    
    .mso .layout__edges {
      font-size: 0;
    }
    
    .layout-fixed-width,
    .mso .layout-full-width {
      background-color: #ffffff;
    }
    
    @media only screen and (min-width: 620px) {
      .column,
      .gutter {
        display: table-cell;
        Float: none !important;
        vertical-align: top;
      }
      div.preheader,
      .email-footer {
        max-width: 560px !important;
        width: 560px !important;
      }
      .snippet,
      .webversion {
        width: 280px !important;
      }
      div.header,
      .layout,
      .one-col .column {
        max-width: 600px !important;
        width: 600px !important;
      }
      .fixed-width.has-border,
      .fixed-width.ecxhas-border,
      .has-gutter.has-border,
      .has-gutter.ecxhas-border {
        max-width: 602px !important;
        width: 602px !important;
      }
      .two-col .column {
        max-width: 300px !important;
        width: 300px !important;
      }
      .three-col .column,
      .column.narrow {
        max-width: 200px !important;
        width: 200px !important;
      }
      .column.wide {
        width: 400px !important;
      }
      .two-col.has-gutter .column,
      .two-col.ecxhas-gutter .column {
        max-width: 290px !important;
        width: 290px !important;
      }
      .three-col.has-gutter .column,
      .three-col.ecxhas-gutter .column,
      .has-gutter .narrow {
        max-width: 188px !important;
        width: 188px !important;
      }
      .has-gutter .wide {
        max-width: 394px !important;
        width: 394px !important;
      }
      .two-col.has-gutter.has-border .column,
      .two-col.ecxhas-gutter.ecxhas-border .column {
        max-width: 292px !important;
        width: 292px !important;
      }
      .three-col.has-gutter.has-border .column,
      .three-col.ecxhas-gutter.ecxhas-border .column,
      .has-gutter.has-border .narrow,
      .has-gutter.ecxhas-border .narrow {
        max-width: 190px !important;
        width: 190px !important;
      }
      .has-gutter.has-border .wide,
      .has-gutter.ecxhas-border .wide {
        max-width: 396px !important;
        width: 396px !important;
      }
    }
    
    @media only screen and (-webkit-min-device-pixel-ratio: 2),
    only screen and (min--moz-device-pixel-ratio: 2),
    only screen and (-o-min-device-pixel-ratio: 2/1),
    only screen and (min-device-pixel-ratio: 2),
    only screen and (min-resolution: 192dpi),
    only screen and (min-resolution: 2dppx) {
      .fblike {
        background-image: url(http://i7.cmail20.com/static/eb/master/13-the-blueprint-3/images/fblike@2x.png) !important;
      }
      .tweet {
        background-image: url(http://i8.cmail20.com/static/eb/master/13-the-blueprint-3/images/tweet@2x.png) !important;
      }
      .linkedinshare {
        background-image: url(http://i10.cmail20.com/static/eb/master/13-the-blueprint-3/images/lishare@2x.png) !important;
      }
      .forwardtoafriend {
        background-image: url(http://i9.cmail20.com/static/eb/master/13-the-blueprint-3/images/forward@2x.png) !important;
      }
    }
    
    @media (max-width: 321px) {
      .fixed-width.has-border .layout__inner {
        border-width: 1px 0 !important;
      }
      .layout,
      .column {
        min-width: 320px !important;
        width: 320px !important;
      }
      .border {
        display: none;
      }
    }
    
    .mso div {
      border: 0 none white !important;
    }
    
    .mso .w560 .divider {
      Margin-left: 260px !important;
      Margin-right: 260px !important;
    }
    
    .mso .w360 .divider {
      Margin-left: 160px !important;
      Margin-right: 160px !important;
    }
    
    .mso .w260 .divider {
      Margin-left: 110px !important;
      Margin-right: 110px !important;
    }
    
    .mso .w160 .divider {
      Margin-left: 60px !important;
      Margin-right: 60px !important;
    }
    
    .mso .w354 .divider {
      Margin-left: 157px !important;
      Margin-right: 157px !important;
    }
    
    .mso .w250 .divider {
      Margin-left: 105px !important;
      Margin-right: 105px !important;
    }
    
    .mso .w148 .divider {
      Margin-left: 54px !important;
      Margin-right: 54px !important;
    }
    
    .mso .size-8,
    .ie .size-8 {
      font-size: 8px !important;
      line-height: 14px !important;
    }
    
    .mso .size-9,
    .ie .size-9 {
      font-size: 9px !important;
      line-height: 16px !important;
    }
    
    .mso .size-10,
    .ie .size-10 {
      font-size: 10px !important;
      line-height: 18px !important;
    }
    
    .mso .size-11,
    .ie .size-11 {
      font-size: 11px !important;
      line-height: 19px !important;
    }
    
    .mso .size-12,
    .ie .size-12 {
      font-size: 12px !important;
      line-height: 19px !important;
    }
    
    .mso .size-13,
    .ie .size-13 {
      font-size: 13px !important;
      line-height: 21px !important;
    }
    
    .mso .size-14,
    .ie .size-14 {
      font-size: 14px !important;
      line-height: 21px !important;
    }
    
    .mso .size-15,
    .ie .size-15 {
      font-size: 15px !important;
      line-height: 23px !important;
    }
    
    .mso .size-16,
    .ie .size-16 {
      font-size: 16px !important;
      line-height: 24px !important;
    }
    
    .mso .size-17,
    .ie .size-17 {
      font-size: 17px !important;
      line-height: 26px !important;
    }
    
    .mso .size-18,
    .ie .size-18 {
      font-size: 18px !important;
      line-height: 26px !important;
    }
    
    .mso .size-20,
    .ie .size-20 {
      font-size: 20px !important;
      line-height: 28px !important;
    }
    
    .mso .size-22,
    .ie .size-22 {
      font-size: 22px !important;
      line-height: 31px !important;
    }
    
    .mso .size-24,
    .ie .size-24 {
      font-size: 24px !important;
      line-height: 32px !important;
    }
    
    .mso .size-26,
    .ie .size-26 {
      font-size: 26px !important;
      line-height: 34px !important;
    }
    
    .mso .size-28,
    .ie .size-28 {
      font-size: 28px !important;
      line-height: 36px !important;
    }
    
    .mso .size-30,
    .ie .size-30 {
      font-size: 30px !important;
      line-height: 38px !important;
    }
    
    .mso .size-32,
    .ie .size-32 {
      font-size: 32px !important;
      line-height: 40px !important;
    }
    
    .mso .size-34,
    .ie .size-34 {
      font-size: 34px !important;
      line-height: 43px !important;
    }
    
    .mso .size-36,
    .ie .size-36 {
      font-size: 36px !important;
      line-height: 43px !important;
    }
    
    .mso .size-40,
    .ie .size-40 {
      font-size: 40px !important;
      line-height: 47px !important;
    }
    
    .mso .size-44,
    .ie .size-44 {
      font-size: 44px !important;
      line-height: 50px !important;
    }
    
    .mso .size-48,
    .ie .size-48 {
      font-size: 48px !important;
      line-height: 54px !important;
    }
    
    .mso .size-56,
    .ie .size-56 {
      font-size: 56px !important;
      line-height: 60px !important;
    }
    
    .mso .size-64,
    .ie .size-64 {
      font-size: 64px !important;
      line-height: 63px !important;
    }
  </style>

  <!--[if !mso]><!-->
  <style type='text/css'>
    @import url(https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic);
  </style>
  <link href='https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
  <!--<![endif]-->
  
  <style type='text/css'>
    body {
      background-color: #00a665
    }
    
    .logo a:hover,
    .logo a:focus {
      color: #fff !important
    }
    
    .mso .layout-has-border {
      border-top: 1px solid #004027;
      border-bottom: 1px solid #004027
    }
    
    .mso .layout-has-bottom-border {
      border-bottom: 1px solid #004027
    }
    
    .mso .border,
    .ie .border {
      background-color: #004027
    }
    
    .mso h1,
    .ie h1 {}
    
    .mso h1,
    .ie h1 {
      font-size: 36px !important;
      line-height: 43px !important
    }
    
    .mso h2,
    .ie h2 {}
    
    .mso h2,
    .ie h2 {
      font-size: 22px !important;
      line-height: 31px !important
    }
    
    .mso h3,
    .ie h3 {}
    
    .mso h3,
    .ie h3 {
      font-size: 18px !important;
      line-height: 26px !important
    }
    
    .mso .layout__inner,
    .ie .layout__inner {}
    
    .mso .footer__share-button p {}
    
    .mso .footer__share-button p {
      font-family: Lato, Tahoma, sans-serif
    }
  </style>
</head>

<!--[if !mso]-->
<body class='full-padding' style='margin: 0px; padding: 0px; -webkit-text-size-adjust: 100%;'>
<!--<![endif]-->
<!--[if mso]>
<body class='mso'>
<![endif]-->
  <table class='wrapper' role='presentation' style='width: 100%; border-collapse: collapse; table-layout: fixed; min-width: 320px; background-color: rgb(0, 166, 101);' cellspacing='0' cellpadding='0'>
    <tbody>
      <tr>
        <td>
          <div role='banner'>
            <div class='preheader' style='margin: 0px auto; width: calc(28000% - 167440px); min-width: 280px; max-width: 560px;'>
              <div style='width: 100%; display: table; border-collapse: collapse;'>
                <!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 280px' valign='top'><![endif]-->
                <div class='snippet' style='padding: 10px 0px 5px; width: calc(14000% - 78120px); color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 140px; max-width: 280px;'>
                </div>
                <!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]-->
                <div class='webversion' style='padding: 10px 0px 5px; width: calc(14100% - 78680px); text-align: right; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 139px; max-width: 280px;'>
                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>
            <div class='header' id='emb-email-header-container' style='margin: 0px auto; width: calc(28000% - 167400px); min-width: 320px; max-width: 600px;'>
              <!--[if (mso)|(IE)]><table align='center' class='header' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 600px'><![endif]-->
              <div align='center' class='logo emb-logo-margin-box' style='margin: 6px 20px 20px; color: rgb(195, 206, 217); line-height: 32px; font-family: Roboto,Tahoma,sans-serif; font-size: 26px;'>
                <div align='center' class='logo-center' id='emb-email-header'>
                  <a style='text-decoration: underline;' href='{url-muni}'>
                    <img width='243' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 243px;' alt='Municipalidad de Cordoba' src='{url-imagen-muni}'>
                  </a>
                </div>
              </div>
              <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
            </div>
          </div>
          <div role='section'>
            <div class='layout one-col fixed-width' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
              <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse; background-color: rgb(255, 255, 255);  border-radius: 8px;' emb-background-style=''>
                <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-fixed-width' emb-background-style><td style='width: 600px' class='w560'><![endif]-->
                <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(120, 119, 120); line-height: 24px; font-family: Lato,Tahoma,sans-serif; font-size: 16px; min-width: 320px; max-width: 600px;'>

                  <div align='center' style='line-height: 19px; font-size: 12px; font-style: normal; font-weight: normal;'>
                    <a style='text-decoration: underline;' href='{url-web}'>
                      <img width='123' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 123px;' alt='CBA147' src='{url-imagen-cba147}'>
                    </a>
                  </div>

                  <div style='margin-top: 20px; margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Mensaje recibido</strong></h1>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 20px; margin-bottom: 0px;'><span style='color: rgb(0, 166, 101);'>{numero-requerimiento}</span><span style='color: rgb(56, 209, 126);'>&nbsp;</span></h1>
                      <p style='margin-top: 20px; margin-bottom: 0px;'>¡Hola {nombre-usuario}!</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'>Uno de nuestros colaboradores te envió un mensaje asociado a tu requerimiento<span style='color: rgb(0, 166, 101);'>&nbsp;<strong>{numero-requerimiento}</strong></span>.</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <blockquote style='margin: 0px 0px 20px; padding-left: 14px; border-left-color: rgb(0, 64, 39); border-left-width: 4px; border-left-style: solid;'>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 12px;'>{contenido-mensaje}</h3></blockquote>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 20px;'>Recordá que podés conocer el estado de tus requerimientos a través de la aplicación móvil o web.&nbsp; Tu nombre de usuario es&nbsp;<span style='color: rgb(0, 166, 101);'><strong>{username-usuario}</strong></span>.</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 10px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>
                  
                  <div style='text-align: center; margin-right: 20px; margin-left: 20px;'>
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-android}'>Aplicación Android</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-android}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación Android</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-ios}'>Aplicación iOS</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-ios}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación iOS</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: rgb(0, 166, 101);' href='{url-web}/MisRequerimientos'>Ir a la Web</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-web}' style='width:200px' arcsize='9%' fillcolor='#00A665' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Ir a la Web</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 30px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 0px;'>Muchas gracias.</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'><strong>#CBA147</strong> Atención al ciudadano
                        <br> Municipalidad de Córdoba</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 60px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-bottom: 24px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p class='size-14' lang='x-size-14' style='line-height: 21px; font-size: 14px; margin-top: 0px; margin-bottom: 0px;'>Este e-mail ha sigo generado de forma automática a través del sistema #CBA147. Por favor, no responda este mensaje.
                        <br> {fecha}
                      </p>
                    </div>
                  </div>

                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>

            <div style='line-height: 10px; font-size: 10px; mso-line-height-rule: exactly;'>&nbsp;</div>

            <div role='contentinfo' style='mso-line-height-rule: exactly;'>
              <div class='layout email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]-->
                  <div class='column wide' style='width: calc(8000% - 47600px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; min-width: 320px; max-width: 400px;'>
                    <div style='margin: 10px 20px;'>
                      <table class='email-footer__links emb-web-links' role='presentation' style='border-collapse: collapse; table-layout: fixed;'>
                        <tbody>
                          <tr role='navigation'>
                            <td class='emb-web-links' style='padding: 0px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-facebook}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Facebook' src='http://i2.cmail20.com/static/eb/master/13-the-blueprint-3/images/facebook.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-twitter}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Twitter' src='http://i3.cmail20.com/static/eb/master/13-the-blueprint-3/images/twitter.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-youtube}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='YouTube' src='http://i4.cmail20.com/static/eb/master/13-the-blueprint-3/images/youtube.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-instagram}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Instagram' src='http://i5.cmail20.com/static/eb/master/13-the-blueprint-3/images/instagram.png'></a>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 20px;'>
                        <div>&nbsp; &nbsp;</div>
                      </div>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 18px;'>

                      </div>
                      <!--[if mso]>&nbsp;<![endif]-->
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]-->
                  <div class='column narrow' style='width: calc(72200px - 12000%); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; min-width: 200px; max-width: 320px;'>
                    <div style='margin: 10px 20px;'>

                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
              <div class='layout one-col email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 600px;' class='w560'><![endif]-->
                  <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; min-width: 320px; max-width: 600px;'>
                    <div style='margin: 10px 20px;'>
                      <div style='line-height: 19px; font-size: 12px;'>
                      </div>
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
            </div>
            <div style='line-height: 40px; font-size: 40px; mso-line-height-rule: exactly;'>&nbsp;</div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
  <img width='1' height='1' style='margin: 0px !important; padding: 0px !important; border: 0px currentColor !important; border-image: none !important; width: 1px !important; height: 1px !important; overflow: hidden; display: block !important; visibility: hidden !important; position: fixed;' alt='' src='https://amura.cmail20.com/t/j-o-ovlkc-l/o.gif' border='0'>
</body>
</html>
";
        }

        private string GetHtmlCambioEstado(string numeroRequerimiento, string nombreUsuario, string usernameUsuario, string fechaEstado, string nombreEstado, string observacionesEstado)
        {
            var html = GetHtmlPuroCambioEstado();
            html = ReemplazarDatosBasicosEnHTML(html);

            html = html.Replace("{numero-requerimiento}", numeroRequerimiento);
            html = html.Replace("{nombre-usuario}", nombreUsuario);
            html = html.Replace("{username-usuario}", usernameUsuario);
            html = html.Replace("{fecha-estado}", fechaEstado);
            html = html.Replace("{nombre-estado}", nombreEstado);
            html = html.Replace("{observaciones-estado}", observacionesEstado);
            return html;
        }
        private string GetHtmlPuroCambioEstado()
        {
            return @"
<html style='margin: 0px; padding: 0px;' xmlns='http://www.w3.org/1999/xhtml'>
<head>
  <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
  <!--[if !mso]><!-->
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <!--<![endif]-->
  <meta name='viewport' content='width=device-width'>
  <style type='text/css'>
    @media only screen and (min-width: 620px) {
      .wrapper {
        min-width: 600px !important
      }
      .wrapper h1 {}
      .wrapper h1 {
        font-size: 36px !important;
        line-height: 43px !important
      }
      .wrapper h2 {}
      .wrapper h2 {
        font-size: 22px !important;
        line-height: 31px !important
      }
      .wrapper h3 {}
      .wrapper h3 {
        font-size: 18px !important;
        line-height: 26px !important
      }
      .column {}
      .wrapper .size-8 {
        font-size: 8px !important;
        line-height: 14px !important
      }
      .wrapper .size-9 {
        font-size: 9px !important;
        line-height: 16px !important
      }
      .wrapper .size-10 {
        font-size: 10px !important;
        line-height: 18px !important
      }
      .wrapper .size-11 {
        font-size: 11px !important;
        line-height: 19px !important
      }
      .wrapper .size-12 {
        font-size: 12px !important;
        line-height: 19px !important
      }
      .wrapper .size-13 {
        font-size: 13px !important;
        line-height: 21px !important
      }
      .wrapper .size-14 {
        font-size: 14px !important;
        line-height: 21px !important
      }
      .wrapper .size-15 {
        font-size: 15px !important;
        line-height: 23px !important
      }
      .wrapper .size-16 {
        font-size: 16px !important;
        line-height: 24px !important
      }
      .wrapper .size-17 {
        font-size: 17px !important;
        line-height: 26px !important
      }
      .wrapper .size-18 {
        font-size: 18px !important;
        line-height: 26px !important
      }
      .wrapper .size-20 {
        font-size: 20px !important;
        line-height: 28px !important
      }
      .wrapper .size-22 {
        font-size: 22px !important;
        line-height: 31px !important
      }
      .wrapper .size-24 {
        font-size: 24px !important;
        line-height: 32px !important
      }
      .wrapper .size-26 {
        font-size: 26px !important;
        line-height: 34px !important
      }
      .wrapper .size-28 {
        font-size: 28px !important;
        line-height: 36px !important
      }
      .wrapper .size-30 {
        font-size: 30px !important;
        line-height: 38px !important
      }
      .wrapper .size-32 {
        font-size: 32px !important;
        line-height: 40px !important
      }
      .wrapper .size-34 {
        font-size: 34px !important;
        line-height: 43px !important
      }
      .wrapper .size-36 {
        font-size: 36px !important;
        line-height: 43px !important
      }
      .wrapper .size-40 {
        font-size: 40px !important;
        line-height: 47px !important
      }
      .wrapper .size-44 {
        font-size: 44px !important;
        line-height: 50px !important
      }
      .wrapper .size-48 {
        font-size: 48px !important;
        line-height: 54px !important
      }
      .wrapper .size-56 {
        font-size: 56px !important;
        line-height: 60px !important
      }
      .wrapper .size-64 {
        font-size: 64px !important;
        line-height: 63px !important
      }
    }
  </style>
  <style type='text/css'>
    body {
      margin: 0;
      padding: 0;
    }
    
    table {
      border-collapse: collapse;
      table-layout: fixed;
    }
    
    * {
      line-height: inherit;
    }
    
    [x-apple-data-detectors],
    [href^='tel'],
    [href^='sms'] {
      color: inherit !important;
      text-decoration: none !important;
    }
    
    .wrapper .footer__share-button a:hover,
    .wrapper .footer__share-button a:focus {
      color: #ffffff !important;
    }
    
    .btn a:hover,
    .btn a:focus,
    .footer__share-button a:hover,
    .footer__share-button a:focus,
    .email-footer__links a:hover,
    .email-footer__links a:focus {
      opacity: 0.8;
    }
    
    .preheader,
    .header,
    .layout,
    .column {
      transition: width 0.25s ease-in-out, max-width 0.25s ease-in-out;
    }
    
    .preheader td {
      padding-bottom: 8px;
    }
    
    .layout,
    div.header {
      max-width: 400px !important;
      -fallback-width: 95% !important;
      width: calc(100% - 20px) !important;
    }
    
    div.preheader {
      max-width: 360px !important;
      -fallback-width: 90% !important;
      width: calc(100% - 60px) !important;
    }
    
    .snippet,
    .webversion {
      Float: none !important;
    }
    
    .column {
      max-width: 400px !important;
      width: 100% !important;
    }
    
    .fixed-width.has-border {
      max-width: 402px !important;
    }
    
    .fixed-width.has-border .layout__inner {
      box-sizing: border-box;
    }
    
    .snippet,
    .webversion {
      width: 50% !important;
    }
    
    .ie .btn {
      width: 100%;
    }
    
    [owa] .column div,
    [owa] .column button {
      display: block !important;
    }
    
    .ie .column,
    [owa] .column,
    .ie .gutter,
    [owa] .gutter {
      display: table-cell;
      float: none !important;
      vertical-align: top;
    }
    
    .ie div.preheader,
    [owa] div.preheader,
    .ie .email-footer,
    [owa] .email-footer {
      max-width: 560px !important;
      width: 560px !important;
    }
    
    .ie .snippet,
    [owa] .snippet,
    .ie .webversion,
    [owa] .webversion {
      width: 280px !important;
    }
    
    .ie div.header,
    [owa] div.header,
    .ie .layout,
    [owa] .layout,
    .ie .one-col .column,
    [owa] .one-col .column {
      max-width: 600px !important;
      width: 600px !important;
    }
    
    .ie .fixed-width.has-border,
    [owa] .fixed-width.has-border,
    .ie .has-gutter.has-border,
    [owa] .has-gutter.has-border {
      max-width: 602px !important;
      width: 602px !important;
    }
    
    .ie .two-col .column,
    [owa] .two-col .column {
      max-width: 300px !important;
      width: 300px !important;
    }
    
    .ie .three-col .column,
    [owa] .three-col .column,
    .ie .narrow,
    [owa] .narrow {
      max-width: 200px !important;
      width: 200px !important;
    }
    
    .ie .wide,
    [owa] .wide {
      width: 400px !important;
    }
    
    .ie .two-col.has-gutter .column,
    [owa] .two-col.x_has-gutter .column {
      max-width: 290px !important;
      width: 290px !important;
    }
    
    .ie .three-col.has-gutter .column,
    [owa] .three-col.x_has-gutter .column,
    .ie .has-gutter .narrow,
    [owa] .has-gutter .narrow {
      max-width: 188px !important;
      width: 188px !important;
    }
    
    .ie .has-gutter .wide,
    [owa] .has-gutter .wide {
      max-width: 394px !important;
      width: 394px !important;
    }
    
    .ie .two-col.has-gutter.has-border .column,
    [owa] .two-col.x_has-gutter.x_has-border .column {
      max-width: 292px !important;
      width: 292px !important;
    }
    
    .ie .three-col.has-gutter.has-border .column,
    [owa] .three-col.x_has-gutter.x_has-border .column,
    .ie .has-gutter.has-border .narrow,
    [owa] .has-gutter.x_has-border .narrow {
      max-width: 190px !important;
      width: 190px !important;
    }
    
    .ie .has-gutter.has-border .wide,
    [owa] .has-gutter.x_has-border .wide {
      max-width: 396px !important;
      width: 396px !important;
    }
    
    .ie .fixed-width .layout__inner {
      border-left: 0 none white !important;
      border-right: 0 none white !important;
    }
    
    .ie .layout__edges {
      display: none;
    }
    
    .mso .layout__edges {
      font-size: 0;
    }
    
    .layout-fixed-width,
    .mso .layout-full-width {
      background-color: #ffffff;
    }
    
    @media only screen and (min-width: 620px) {
      .column,
      .gutter {
        display: table-cell;
        Float: none !important;
        vertical-align: top;
      }
      div.preheader,
      .email-footer {
        max-width: 560px !important;
        width: 560px !important;
      }
      .snippet,
      .webversion {
        width: 280px !important;
      }
      div.header,
      .layout,
      .one-col .column {
        max-width: 600px !important;
        width: 600px !important;
      }
      .fixed-width.has-border,
      .fixed-width.ecxhas-border,
      .has-gutter.has-border,
      .has-gutter.ecxhas-border {
        max-width: 602px !important;
        width: 602px !important;
      }
      .two-col .column {
        max-width: 300px !important;
        width: 300px !important;
      }
      .three-col .column,
      .column.narrow {
        max-width: 200px !important;
        width: 200px !important;
      }
      .column.wide {
        width: 400px !important;
      }
      .two-col.has-gutter .column,
      .two-col.ecxhas-gutter .column {
        max-width: 290px !important;
        width: 290px !important;
      }
      .three-col.has-gutter .column,
      .three-col.ecxhas-gutter .column,
      .has-gutter .narrow {
        max-width: 188px !important;
        width: 188px !important;
      }
      .has-gutter .wide {
        max-width: 394px !important;
        width: 394px !important;
      }
      .two-col.has-gutter.has-border .column,
      .two-col.ecxhas-gutter.ecxhas-border .column {
        max-width: 292px !important;
        width: 292px !important;
      }
      .three-col.has-gutter.has-border .column,
      .three-col.ecxhas-gutter.ecxhas-border .column,
      .has-gutter.has-border .narrow,
      .has-gutter.ecxhas-border .narrow {
        max-width: 190px !important;
        width: 190px !important;
      }
      .has-gutter.has-border .wide,
      .has-gutter.ecxhas-border .wide {
        max-width: 396px !important;
        width: 396px !important;
      }
    }
    
    @media only screen and (-webkit-min-device-pixel-ratio: 2),
    only screen and (min--moz-device-pixel-ratio: 2),
    only screen and (-o-min-device-pixel-ratio: 2/1),
    only screen and (min-device-pixel-ratio: 2),
    only screen and (min-resolution: 192dpi),
    only screen and (min-resolution: 2dppx) {
      .fblike {
        background-image: url(http://i7.cmail19.com/static/eb/master/13-the-blueprint-3/images/fblike@2x.png) !important;
      }
      .tweet {
        background-image: url(http://i8.cmail19.com/static/eb/master/13-the-blueprint-3/images/tweet@2x.png) !important;
      }
      .linkedinshare {
        background-image: url(http://i10.cmail19.com/static/eb/master/13-the-blueprint-3/images/lishare@2x.png) !important;
      }
      .forwardtoafriend {
        background-image: url(http://i9.cmail19.com/static/eb/master/13-the-blueprint-3/images/forward@2x.png) !important;
      }
    }
    
    @media (max-width: 321px) {
      .fixed-width.has-border .layout__inner {
        border-width: 1px 0 !important;
      }
      .layout,
      .column {
        min-width: 320px !important;
        width: 320px !important;
      }
      .border {
        display: none;
      }
    }
    
    .mso div {
      border: 0 none white !important;
    }
    
    .mso .w560 .divider {
      Margin-left: 260px !important;
      Margin-right: 260px !important;
    }
    
    .mso .w360 .divider {
      Margin-left: 160px !important;
      Margin-right: 160px !important;
    }
    
    .mso .w260 .divider {
      Margin-left: 110px !important;
      Margin-right: 110px !important;
    }
    
    .mso .w160 .divider {
      Margin-left: 60px !important;
      Margin-right: 60px !important;
    }
    
    .mso .w354 .divider {
      Margin-left: 157px !important;
      Margin-right: 157px !important;
    }
    
    .mso .w250 .divider {
      Margin-left: 105px !important;
      Margin-right: 105px !important;
    }
    
    .mso .w148 .divider {
      Margin-left: 54px !important;
      Margin-right: 54px !important;
    }
    
    .mso .size-8,
    .ie .size-8 {
      font-size: 8px !important;
      line-height: 14px !important;
    }
    
    .mso .size-9,
    .ie .size-9 {
      font-size: 9px !important;
      line-height: 16px !important;
    }
    
    .mso .size-10,
    .ie .size-10 {
      font-size: 10px !important;
      line-height: 18px !important;
    }
    
    .mso .size-11,
    .ie .size-11 {
      font-size: 11px !important;
      line-height: 19px !important;
    }
    
    .mso .size-12,
    .ie .size-12 {
      font-size: 12px !important;
      line-height: 19px !important;
    }
    
    .mso .size-13,
    .ie .size-13 {
      font-size: 13px !important;
      line-height: 21px !important;
    }
    
    .mso .size-14,
    .ie .size-14 {
      font-size: 14px !important;
      line-height: 21px !important;
    }
    
    .mso .size-15,
    .ie .size-15 {
      font-size: 15px !important;
      line-height: 23px !important;
    }
    
    .mso .size-16,
    .ie .size-16 {
      font-size: 16px !important;
      line-height: 24px !important;
    }
    
    .mso .size-17,
    .ie .size-17 {
      font-size: 17px !important;
      line-height: 26px !important;
    }
    
    .mso .size-18,
    .ie .size-18 {
      font-size: 18px !important;
      line-height: 26px !important;
    }
    
    .mso .size-20,
    .ie .size-20 {
      font-size: 20px !important;
      line-height: 28px !important;
    }
    
    .mso .size-22,
    .ie .size-22 {
      font-size: 22px !important;
      line-height: 31px !important;
    }
    
    .mso .size-24,
    .ie .size-24 {
      font-size: 24px !important;
      line-height: 32px !important;
    }
    
    .mso .size-26,
    .ie .size-26 {
      font-size: 26px !important;
      line-height: 34px !important;
    }
    
    .mso .size-28,
    .ie .size-28 {
      font-size: 28px !important;
      line-height: 36px !important;
    }
    
    .mso .size-30,
    .ie .size-30 {
      font-size: 30px !important;
      line-height: 38px !important;
    }
    
    .mso .size-32,
    .ie .size-32 {
      font-size: 32px !important;
      line-height: 40px !important;
    }
    
    .mso .size-34,
    .ie .size-34 {
      font-size: 34px !important;
      line-height: 43px !important;
    }
    
    .mso .size-36,
    .ie .size-36 {
      font-size: 36px !important;
      line-height: 43px !important;
    }
    
    .mso .size-40,
    .ie .size-40 {
      font-size: 40px !important;
      line-height: 47px !important;
    }
    
    .mso .size-44,
    .ie .size-44 {
      font-size: 44px !important;
      line-height: 50px !important;
    }
    
    .mso .size-48,
    .ie .size-48 {
      font-size: 48px !important;
      line-height: 54px !important;
    }
    
    .mso .size-56,
    .ie .size-56 {
      font-size: 56px !important;
      line-height: 60px !important;
    }
    
    .mso .size-64,
    .ie .size-64 {
      font-size: 64px !important;
      line-height: 63px !important;
    }
  </style>

  <!--[if !mso]><!-->
  <style type='text/css'>
    @import url(https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic);
  </style>
  <link href='https://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
  <!--<![endif]-->
  
  <style type='text/css'>
    body {
      background-color: #00a665
    }
    
    .logo a:hover,
    .logo a:focus {
      color: #fff !important
    }
    
    .mso .layout-has-border {
      border-top: 1px solid #004027;
      border-bottom: 1px solid #004027
    }
    
    .mso .layout-has-bottom-border {
      border-bottom: 1px solid #004027
    }
    
    .mso .border,
    .ie .border {
      background-color: #004027
    }
    
    .mso h1,
    .ie h1 {}
    
    .mso h1,
    .ie h1 {
      font-size: 36px !important;
      line-height: 43px !important
    }
    
    .mso h2,
    .ie h2 {}
    
    .mso h2,
    .ie h2 {
      font-size: 22px !important;
      line-height: 31px !important
    }
    
    .mso h3,
    .ie h3 {}
    
    .mso h3,
    .ie h3 {
      font-size: 18px !important;
      line-height: 26px !important
    }
    
    .mso .layout__inner,
    .ie .layout__inner {}
    
    .mso .footer__share-button p {}
    
    .mso .footer__share-button p {
      font-family: Lato, Tahoma, sans-serif
    }
  </style>
</head>

<!--[if !mso]-->
<body class='full-padding' style='margin: 0px; padding: 0px; -webkit-text-size-adjust: 100%;'>
<!--<![endif]-->
<!--[if mso]>
<body class='mso'>
<![endif]-->
  <table class='wrapper' role='presentation' style='width: 100%; border-collapse: collapse; table-layout: fixed; min-width: 320px; background-color: rgb(0, 166, 101);' cellspacing='0' cellpadding='0'>
    <tbody>
      <tr>
        <td>
          <div role='banner'>
            <div class='preheader' style='margin: 0px auto; width: calc(28000% - 167440px); min-width: 280px; max-width: 560px;'>
              <div style='width: 100%; display: table; border-collapse: collapse;'>
                <!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 280px' valign='top'><![endif]-->
                <div class='snippet' style='padding: 10px 0px 5px; width: calc(14000% - 78120px); color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 140px; max-width: 280px;'>

                </div>
                <!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]-->
                <div class='webversion' style='padding: 10px 0px 5px; width: calc(14100% - 78680px); text-align: right; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; display: table-cell; min-width: 139px; max-width: 280px;'>

                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>
            <div class='header' id='emb-email-header-container' style='margin: 0px auto; width: calc(28000% - 167400px); min-width: 320px; max-width: 600px;'>
              <!--[if (mso)|(IE)]><table align='center' class='header' cellpadding='0' cellspacing='0' role='presentation'><tr><td style='width: 600px'><![endif]-->
              <div align='center' class='logo emb-logo-margin-box' style='margin: 6px 20px 20px; color: rgb(195, 206, 217); line-height: 32px; font-family: Roboto,Tahoma,sans-serif; font-size: 26px;'>
                <div align='center' class='logo-center' id='emb-email-header'>
                  <a style='text-decoration: underline;' href='{url-muni}'>
                    <img width='243' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 243px;' alt='Municipalidad de Cordoba' src='{url-imagen-muni}'>
                  </a>
                </div>
              </div>
              <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
            </div>
          </div>
          <div role='section'>
            <div class='layout one-col fixed-width' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
              <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse; background-color: rgb(255, 255, 255); border-radius: 8px;' emb-background-style=''>
                <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-fixed-width' emb-background-style><td style='width: 600px' class='w560'><![endif]-->
                <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(120, 119, 120); line-height: 24px; font-family: Lato,Tahoma,sans-serif; font-size: 16px; min-width: 320px; max-width: 600px;'>

                  <div align='center' style='line-height: 19px; font-size: 12px; font-style: normal; font-weight: normal;'>
                    <a style='text-decoration: underline;' href='{url-web}'>
                      <img width='123' style='border: 0px currentColor; border-image: none; width: 100%; height: auto; display: block; max-width: 123px;' alt='CBA147' src='{url-imagen-cba147}'>
                    </a>
                  </div>

                  <div style='margin-top: 20px; margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Mensaje recibido</strong></h1>
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 20px; margin-bottom: 0px;'><span style='color: rgb(0, 166, 101);'>{numero-requerimiento}&nbsp;</span></h1>
                      <p style='margin-top: 20px; margin-bottom: 0px;'>¡Hola {nombre-usuario}!</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'>Te informamos que tu requerimiento<span style='color: rgb(0, 166, 101);'> </span><strong><span style='color: rgb(0, 166, 101);'>{numero-requerimiento}</span>&nbsp;</strong>presenta las siguientes novedades:</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <blockquote style='margin: 0px 0px 20px; padding-left: 14px; border-left-color: rgb(0, 64, 39); border-left-width: 4px; border-left-style: solid;'>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Fecha:</strong> {fecha-estado}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 0px;'><strong>Estado:</strong>&nbsp; {nombre-estado}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 12px;'><strong>Observaciones:</strong> {observaciones-estado}</h3></blockquote>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 20px;'>Recordá que podés conocer el estado de tus requerimientos a través de la aplicación móvil o web.&nbsp; Tu nombre de usuario es&nbsp;<span style='color: rgb(0, 166, 101);'><strong>{username-usuario}</strong></span>.</p>
                    </div>
                  </div>
                  
                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 10px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>
                  
                  <div style='text-align: center; margin-right: 20px; margin-left: 20px;'>
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-android}'>Aplicación Android</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-android}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación Android</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px; display: inline-block !important;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: #212121;' href='{url-app-ios}'>Aplicación iOS</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-app-ios}' style='width:200px' arcsize='9%' fillcolor='#212121' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Aplicación iOS</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                    
                    <div class='btn btn--flat btn--large' style='margin: 10px;'>
                      <!--[if !mso]-->
                      <a style='width: 120px; padding: 12px 24px; border-radius: 4px; transition: opacity 0.1s ease-in; text-align: center; color: rgb(255, 255, 255) !important; line-height: 24px; font-family: Lato, Tahoma, sans-serif; font-size: 14px; font-weight: bold; text-decoration: none !important; display: inline-block; background-color: rgb(0, 166, 101);' href='{url-web}/MisRequerimientos'>Ir a la Web</a>
                      <!--[endif]-->
                      
                      <!--[if mso]>
                      <p style='line-height:0;margin:0;'>&nbsp;</p><v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' href='{url-web}' style='width:200px' arcsize='9%' fillcolor='#00A665' stroke='f'><v:textbox style='mso-fit-shape-to-text:t' inset='0px,11px,0px,11px'><center style='font-size:14px;line-height:24px;color:#FFFFFF;font-family:Lato,Tahoma,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:4px'>Ir a la Web</center></v:textbox></v:roundrect>
                      <![endif]-->
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 30px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 0px;'>Muchas gracias.</p>
                      <p style='margin-top: 20px; margin-bottom: 20px;'><strong>#CBA147</strong> Atención al ciudadano
                        <br> Municipalidad de Córdoba</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 60px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-bottom: 24px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p class='size-14' lang='x-size-14' style='line-height: 21px; font-size: 14px; margin-top: 0px; margin-bottom: 0px;'>Este e-mail ha sigo generado de forma automática a través del sistema #CBA147. Por favor, no responda este mensaje.
                        <br> {fecha}
                      </p>
                    </div>
                  </div>

                </div>
                <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
              </div>
            </div>

            <div style='line-height: 10px; font-size: 10px; mso-line-height-rule: exactly;'>&nbsp;</div>

            <div role='contentinfo' style='mso-line-height-rule: exactly;'>
              <div class='layout email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]-->
                  <div class='column wide' style='width: calc(8000% - 47600px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; min-width: 320px; max-width: 400px;'>
                    <div style='margin: 10px 20px;'>
                      <table class='email-footer__links emb-web-links' role='presentation' style='border-collapse: collapse; table-layout: fixed;'>
                        <tbody>
                          <tr role='navigation'>
                            <td class='emb-web-links' style='padding: 0px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-facebook}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Facebook' src='http://i2.cmail19.com/static/eb/master/13-the-blueprint-3/images/facebook.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-twitter}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Twitter' src='http://i3.cmail19.com/static/eb/master/13-the-blueprint-3/images/twitter.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-youtube}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='YouTube' src='http://i4.cmail19.com/static/eb/master/13-the-blueprint-3/images/youtube.png'></a>
                            </td>
                            <td class='emb-web-links' style='padding: 0px 0px 0px 3px; width: 26px;'>
                              <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='{url-instagram}'><img width='26' height='26' style='border: 0px currentColor; border-image: none;' alt='Instagram' src='http://i5.cmail19.com/static/eb/master/13-the-blueprint-3/images/instagram.png'></a>
                            </td>
                          </tr>
                        </tbody>
                      </table>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 20px;'>
                        <div>&nbsp; &nbsp;</div>
                      </div>
                      <div style='line-height: 19px; font-size: 12px; margin-top: 18px;'>

                      </div>
                      <!--[if mso]>&nbsp;<![endif]-->
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]-->
                  <div class='column narrow' style='width: calc(72200px - 12000%); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; float: left; min-width: 200px; max-width: 320px;'>
                    <div style='margin: 10px 20px;'>

                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
              <div class='layout one-col email-footer' style='margin: 0px auto; width: calc(28000% - 167400px); -ms-word-wrap: break-word; min-width: 320px; max-width: 600px; overflow-wrap: break-word;'>
                <div class='layout__inner' style='width: 100%; display: table; border-collapse: collapse;'>
                  <!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0' role='presentation'><tr class='layout-email-footer'><td style='width: 600px;' class='w560'><![endif]-->
                  <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(189, 189, 189); line-height: 19px; font-family: Lato,Tahoma,sans-serif; font-size: 12px; min-width: 320px; max-width: 600px;'>
                    <div style='margin: 10px 20px;'>
                      <div style='line-height: 19px; font-size: 12px;'>
                      </div>
                    </div>
                  </div>
                  <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                </div>
              </div>
            </div>
            <div style='line-height: 40px; font-size: 40px; mso-line-height-rule: exactly;'>&nbsp;</div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
  <img width='1' height='1' style='margin: 0px !important; padding: 0px !important; border: 0px currentColor !important; border-image: none !important; width: 1px !important; height: 1px !important; overflow: hidden; display: block !important; visibility: hidden !important; position: fixed;' alt='' src='https://amura.cmail19.com/t/j-o-ovbx-l/o.gif' border='0'>
</body>
</html>
";
        }
        #endregion

    }

}
