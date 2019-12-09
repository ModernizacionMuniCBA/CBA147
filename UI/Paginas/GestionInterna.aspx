<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="GestionInterna.aspx.cs" Inherits="UI.GestionInterna" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/Configuracion.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedor_General">
        <label id="texto_TituloGeneral" class="tituloBoton">Paneles</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnFlotas">
                <i class="material-icons">people</i>
                <label>Flotas</label>
            </div>

            <div class="btn-pagina card" id="btnPersonal">
                <i class="material-icons">person</i>
                <label>Personal</label>
            </div>
        </div>
    </div>

    <div id="contenedor_InformacionOrganica">

        <label class="tituloBoton">Gestión Interna</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnConfigurarPersonal">
                <i class="material-icons">person</i>
                <label>Personal</label>
            </div>

            <div class="btn-pagina card" id="btnMoviles">
                <i class="material-icons">directions_car</i>
                <label>Móviles</label>
            </div>

            <div class="btn-pagina card" id="btnZonas">
                <i class="material-icons">map</i>
                <label>Zonas y barrios</label>
            </div>

            <div class="btn-pagina card" id="btnTareas">
                <i class="mdi mdi-hammer"></i>
                <%--<i class="mdi mdi-hammer"></i>--%>
                <label>Tareas</label>
            </div>

            <div class="btn-pagina card" id="btnSecciones">
                <i class="material-icons">settings_applications</i>
                <label>Secciones</label>
            </div>

            <div class="btn-pagina card" id="btnFunciones">
                <i class="material-icons">settings_applications</i>
                <label>Funciones</label>
            </div>
        </div>
    </div>

    <div id="contenedor_Configuraciones" style="display: none">
        <label class="tituloBoton">Configuraciones</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnCatalogo">
                <i class="material-icons">assignment</i>
                <label>Catálogo</label>
            </div>

            <div class="btn-pagina card" id="btnServicioAreaMotivo">
                <i class="material-icons">insert_chart</i>
                <label>Servicio, Area y Motivo</label>
            </div>

            <div class="btn-pagina card" id="btnOtrasConfiguraciones">
                <i class="material-icons">settings_applications</i>
                <label>Otras configuraciones</label>
            </div>
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/GestionInterna.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

