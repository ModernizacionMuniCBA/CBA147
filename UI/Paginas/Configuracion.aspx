<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="UI.Configuracion" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/Configuracion.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedor_General">
        <label id="texto_TituloGeneral" class="tituloBoton">General</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnNotificacionesSistema">
                <i class="material-icons">notifications</i>
                <label>Notificaciones</label>
            </div>

            <div class="btn-pagina card" id="btnVersionSistema">
                <i class="material-icons">info</i>
                <label>Versión del Sistema</label>
            </div>

            <div class="btn-pagina card" id="btnBarrios">
                <i class="material-icons">location_on</i>
                <label>Insertar barrios</label>
            </div>

            <div class="btn-pagina card" id="btnInfoGlobal">
                <i class="material-icons">insert_chart</i>
                <label>Info global</label>
            </div>

            <div class="btn-pagina card" id="btnEdificiosMunicipales">
                <i class="material-icons">location_city</i>
                <label>Edificios municipales</label>
            </div>

            <div class="btn-pagina card" id="btnRubrosMotivo">
                <i class="material-icons">insert_chart</i>
                <label>Rubros de Motivos</label>
            </div>
        </div>
    </div>

    <div id="contenedor_InformacionOrganica">

        <label class="tituloBoton">Información Orgánica</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnInformacionOrganica">
                <i class="material-icons">assignment</i>
                <label>Informacion orgánica</label>
            </div>

            <div class="btn-pagina card" id="btnSecretarias">
                <i class="material-icons">account_balance</i>
                <label>Secretarias</label>
            </div>

            <div class="btn-pagina card" id="btnDirecciones">
                <i class="material-icons">account_balance</i>
                <label>Direcciones</label>
            </div>
        </div>
    </div>


    <div id="contenedor_Origenes">

        <label class="tituloBoton">Origenes</label>

        <div class="contenedor-botones-paginas">

            <div class="btn-pagina card" id="btnOrigenes">
                <i class="material-icons">my_location</i>
                <label>Origenes</label>
            </div>

            <div class="btn-pagina card" id="btnOrigenesPorArea">
                <i class="material-icons">account_balance</i>
                <label>Origenes por Area</label>
            </div>

            <div class="btn-pagina card" id="btnOrigenesPorAmbito">
                <i class="material-icons">account_balance</i>
                <label>Origenes por Ambito</label>
            </div>

            <div class="btn-pagina card" id="btnOrigenesPorUsuario">
                <i class="material-icons">person</i>
                <label>Origenes por Usuario</label>
            </div>

        </div>
    </div>

    <div id="contenedor_Permisos">
        <label class="tituloBoton">Permisos</label>
        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnPermisosRequerimiento">
                <i class="material-icons">vpn_key</i>
                <label>Permisos Requerimiento</label>
            </div>
            <div class="btn-pagina card" id="btnPermisosOrdenTrabajo">
                <i class="material-icons">vpn_key</i>
                <label>Permisos Orden Trabajo</label>
            </div>
            <div class="btn-pagina card" id="btnPermisosOrdenInspeccion">
                <i class="material-icons">vpn_key</i>
                <label>Permisos Orden Inspección</label>
            </div>
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/Configuracion.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

