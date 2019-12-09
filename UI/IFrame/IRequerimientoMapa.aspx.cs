using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using Rules.Rules;
using Rules.Rules.Reportes;
using Telerik.Reporting.Processing;
using UI.Resources;
using System.Web;

namespace UI.IFrame
{
    public partial class IRequerimientoMapa : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            InitJs(resultado);
        }
      
    }
}