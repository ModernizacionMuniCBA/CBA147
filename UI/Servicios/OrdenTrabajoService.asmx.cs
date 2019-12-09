using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using Rules.Rules.Reportes;
using UI.Resources;
using System.Web;
using Model.Resultados;
using Intranet_UI.Utils;
using Model.Comandos;
using Model.Consultas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class OrdenTrabajoService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenTrabajoInit> Init(List<int> idsRequerimientos)
        {
            ValidarSesion(Session);
            return new OrdenTrabajoRules(SessionKey.getUsuarioLogueado(Session)).Init(idsRequerimientos);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenTrabajo> Insertar(Comando_OrdenTrabajo comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultado = new Result<Resultado_OrdenTrabajo>();

            var resultInsertarOrdenTrabajo = new OrdenTrabajoRules(userLogueado).Insert(comando);
            if (!resultInsertarOrdenTrabajo.Ok)
            {
                resultado.Copy(resultInsertarOrdenTrabajo.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_OrdenTrabajo(resultInsertarOrdenTrabajo.Return);
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EnviarMail(Comando_OrdenTrabajoMail comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EnviarMail(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).GetDatosTablaByIds(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_OrdenTrabajo> GetDatosTablaById(int id)
        {
            ValidarSesion(Session);
            return new OrdenTrabajoRules(GetUsuarioLogeado()).GetDatosTablaById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenTrabajoDetalle> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            return new OrdenTrabajoRules(GetUsuarioLogeado()).GetDetalleById(id);
        }

        //NUEVOS METODOS
        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTabla(Consulta_OrdenTrabajo consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            return new OrdenTrabajoRules(userLogeado).GetDatosTabla(consulta);
        }

           [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaMisTrabajos(Consulta_OrdenTrabajo consulta)
        {
            ValidarSesion(Session);
            return new OrdenTrabajoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetDatosTablaMisTrabajos(consulta);
        }

           [WebMethod(EnableSession = true)]
           public Result<int> GetCantidadMisTrabajos()
           {
               ValidarSesion(Session);
               return new OrdenTrabajoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetCantidadMisTrabajos();
           }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetDatosTablaByIdEmpleado(Consulta_OrdenTrabajo consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).GetDatosTablaByIdEmpleado(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarDescripcion(Comando_OrdenTrabajo_Descripcion comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EditarDescripcion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarRequerimiento(Comando_OrdenTrabajo_Requerimiento comando)
        {
            return AgregarRequerimientos(new Comando_OrdenTrabajo_Requerimientos()
            {
                IdOrdenTrabajo = comando.IdOrdenTrabajo,
                IdsRequerimientos = new List<int>() { comando.IdRequerimiento }
            });
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarRequerimientos(Comando_OrdenTrabajo_Requerimientos comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).AgregarRequerimientos(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarRequerimiento(Comando_OrdenTrabajo_QuitarRequerimiento comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).QuitarRequerimiento(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarRecursos(Comando_OrdenTrabajo_Recursos comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EditarRecursos(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarNota(Comando_OrdenTrabajo_Nota comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).AgregarNota(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarMoviles(Comando_OrdenTrabajo_Moviles comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EditarMoviles(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarMovil(Comando_OrdenTrabajo_QuitarMovil comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).QuitarMovil(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarEmpleados(Comando_OrdenTrabajo_Empleados comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EditarEmpleados(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarEmpleado(Comando_OrdenTrabajo_QuitarEmpleado comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).QuitarEmpleado(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarFlotas(Comando_OrdenTrabajo_Flotas comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).EditarFlotas(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarFlota(Comando_OrdenTrabajo_QuitarFlota comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogeado).QuitarFlota(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Completar(Comando_OrdenTrabajo_Cerrar comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogueado).Completar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Cancelar(Comando_OrdenTrabajo_Cancelar comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogueado).Cancelar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarSeccion(Comando_OrdenTrabajo_Seccion comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogueado).CambiarSeccion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> CantidadMovilesPorAreas(int idArea)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new MovilRules(userLogueado).GetCantidadParaAgregarAOT(idArea);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenTrabajoPanelMasInfo> GetResultadoPanelMasInfoById(int id)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenTrabajoRules(userLogueado).GetResultadoPanelMasInfoById(id);
        }
        
    }

}
