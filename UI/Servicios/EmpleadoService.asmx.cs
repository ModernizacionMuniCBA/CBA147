using Model;
using Model.Comandos;
using Model.Consultas;
using Model.Entities;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UI.Servicios
{
    /// <summary>
    /// Descripción breve de EmpleadoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class EmpleadoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_EmpleadoPorArea> Insert(Comando_Empleado comando)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_EmpleadoPorArea> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaByIds(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_Empleado> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByFilters(Consulta_Empleado consulta)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByIdOrdenTrabajo(int idOrden)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaByIdOrdenTrabajo(idOrden);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarFunciones(Comando_Empleado_EditarFunciones comando)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).EditarFunciones(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> DarDeBaja(int idEmpleado)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).DarDeBaja(idEmpleado);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarEstado(Comando_Empleado_CambioEstado comando)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).CambiarEstado(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>> GetResultadoTablaPanelByFilters(Consulta_Empleado consulta)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaPanelByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_EmpleadoPanel> GetResultadoTablaPanelById(int id)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetResultadoTablaPanelById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadParaAgregarAOT(int id)
        {
            ValidarSesion(Session);
            return new EmpleadoPorAreaRules(GetUsuarioLogeado()).GetCantidadParaAgregarAOT(id);
        }
    }
}
