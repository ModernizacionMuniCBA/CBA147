<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoCerrar.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoCerrar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoCerrar.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedor_Descripcion">
        <div class="input-field">
            <input type="text" id="input_Motivo" />
            <label for="input_Motivo">Observaciones del cierre</label>
        </div>
    </div>


    <div id="contenedor_Tabla">
        <div class="tabla-contenedor">
            <table id="tablaReclamos"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>
    </div>
    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoCerrar.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
