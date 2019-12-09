<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUsuarioSelector.aspx.cs" Inherits="UI.IFrame.IUsuarioSelector" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUsuarioSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div id="contenedor-tabla" class="flex direction-vertical no-scroll full-height">
        <div class="tabla-contenedor flex-main flex direction-vertical">
            <table id="tabla"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>

    </div>

    <div id="sin-items" style="display: none">
        <label class="texto">No se encontraron usuarios</label>
    </div>
    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUsuarioSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
