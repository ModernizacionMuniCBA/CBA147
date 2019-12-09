<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="EstadisticasPanel.aspx.cs" Inherits="UI.EstadisticasPanel" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/Configuracion.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedor_General">
        <label id="texto_TituloGeneral" class="tituloBoton">Estadísticas</label>

        <div class="contenedor-botones-paginas">
            <div class="btn-pagina card" id="btnCpcGlobal">
                <i class="material-icons">people</i>
                <label>Cpc Global</label>
            </div>
            <div class="btn-pagina card" id="btnOrigen">
                <i class="material-icons">person</i>
                <label>Origen</label>
            </div>
            <div class="btn-pagina card" id="btnEficacia">
                <i class="material-icons">people</i>
                <label>Eficacia</label>
            </div>
            <div class="btn-pagina card" id="btnResueltos">
                <i class="material-icons">person</i>
                <label>Resueltos</label>
            </div>
            <div class="btn-pagina card" id="btnServicios">
                <i class="material-icons">people</i>
                <label>Servicios</label>
            </div>
            <div class="btn-pagina card" id="btnZona">
                <i class="material-icons">person</i>
                <label>Zona</label>
            </div>
            <div class="btn-pagina card" id="btnUsuarios">
                <i class="material-icons">people</i>
                <label>Usuarios</label>
            </div>
            <div class="btn-pagina card" id="btnMotivos">
                <i class="material-icons">person</i>
                <label>Motivos</label>
            </div>
            <div class="btn-pagina card" id="btnRubros">
                <i class="material-icons">people</i>
                <label>Rubros</label>
            </div>
            <div class="btn-pagina card" id="btnAreas">
                <i class="material-icons">person</i>
                <label>Areas</label>
            </div>
            <div class="btn-pagina card" id="btnSubareas">
                <i class="material-icons">people</i>
                <label>Subareas</label>
            </div>
        </div>
    </div>




    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/EstadisticasPanel.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

