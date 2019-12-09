using Intranet_UI.Utils;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI.IFrame
{
    public partial class IOrdenTrabajoEnviarMail : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = int.Parse(Request.Params["id"] as string);           
                
  
                resultado.Add("IdOrdenTrabajo", id);
            }
            catch (Exception)
            {
                resultado.Add("Error", "Identificador de orden invalido");
            }

            InitJs(resultado);
        }
    }
}