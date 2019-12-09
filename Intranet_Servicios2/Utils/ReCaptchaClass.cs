using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;

namespace Intranet_Servicios.Utils
{
    public class ReCaptchaClass
    {
        public static bool Validate(string EncodedResponse)
        {
            try
            {
                var validarRecaptcha = bool.Parse(ConfigurationManager.AppSettings["RECAPTCHA_VALIDAR"]);
                if (!validarRecaptcha) return true;

                var client = new System.Net.WebClient();
                string PrivateKey = ConfigurationManager.AppSettings["RECAPTCHA_PRIVATE_KEY"];
                var urlRecaptcha = ConfigurationManager.AppSettings["RECAPTCHA_URL"] + "?secret={0}&response={1}";
                var GoogleReply = client.DownloadString(string.Format(urlRecaptcha, PrivateKey, EncodedResponse));
                var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);
                var resultado = captchaResponse.Success;
                if (resultado == null || resultado.ToLower() != "true") return false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }


        private List<string> m_ErrorCodes;
    }
}