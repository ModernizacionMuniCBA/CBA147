<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="UI.Controls.Navigation.Header" %>

<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Controls/Navigation/Styles/Header.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />

<div id="header">
    <nav>

        <!-- Toolbar -->
        <div id="header_Toolbar" class="nav-wrapper header">

            <%--Titulo--%>
            <div id="header_Toolbar_ContenedorTitulo">
                <a id="btnExpandirDrawer" href="#" class="btn-flat btn-redondo  waves-effect"><i class="material-icons">menu</i></a>
                <a id="btnDrawer" href="#" data-activates="nav-mobile" class="button-collapse btn-flat btn-redondo  waves-effect"><i class="material-icons">menu</i></a>
                <a id="textoToolbar_Titulo" runat="server" class="toolbarTitulo no-select"></a>
            </div>


            <%--Busqueda--%>
            <div id="header_Toolbar_ContenedorBusqueda">
                <div id="header_Toolbar_Busqueda" class="waves-effect waves-light">
                    <input id="toolbar_InputBusqueda" type="text" placeholder="Buscar por número o DNI del referente..." />
                    <i class="material-icons">search</i>

                    <a id="toolbar_BtnCancelarBusqueda" class="btn-flat btn-redondo chico waves-effect"><i class="material-icons">clear</i></a>
                </div>
            </div>

            <%--                     <div id="header_Toolbar_ContenedorBusquedaReferente">
                <div id="header_Toolbar_BusquedaReferente" class="waves-effect waves-light">
                    <input id="toolbar_InputBusquedaReferente" type="text" placeholder="Buscar por DNI de referente..." />
                    <i class="material-icons">search</i>

                    <a id="toolbar_BtnCancelarBusquedaReferente" class="btn-flat btn-redondo chico waves-effect"><i class="material-icons">clear</i></a>
                </div>
            </div>--%>

            <%--Botones--%>
            <div class="botones-toolbar">
                <div id="header_Toolbar_ContenedorBtnInicio" class="contenedor-boton tooltipped" data-position="bottom" data-delay="50" data-tooltip="Inicio">
                    <a id="header_BtnInicio" class="btn-flat btn-redondo  waves-effect"><i class="material-icons">home</i></a>
                </div>

                <div id="header_Toolbar_ContenedorBtnRequerimientosFavoritos" class="contenedor-boton tooltipped" data-position="bottom" data-delay="50" data-tooltip="Requerimientos favoritos">
                    <a id="header_BtnRequerimientosFavoritos" class="btn-flat btn-redondo  waves-effect"><i class="material-icons">star</i></a>
                    <label id="header_Toolbar_TextoCantidadRequerimientosFavoritos" class="indicadorBagde"></label>
                </div>

                <%--    <div id="header_Toolbar_ContenedorBtnNotificacionesParaUsuario" class="contenedor-boton">
                    <a id="header_Toolbar_BtnNotificacionesParaUsuario" class="btn-flat btn-redondo  waves-effect"><i class="material-icons">notifications</i></a>
                    <label id="header_Toolbar_TextoCantidadNotificaciones" class="indicadorBagde"></label>
                </div>--%>

                <div id="header_Toolbar_ContenedorBtnMisTrabajos" class="contenedor-boton tooltipped" data-position="bottom" data-delay="50" data-tooltip="Mis Trabajos">
                    <a id="header_Toolbar_BtnMisTrabajos" class="btn-flat btn-redondo  waves-effect"><i class="material-icons">build</i></a>
                    <label id="header_Toolbar_TextoCantidadMisTrabajos" class="indicadorBagde"></label>
                </div>
            </div>

            <%--Indicadores--%>
            <div id="header_Toolbar_ContenedorIndicadores">
                <div id="header_Toolbar_ContenedorIndicador_Urgentes" class="contenedorIndicador tooltipped" data-position="bottom" data-delay="50" data-tooltip="Requerimientos peligrosos a resolver">
                    <div class="contenido waves-effect">
                        <i class="material-icons">warning</i>
                        <label></label>
                    </div>
                </div>
            </div>

            <%--Usuario--%>
            <div id="header_Toolbar_Usuario" class="waves-effect">
            </div>
        </div>


        <div id="toolbar_ContenedorSugerenciasBusqueda">
            <div class="fondo"></div>
            <div class="sugerencias">
            </div>
        </div>

        <div id="template_SugerenciaBusqueda" style="display: none">
            <div class="sugerencia waves-effect">
                <i class="material-icons">history</i>
                <label class="texto"></label>
                <a class="btn-flat chico waves-effect btn-redondo"><i class="material-icons">clear</i></a>
            </div>
        </div>
        <!-- Menu Lateral -->
        <ul id="nav-mobile" class="side-nav fixed flex direction-vertical full-height no-scroll">

            <!-- Titulo -->
            <div id="contenedor_DrawerTitulo">
                <label>Menu</label>
            </div>

            <!-- Menu -->
            <li class="navegacion flex-main">
                <ul id="contenedorMenu" runat="server" class="collapsible" data-collapsible="accordion">
                </ul>
            </li>

            <!-- Telefono -->
            <li class="header-footer no-select">
                <div id="imagen_LogoMuni" class="logo-muni no-select"></div>
                <div id="imagen_LogoCba147" class="logo-cba147 no-select"></div>
                <label id="texto_Version"></label>
                <div class="contenedorContacto" id="idContenedorContacto">
                    <a class="titulo no-select">Soporte</a>
                </div>
            </li>

        </ul>
    </nav>
