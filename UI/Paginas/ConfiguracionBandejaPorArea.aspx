<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="ConfiguracionBandejaPorArea.aspx.cs" Inherits="UI.ConfiguracionBandejaPorArea" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/PermisoEstadoRequerimiento.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="card contenedor">
        <div class="contenedor-main">
            <div class="tabla-contenedor">
                <table id="tabla"></table>
            </div>
        </div>
        <div class="contenedor-footer flex direction-horizontal separador">
            <div class="tabla-footer flex-main"></div>
            <div class="contenedor-botones no-select">
                <a id="btn_Guardar" class="btn waves-effect colorExito">Guardar</a>
            </div>
        </div>
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/ConfiguracionBandejaPorArea.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
