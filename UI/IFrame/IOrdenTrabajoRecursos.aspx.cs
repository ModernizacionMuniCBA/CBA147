using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IOrdenTrabajoRecursos : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resultadoConsulta = new OrdenTrabajoService().GetDetalleById(int.Parse(Request.Params["Id"] + ""));
                if (!resultadoConsulta.Ok)
                {
                    resultado.Add("Error", resultadoConsulta.ToStringPublico());
                    InitJs(resultado);
                    return;
                }

                var ot = resultadoConsulta.Return;
                if (ot == null)
                {
                    resultado.Add("Error", "La orden de trabajo no existe");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("OrdenTrabajo", ot);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
            }

            //Devuelvo la info
            InitJs(resultado);
        }
    }
}