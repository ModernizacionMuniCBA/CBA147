using Internet_UI.Utils;
using System;
using System.Linq;
using System.Web.Routing;

namespace Internet_UI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("", Consts.PAGE_LOGIN, "~/Paginas/Login.aspx");
            routes.MapPageRoute("", Consts.PAGE_CERRAR_SESION, "~/Paginas/CerrarSesion.aspx");

            routes.MapPageRoute("", Consts.PAGE_INICIO, "~/Paginas/MisRequerimientos.aspx");

            routes.MapPageRoute("", Consts.PAGE_MIS_REQUERIMIENTOS, "~/Paginas/MisRequerimientos.aspx");
            routes.MapPageRoute("", Consts.PAGE_NUEVO_REQUERIMIENTO, "~/Paginas/RequerimientoNuevo.aspx");
            routes.MapPageRoute("", Consts.PAGE_DETALLE_REQUERIMIENTO, "~/Paginas/RequerimientoDetalle.aspx");

            routes.MapPageRoute("", Consts.PAGE_AJUSTES, "~/Paginas/Ajustes.aspx");
            routes.MapPageRoute("", Consts.PAGE_CAMBIAR_USERNAME, "~/Paginas/CambiarUsername.aspx");
            routes.MapPageRoute("", Consts.PAGE_CAMBIAR_PASSWORD, "~/Paginas/CambiarPassword.aspx");
            routes.MapPageRoute("", Consts.PAGE_MI_PERFIL, "~/Paginas/MiPerfil.aspx");
            routes.MapPageRoute("", Consts.PAGE_VALIDAR_DATOS, "~/Paginas/ValidarDatos.aspx");

            routes.MapPageRoute("", Consts.PAGE_ESTADISTICA_TV, "~/Paginas/EstadisticasTV.aspx");



            //SISI
            routes.MapPageRoute("", "SiSi_Login", "~/PaginasSiSi/Login.aspx");
            routes.MapPageRoute("", "SiSi_Inicio", "~/PaginasSiSi/Inicio.aspx");
            routes.MapPageRoute("", "SiSi_Programas", "~/PaginasSiSi/Programas.aspx");
            routes.MapPageRoute("", "SiSi_Entrevista", "~/PaginasSiSi/Entrevista.aspx");

            routes.MapPageRoute("", "SiSi_ValidarDatos", "~/PaginasSiSi/ValidarDatos.aspx");
            routes.MapPageRoute("", "SiSi_CompletarPerfil", "~/PaginasSiSi/CompletarPerfil.aspx");

            routes.MapPageRoute("", "SiSi_CambiarUsername", "~/PaginasSiSi/CambiarUsername.aspx");
            routes.MapPageRoute("", "SiSi_CambiarPassword", "~/PaginasSiSi/CambiarPassword.aspx");
            routes.MapPageRoute("", "SiSi_MiPerfil", "~/PaginasSiSi/MiPerfil.aspx");
            routes.MapPageRoute("", "SiSi_ExperienciaLaboral", "~/PaginasSiSi/ExperienciaLaboral.aspx");
            routes.MapPageRoute("", "SiSi_CerrarSesion", "~/PaginasSiSi/CerrarSesion.aspx");
            routes.MapPageRoute("", "SiSi_PreinscripcionesInfoGlobal", "~/PaginasSiSi/PreinscripcionesInfoGlobal.aspx");


        }
    }
}