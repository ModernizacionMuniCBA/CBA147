<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilSelector.aspx.cs" Inherits="UI.IFrame.IMovilSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical no-scroll full-height">


        <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
            <table id="tablaMovilesSelector"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
