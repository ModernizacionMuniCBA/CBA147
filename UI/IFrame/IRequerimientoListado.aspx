<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoListado.aspx.cs" Inherits="UI.IRequerimientoListado" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoListado.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="no-scroll flex direction-vertical full-height">

        <div class="flex-main flex direction-vertical">
            <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                <table id="tabla"></table>
            </div>
            <div class="tabla-footer">
            </div>

        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoListado.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
