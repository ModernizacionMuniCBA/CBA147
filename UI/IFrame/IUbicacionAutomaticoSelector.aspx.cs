using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using System.Collections.Generic;
using Model;
using Intranet_UI.Utils;
using UI.Resources;
using System.Web;
using Model.Resultados;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IUbicacionAutomaticoSelector : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            InitJs(resultado);
        }
    }
}