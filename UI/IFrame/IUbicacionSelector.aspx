<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUbicacionSelector.aspx.cs" Inherits="UI.IFrame.IUbicacionSelector" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUbicacionSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <Controles:Mapa ID="mapa" runat="server" />


    <div id="template_Input" style="display: none">
        <div id="contenedor_Observaciones">
            <input id="input_Observaciones" type="text" placeholder="Observación del domicilio..." />
            <i class="material-icons icono_Error">error_outline</i>
        </div>
    </div>
    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUbicacionSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
