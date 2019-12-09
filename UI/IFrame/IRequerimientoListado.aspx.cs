using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using UI.IFrame;

namespace UI
{
    public partial class IRequerimientoListado : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            InitJs(resultado);
        }
    }
}