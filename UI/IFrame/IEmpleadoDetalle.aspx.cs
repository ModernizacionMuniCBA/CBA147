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
using Model.Consultas;

namespace UI.IFrame
{
    public partial class IEmpleadoDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = Int32.Parse(Request.QueryString["Id"]);
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                //Usuario
                var resultUsuario = new EmpleadoPorAreaRules(userLogeado).GetResultadoById(id);
                if (!resultUsuario.Ok)
                {
                    resultado.Add("Error", "El empleado no existe");
                    InitJs(resultado);
                    return;
                }

                var resultEstadoOcupado = new EstadoEmpleadoRules(userLogeado).GetAllOcupados();
                if (!resultEstadoOcupado.Ok)
                {
                    resultado.Add("Error", "Error al consultar uno de los datos");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Empleado",  resultUsuario.Return);
                resultado.Add("EstadosOcupado", resultEstadoOcupado.Return);
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}