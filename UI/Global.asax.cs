using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace UI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
        }


        void Application_EndRequest(object sender, EventArgs e)
        {
            SessionManager.Instance.EndSession();
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            //Basicas
            routes.MapPageRoute("", "Login", "~/Acceso.aspx");
            routes.MapPageRoute("", "Origen", "~/AccesoOrigenes.aspx");

            routes.MapPageRoute("", "Sistema", "~/Default.aspx");

            routes.MapPageRoute("", "Inicio", "~/Paginas/Inicio.aspx");
            routes.MapPageRoute("", "BusquedaGlobal", "~/Paginas/BusquedaGlobal.aspx");
            routes.MapPageRoute("", "CambiarContraseña", "~/Paginas/CambiarPass.aspx");
            routes.MapPageRoute("", "CerrarSesion", "~/CerrarSesion.aspx");
            routes.MapPageRoute("", "RequerimientosFavoritos", "~/Paginas/RequerimientosFavoritosPorUsuario.aspx");
            routes.MapPageRoute("", "Error", "~/Paginas/PaginaError.aspx");
            routes.MapPageRoute("", "NoEncontrado", "~/Paginas/PaginaNoEncontrada.aspx");

            //Emergencias
            routes.MapPageRoute("", "RequerimientoEmergenciaNueva", "~/Paginas/RequerimientoEmergenciaNueva.aspx?Tipo=107");

            //Requerimiento
            routes.MapPageRoute("", "RequerimientoNuevo", "~/Paginas/RequerimientoNuevo.aspx");
            routes.MapPageRoute("", "RequerimientoInternoNuevo", "~/Paginas/RequerimientoInternoNuevo.aspx");
            routes.MapPageRoute("", "RequerimientoExternoNuevo", "~/Paginas/RequerimientoExternoNuevo.aspx");
            routes.MapPageRoute("", "RequerimientoCargaMasiva", "~/Paginas/RequerimientoCargaMasiva.aspx");
            routes.MapPageRoute("", "Requerimientos", "~/Paginas/RequerimientoConsulta.aspx");
            routes.MapPageRoute("", "RequerimientoConsultaBasica", "~/Paginas/RequerimientoConsultaBasica.aspx");

            //Ordenes de inspeccion
            routes.MapPageRoute("", "OrdenesDeInspeccionBandeja", "~/Paginas/RequerimientoConsulta.aspx");
            routes.MapPageRoute("", "OrdenesDeInspeccionConsulta", "~/Paginas/OrdenInspeccionConsulta.aspx");

            //Ordenes de Trabajos
            routes.MapPageRoute("", "OrdenesDeTrabajoBandeja", "~/Paginas/RequerimientoConsulta.aspx");
            routes.MapPageRoute("", "OrdenesDeTrabajoConsulta", "~/Paginas/OrdenTrabajoConsulta.aspx");
            routes.MapPageRoute("", "MisTrabajos", "~/Paginas/OrdenTrabajoConsulta.aspx");
            routes.MapPageRoute("", "OrdenesDeTrabajoTop20", "~/Paginas/RequerimientoTopBarrios.aspx");

            //Ordenes de Atencion Critica
            routes.MapPageRoute("", "OrdenesDeAtencionCriticaBandeja", "~/Paginas/RequerimientoConsulta.aspx");
            routes.MapPageRoute("", "OrdenesDeAtencionCriticaConsulta", "~/Paginas/OrdenAtencionCriticaConsulta.aspx");

            //Gestion Interna
            routes.MapPageRoute("", "GestionInterna", "~/Paginas/GestionInterna.aspx");

        
           

            //Personal
            routes.MapPageRoute("", "PersonalPanel", "~/Paginas/EmpleadoPanel.aspx");
            routes.MapPageRoute("", "PersonalConfiguracion", "~/Paginas/EmpleadoConsulta.aspx");
            routes.MapPageRoute("", "Secciones", "~/Paginas/SeccionConsulta.aspx");

            //Moviles
            routes.MapPageRoute("", "Moviles", "~/Paginas/MovilConsulta.aspx");

            //Flota
            routes.MapPageRoute("", "FlotaPanel", "~/Paginas/FlotaPanel.aspx");

            //Edificio Municipal
            routes.MapPageRoute("", "EdificioMunicipalConsulta", "~/Paginas/EdificioMunicipalConsulta.aspx");

            //Usuarios  
            routes.MapPageRoute("", "UsuarioConsulta", "~/Paginas/UsuarioConsulta.aspx");

            //Estadisticas
            routes.MapPageRoute("", "estadisticaspanel", "~/Paginas/EstadisticasPanel.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoCPC", "~/Paginas/Estadisticas/EstadisticaReclamoCPC.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoOrigen", "~/Paginas/Estadisticas/EstadisticaReclamoOrigen.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoEficacia", "~/Paginas/Estadisticas/EstadisticaReclamoEficacia.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoResueltos", "~/Paginas/Estadisticas/EstadisticaReclamoResueltos.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoServicio", "~/Paginas/Estadisticas/EstadisticaReclamoServicio.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoArea", "~/Paginas/Estadisticas/EstadisticaReclamoArea.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoZona", "~/Paginas/Estadisticas/EstadisticaReclamoZona.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoMotivos", "~/Paginas/Estadisticas/EstadisticaReclamoMotivos.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoUsuario", "~/Paginas/Estadisticas/EstadisticaReclamoUsuario.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoRubros", "~/Paginas/Estadisticas/EstadisticaReclamoRubros.aspx");
            routes.MapPageRoute("", "EstadisticaReclamoSubArea", "~/Paginas/Estadisticas/EstadisticaReclamoSubArea.aspx");

            //Categorias Motivo
            routes.MapPageRoute("", "RubrosMotivo", "~/Paginas/RubroMotivoConsulta.aspx");

            //Usuario Por ATI
            routes.MapPageRoute("", "UsuarioPorATI", "~/Paginas/UsuarioPorATIConsulta.aspx");

            //Configuracion
            routes.MapPageRoute("", "Configuracion", "~/Paginas/Configuracion.aspx");
            routes.MapPageRoute("", "ConfiguracionBandejaPorArea", "~/Paginas/ConfiguracionBandejaPorArea.aspx");
            routes.MapPageRoute("", "ConfiguracionPorArea", "~/Paginas/ConfiguracionPorArea.aspx");
            routes.MapPageRoute("", "InformacionOrganica", "~/Paginas/InformacionOrganicaConsulta.aspx");
            routes.MapPageRoute("", "InformacionOrganicaSecretarias", "~/Paginas/InformacionOrganicaSecretariaConsulta.aspx");
            routes.MapPageRoute("", "InformacionOrganicaDirecciones", "~/Paginas/InformacionOrganicaDireccionConsulta.aspx");
            routes.MapPageRoute("", "RequerimientosInfoGlobal", "~/Paginas/RequerimientoInfoGlobal.aspx");
            routes.MapPageRoute("", "ServicioAreaMotivo", "~/Paginas/ServicioAreaMotivoConsulta.aspx");
            routes.MapPageRoute("", "Catalogos", "~/Paginas/CatalogoConsulta.aspx");

            routes.MapPageRoute("", "Origenes", "~/Paginas/OrigenConsulta.aspx");
            routes.MapPageRoute("", "OrigenesPorAmbito", "~/Paginas/OrigenPorAmbitoConsulta.aspx");
            routes.MapPageRoute("", "OrigenesPorArea", "~/Paginas/OrigenPorAreaConsulta.aspx");
            routes.MapPageRoute("", "OrigenesPorUsuario", "~/Paginas/OrigenPorUsuarioConsulta.aspx");

            routes.MapPageRoute("", "PermisosRequerimiento", "~/Paginas/PermisoEstadoRequerimiento.aspx");
            routes.MapPageRoute("", "PermisosOrdenTrabajo", "~/Paginas/PermisoEstadoOrdenTrabajo.aspx");
            routes.MapPageRoute("", "PermisosOrdenInspeccion", "~/Paginas/PermisoEstadoOrdenInspeccion.aspx");

            routes.MapPageRoute("", "Zonas", "~/Paginas/ZonaConsulta.aspx");
            routes.MapPageRoute("", "Tareas", "~/Paginas/TareaPorAreaConsulta.aspx");

            routes.MapPageRoute("", "Notificaciones", "~/Paginas/NotificacionSistemaConsulta.aspx");

        }

    }
}