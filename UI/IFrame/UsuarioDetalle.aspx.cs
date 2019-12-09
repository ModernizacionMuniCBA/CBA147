using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using Rules;
using System.Web;
using Intranet_UI.Utils;
using UI.IFrame;

namespace UI
{
    public partial class UsuarioDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            InitJs(resultado);
        }
    }
}