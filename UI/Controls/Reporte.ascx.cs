using Rules.Rules;
using Rules.Rules.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace UI.Controls
{
    public partial class Reporte : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void MostrarReporte(Telerik.Reporting.Report reporte)
        {
            Telerik.Reporting.InstanceReportSource reportSource = new InstanceReportSource();
            reportSource.ReportDocument = reporte;
            ReportViewer.ReportSource = reportSource;
        }

    }
}
