using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAO.DAO;
using Model;
using Model.Entities;
using System.Reflection.Emit;
using Telerik.Reporting.Processing;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using Rules.Resources;

namespace Rules.Rules.Mails
{
    public class BaseMailRules
    {
        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        public BaseMailRules(UsuarioLogueado data)
        {
            this.data = data;
        }

        protected Result<bool> EnviarEmail(ComandoMail comando)
        {

            var resultado = new Result<bool>();

            try
            {
                if (!comando.validar())
                {
                    resultado.AddErrorPublico("Error con el comando");
                    return resultado;
                }

                comando.encode();

                MailMessage message = new MailMessage();
                message.To.Add(comando.ReceptorMail);

                //Mando el mail
                message.Subject = comando.Asunto;
                if (string.IsNullOrEmpty(comando.Contenido))
                {
                    message.Body = "";
                }
                else
                {
                    message.Body = comando.Contenido;
                }

                if (comando.EsHTML)
                {
                    message.IsBodyHtml = true;
                }


                if (comando.Adjuntos != null)
                {
                    comando.Adjuntos.ForEach(x => message.Attachments.Add(new Attachment(new MemoryStream(x.Data), x.Nombre)));
                }

                //Prioridad
                message.Priority = MailPriority.Normal;

                Task task = Task.Run(async () => await SendEmail(message));

                resultado.Return = true;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public async Task SendEmail(MailMessage mailMessage)
        {
            MiLog.Info("Por enviar e-mail");
            var smtpClient = new SmtpClient();
            smtpClient.SendCompleted += (s, e) =>
            {
                MiLog.Info("Termine de enviar el e-mail");
                smtpClient.Dispose();
            };
            await smtpClient.SendMailAsync(mailMessage);
        }

        public class ComandoMail
        {
            public string ReceptorMail { get; set; }
            public string ReceptorNombre { get; set; }
            public string Asunto { get; set; }
            public string Contenido { get; set; }
            public bool EsHTML { get; set; }
            public List<AdjuntoMail> Adjuntos { get; set; }

            public bool validar()
            {
                if (string.IsNullOrEmpty(ReceptorMail)) return false;
                if (string.IsNullOrEmpty(ReceptorNombre)) return false;
                if (string.IsNullOrEmpty(Asunto)) return false;

                if (Adjuntos != null && Adjuntos.Count != 0)
                {
                    foreach (var a in Adjuntos)
                    {
                        if (a.Data == null) return false;
                        if (string.IsNullOrEmpty(a.Nombre)) return false;
                    }
                }
                return true;
            }

            public void encode()
            {
                if (!validar()) return;
                ReceptorMail = HttpUtility.JavaScriptStringEncode(ReceptorMail);
                ReceptorNombre = HttpUtility.JavaScriptStringEncode(ReceptorNombre);
                Asunto = HttpUtility.JavaScriptStringEncode(Asunto);

                if (Adjuntos != null && Adjuntos.Count != 0)
                {
                    foreach (var a in Adjuntos)
                    {
                        a.Nombre = HttpUtility.JavaScriptStringEncode(a.Nombre);
                    }
                }
            }
        }

        protected String ReemplazarDatosBasicosEnHTML(string html)
        {
            //Basicos
            html = html.Replace("{url-imagen-muni}", Resource.url_imagen_muni);
            html = html.Replace("{url-imagen-cba147}", Resource.url_imagen_cba147);
            html = html.Replace("{fecha}", Utils.DateTimeToString(DateTime.Now));
            html = html.Replace("{url-muni}", Resource.url_muni);

            //Redes
            html = html.Replace("{url-facebook}", Resource.url_facebook);
            html = html.Replace("{url-twitter}", Resource.url_twitter);
            html = html.Replace("{url-instagram}", Resource.url_instagram);
            html = html.Replace("{url-youtube}", Resource.url_youtube);

            //App
            html = html.Replace("{url-app-android}", Resource.url_app_android);
            html = html.Replace("{url-app-ios}", Resource.url_app_ios);
            html = html.Replace("{url-web}", Resource.url_web);

            return html;
        }

        public class AdjuntoMail
        {
            public byte[] Data { get; set; }
            public String Nombre { get; set; }
        }
    }
}
