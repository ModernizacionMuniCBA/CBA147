using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Intranet_UI.Utils
{
    public class JsonUtils
    {
        public static string toJson(object obj)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                ser.Serialize(writer, obj);
            }
            return HttpUtility.JavaScriptStringEncode(sb.ToString());
        }
    }
}