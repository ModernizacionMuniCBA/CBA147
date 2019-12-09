using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IMovilEditarCondicion : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando el móvil");
                InitJs(resultado);
                return;
            }

            int idCondicion=-1;
            if (Request.QueryString["IdCondicion"]!= null)
            {
                idCondicion = Int32.Parse(Request.QueryString["IdCondicion"]);
            }

            int id = Int32.Parse(Request.QueryString["Id"]);
            resultado.Add("IdMovil", id);
            resultado.Add("IdCondicion", idCondicion);

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultCondiciones = new EnumsRules(usuarioLogeado).GetAll(typeof(Enums.CondicionMovil));
            if (!resultCondiciones.Ok)
            {
                resultado.Add("Error", resultCondiciones.Error);
                InitJs(resultado);
                return;
            }

            resultado.Add("Condiciones", resultCondiciones.Return);
            InitJs(resultado);
        }
    }
}