</div>

<!-- Dropdown Usuario -->
<ul id='popupUsuario' class='dropdown-content'>
    <li><a id="btnInfoUsuario" href="#" class="no-select">Info usuario</a></li>
    <li><a id="btnCambiarPassword" href="#" class="no-select">Cambiar contraseña</a></li>
    <li><a id="btnCerrarSesion" href="#" class="no-select">Cerrar sesión</a></li>
</ul>


<%--Notificaciones Para Usuario--%>
<div id="header_ContenedorNotificacionesParaUsuario" class="panelFlotante">
    <div class="fondo waves-effect"></div>
    <div class="contenido">
        <div class="indicadorVacio">
            <i class="material-icons">notifications</i>
            <label>No hay ninguna notificación</label>
        </div>
        <div class="indicadorCargando">
            <div class="preloader-wrapper big active">
                <div class="spinner-layer">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div>
                    <div class="gap-patch">
                        <div class="circle"></div>
                    </div>
                    <div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="items">
            <div class="contenedorTitulo">
                <label class="titulo">Notificaciones</label>
                <a class="btnCerrar btn-flat btn-redondo waves-effect"><i class="material-icons">clear</i></a>
            </div>
            <div class="content"></div>
        </div>
    </div>
    <div class="templateItem" style="display: none">
        <div class="item">
            <i class="icono material-icons"></i>
            <div class="textos">
                <label class="textoTitulo"></label>
                <label class="textoContenido" style="display: none"></label>
            </div>

        </div>
    </div>
</div>

<%--Requerimientos favoritos--%>
<%--<div id="header_ContenedorRequerimientosFavoritos" class="panelFlotante">
    <div class="fondo waves-effect"></div>
    <div class="contenido">
        <div class="indicadorVacio">
            <i class="material-icons">favorite</i>
            <label>No tiene ningun requerimiento marcado como favorito</label>
        </div>
        <div class="indicadorCargando">
            <div class="preloader-wrapper big active">
                <div class="spinner-layer">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div>
                    <div class="gap-patch">
                        <div class="circle"></div>
                    </div>
                    <div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="items">
            <div class="contenedorTitulo">
                <label class="titulo">Requerimientos favoritos</label>
                <a class="btnCerrar btn-flat btn-redondo waves-effect"><i class="material-icons">clear</i></a>
            </div>
            <div class="content">
                <div class="tabla-contenedor">
                    <table id="header_TablaRequerimientosFavoritos"></table>
                </div>
                <div class="tabla-footer">
                </div>
            </div>
        </div>
    </div>
</div>--%>

<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Navigation/Scripts/Header.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
