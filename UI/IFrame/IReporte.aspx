<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IReporte.aspx.cs" Inherits="UI.IFrame.IReporte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IReporte.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedorDocumento"></div>


    <div id="contenedor_Error" class="panel-mensaje">
        <i class="material-icons">error_outline</i>
        <label></label>
    </div>

    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IReporte.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
