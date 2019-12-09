

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Configuration;
namespace Rules
{
    public class MiLog
    {
        private static string NAME = "Rules";

        public static void Info(string mensaje)
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

                client.PushAsync(NAME, mensaje);
            }
            catch (Exception e)
            {
            }
        }
    }
}
