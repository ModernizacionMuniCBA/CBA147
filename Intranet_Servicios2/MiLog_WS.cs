

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Configuration;
namespace Intranet_Servicios2
{
    public class MiLog_WS
    {
        
        private static string NAME = "WSLogger";

        public static void Info(object data)
        {
            bool logear = bool.Parse(ConfigurationManager.AppSettings["LOG"] + "");
            if (!logear) return;

            try
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = ConfigurationManager.AppSettings["LOG_KEY"],
                    BasePath = ConfigurationManager.AppSettings["LOG_URL"]
                };
                IFirebaseClient client = new FirebaseClient(config);

                client.PushAsync(NAME, data);
            }
            catch (Exception e)
            {
            }
        }
    }
}
