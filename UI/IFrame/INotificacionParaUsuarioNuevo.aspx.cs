using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class INotificacionParaUsuarioNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();

            if (Request.Params["Id"] != null)
            {
                var id = int.Parse(Request.Params["Id"] + "");
                var resultadoNotificacion = new NotificacionParaUsuarioRules(SessionKey.getUsuarioLogueado(Session)).GetById(id);
                if (!resultadoNotificacion.Ok)
                {
                    resultado.Add("Error", resultadoNotificacion.Error);
                    InitJs(resultado);
                    return;
                }

                if (resultadoNotificacion.Return == null)
                {
                    resultado.Add("Error", "La notificación no existe");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Notificacion", new Resultado_NotificacionSistema(resultadoNotificacion.Return));
            }

            //Devuelvo la info
            InitJs(resultado);
        }
    }
}