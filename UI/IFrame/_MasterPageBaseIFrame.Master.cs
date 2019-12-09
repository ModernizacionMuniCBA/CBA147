using System;
using System.Linq;
using Model.Entities;
using Rules;
using UI.Resources;
using System.Web;
using Rules.Rules;
using System.Web.UI;
using System.Collections.Generic;
using Intranet_UI.Utils;

namespace UI
{
    public partial class _MasterPageBaseIFrame : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfiguracionGeneral.SiglasSistema + " - " + ConfiguracionGeneral.NombreSistema;
         
        }

        
    }
}