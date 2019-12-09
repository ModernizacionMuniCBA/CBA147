using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Rules.Rules.Mails
{
    public class ContactoMailRules : BaseMailRules
    {
        public ContactoMailRules(UsuarioLogueado data)
            : base(data)
        {
        }


        public Result<bool> EnviarMailContacto(string contenido, string mailContacto, string telefonoContacto)
        {
            var resultado = new Result<bool>();

            try
            {
                var usuario = getUsuarioLogueado();
                string nombre = usuario.Usuario.Nombre + " " + usuario.Usuario.Apellido;
                string username = usuario.Usuario.Username;
                string rol = usuario.Rol != null ? usuario.Rol.Rol : "";
                string areas = usuario.Areas != null ? string.Join(" - ", usuario.Areas.Select(x => x.Nombre).ToList()) : "";

                var html = GetHtmlComprobanteAtencion(nombre, username, rol, areas, mailContacto, telefonoContacto, contenido);

                var mails = new List<string>() {
                    "amura_f@cordoba.gov.ar",
                    "rsasia@cordoba.gov.ar",
                    "gafunes@cordoba.gov.ar",
                    "mbagnus@cordoba.gov.ar",
                    "ivillegasrojas@cordoba.gov.ar"
                };

                foreach (string mail in mails)
                {
                    ComandoMail comando = new ComandoMail()
                    {
                        Asunto = "Mensaje de Contacto",
                        Contenido = html,
                        EsHTML = true,
                        ReceptorMail = mail,
                        ReceptorNombre = "Municipalidad de Córdoba"
                    };

                    //Mando
                    var resultadoEnviar = EnviarEmail(comando);
                    if (!resultadoEnviar.Ok)
                    {
                        resultado.AddErrorPublico("Error procesando al operacion");
                        return resultado;
                    }
                }

                resultado.Return = true;
                return resultado;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        #region HTMLS
        private string GetHtmlComprobanteAtencion(string nombreUsuario, string usernameUsuario, string rol, string areas, string mail, string telefono, string mensaje)
        {
            var html = GetHtmlPuroComprobanteATencion();
            html = ReemplazarDatosBasicosEnHTML(html);

            //Datos del usuario
            html = html.Replace("{nombre-usuario}", nombreUsuario);
            html = html.Replace("{username-usuario}", usernameUsuario);
            html = html.Replace("{rol-usuario}", rol);
            html = html.Replace("{areas-usuario}", areas);
            html = html.Replace("{mail-usuario}", mail);
            html = html.Replace("{telefono-usuario}", telefono);

            //Mensaje
            html = html.Replace("{contenido-mensaje}", mensaje);
            html = html.Replace("{fecha-mail}", Utils.DateTimeToString(DateTime.Now));
            return html;
        }

        private string GetHtmlPuroComprobanteATencion()
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
    @import url(https://fonts.googleapis.com/css?family=Ubuntu:400,700,400italic,700italic);
  </style>
  <link href='https://fonts.googleapis.com/css?family=Ubuntu:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
  <!--<![endif]-->
  
  <style type='text/css'>
    body {
      background-color: #38d17e
    }
    
    .logo a:hover,
    .logo a:focus {
      color: #fff !important
    }
    
    .mso .layout-has-border {
      border-top: 1px solid #1f844d;
      border-bottom: 1px solid #1f844d
    }
    
    .mso .layout-has-bottom-border {
      border-bottom: 1px solid #1f844d
    }
    
    .mso .border,
    .ie .border {
      background-color: #1f844d
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
      font-family: Ubuntu, sans-serif
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
                <div class='column' style='width: calc(28000% - 167400px); text-align: left; color: rgb(120, 119, 120); line-height: 24px; font-family: Ubuntu,sans-serif; font-size: 16px; min-width: 320px; max-width: 600px;'>

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
                      <h1 style='text-align: center; color: rgb(86, 86, 86); line-height: 38px; font-size: 30px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Mensaje de contacto&nbsp;</strong></h1>
                      <p style='margin-top: 20px; margin-bottom: 20px;'>El usuario de #CBA147 {nombre-usuario}&nbsp;con nombre de usuario&nbsp;{username-usuario} ha enviado un e-mail de contacto.</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <h2 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 18px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 16px;'>Datos del usuario</h2>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <blockquote style='margin: 0px 0px 20px; padding-left: 14px; border-left-color: rgb(31, 132, 77); border-left-width: 4px; border-left-style: solid;'>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Usuario:</strong> {nombre-usuario} ({username-usuario})</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 0px;'><strong>Rol:</strong> {rol-usuario}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 0px;'><strong>Areas:</strong> {areas-usuario}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 0px;'><strong>E-Mail contacto:</strong> {mail-usuario}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 12px;'><strong style='font-family: inherit; font-size: inherit; font-style: inherit; font-variant-caps: inherit; font-variant-ligatures: inherit;'>Teléfono</strong><strong style='font-family: inherit; font-size: inherit; font-style: inherit; font-variant-caps: inherit; font-variant-ligatures: inherit;'>&nbsp;contacto:</strong> {telefono-usuario}</h3></blockquote>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 20px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <h2 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 18px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 16px;'>Contenido del mensaje</h2>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <blockquote style='margin: 0px 0px 20px; padding-left: 14px; border-left-color: rgb(31, 132, 77); border-left-width: 4px; border-left-style: solid;'>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 0px; margin-bottom: 0px;'><strong>Mensaje:</strong> {contenido-mensaje}</h3>
                        <h3 style='color: rgb(86, 86, 86); line-height: 26px; font-size: 17px; font-style: normal; font-weight: normal; margin-top: 12px; margin-bottom: 12px;'><strong>Fecha:</strong> {fecha-mail}</h3></blockquote>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 30px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p style='margin-top: 0px; margin-bottom: 20px;'>Muchas gracias.</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='line-height: 60px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
                  </div>

                  <div style='margin-right: 20px; margin-left: 20px;'>
                    <div style='mso-line-height-rule: exactly; mso-text-raise: 4px;'>
                      <p class='size-14' lang='x-size-14' style='line-height: 21px; font-size: 14px; margin-top: 0px; margin-bottom: 0px;'>Este e-mail ha sigo generado de forma automática a traves del sistema #CBA147. Por favor, no responda este mensaje.</p>
                      <p class='size-14' lang='x-size-14' style='line-height: 21px; font-size: 14px; margin-top: 20px; margin-bottom: 20px;'>{fecha}</p>
                    </div>
                  </div>

                  <div style='margin-right: 20px; margin-bottom: 24px; margin-left: 20px;'>
                    <div style='line-height: 5px; font-size: 1px; mso-line-height-rule: exactly;'>&nbsp;</div>
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
                        <a style='transition:opacity 0.1s ease-in; color: rgb(189, 189, 189); text-decoration: underline;' href='http://amura.cmail20.com/t/j-u-owihz-l-h/'>Cancelar suscripción</a>
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
  <img width='1' height='1' style='margin: 0px !important; padding: 0px !important; border: 0px currentColor !important; border-image: none !important; width: 1px !important; height: 1px !important; overflow: hidden; display: block !important; visibility: hidden !important; position: fixed;' alt='' src='https://amura.cmail20.com/t/j-o-owihz-l/o.gif' border='0'>
</body>
</html>
";
        }
        #endregion

    }

}
