using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Internet_UI.Utils
{
    public class JsUtils
    {
        public static void InitMasterPage(MasterPage masterPage, object data)
        {
            var funcion = "$(function () { var data = parse('" + ToJson(data) + "'); initMaster(data); });";
            funcion = "$(window).on('load', function(){" + funcion + "} );";
            ScriptManager.RegisterStartupScript(masterPage, masterPage.GetType(), "scriptMasterPage", funcion, true);
        }

        public static void InitPage(Page page, Object data)
        {
            var funcion = "$(function () { var data = parse('" + ToJson(data) + "'); init(data); });";
            funcion = "$(window).on('load', function(){ setTimeout(function(){" + funcion + "},100);});";
            ScriptManager.RegisterStartupScript(page, page.GetType(), "scriptPage", funcion, true);
        }

        public static string ToJson(object obj)
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