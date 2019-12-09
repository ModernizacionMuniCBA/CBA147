using Rules;
using System;
using System.Linq;
using UI.Resources;
using Rules.Rules;

namespace UI
{
    public partial class MasterPageBase : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147";
        }
    }
}