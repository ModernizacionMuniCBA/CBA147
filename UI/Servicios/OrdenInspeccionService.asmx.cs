using Model;
using Model.Comandos;
using Model.Consultas;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UI.Resources;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class OrdenInspeccionService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenInspeccionInit> Init(List<int> idsRequerimientos)
        {
            ValidarSesion(Session);
            return new OrdenInspeccionRules(SessionKey.getUsuarioLogueado(Session)).Init(idsRequerimientos);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenInspeccion> Insert(Comando_OrdenInspeccion comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultado = new Result<Resultado_OrdenInspeccion>();

            var resultInsertarOrdenInspeccion= new OrdenInspeccionRules(userLogueado).Insert(comando);
            if (!resultInsertarOrdenInspeccion.Ok)
            {
                resultado.Copy(resultInsertarOrdenInspeccion.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrdenInspeccion(resultInsertarOrdenInspeccion.Return);
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenInspeccionDetalle> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            return new OrdenInspeccionRules(GetUsuarioLogeado()).GetDetalleById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarDescripcion(Comando_OrdenInspeccion_Descripcion comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).EditarDescripcion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarRequerimientos(Comando_OrdenInspeccion_Requerimientos comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).AgregarRequerimientos(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarRequerimiento(Comando_OrdenTrabajo_QuitarRequerimiento comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).QuitarRequerimiento(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarNota(Comando_OrdenInspeccion_Nota comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).AgregarNota(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Completar(Comando_OrdenInspeccion_Cerrar comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).Completar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Cancelar(Comando_OrdenTrabajo_Cancelar comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).Cancelar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetDatosTabla(Consulta_OrdenInspeccion consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).GetDatosTabla(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetDatosTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenInspeccionRules(userLogeado).GetDatosTablaByIds(ids);
        }
    }
}
