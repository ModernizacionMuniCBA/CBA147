<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientosCercanos.aspx.cs" Inherits="UI.IRequerimientosCercanos" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoCercano.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="contenedorTabla">
        <div class="contenedorAyuda">
            <i class="material-icons">help</i>
            <label>En esta pantalla le aparecen requerimientos con el mismo motivo y que se encuentren en un radio de 50 metros del requerimiento que usted esta creando. Si el requerimiento existe, puede sumarse al mismo con el "+1".</label>
        </div>
        <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
            <table id="tabla"></table>
        </div>
        <div class="tabla-footer">
        </div>
    </div>

    <div class="contenedorMapa">
        <div id="map"></div>
    </div>

    <!-- Mi JS -->
    <script src="https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/markerclusterer.js"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientosCercanos.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
