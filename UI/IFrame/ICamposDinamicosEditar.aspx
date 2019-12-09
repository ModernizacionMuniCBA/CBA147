<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ICamposDinamicosEditar.aspx.cs" Inherits="UI.IFrame.ICamposDinamicosEditar" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ICamposDinamicosEditar.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Card Alta -->
    <form id="cardFormulario">
        <div id="contenido"  style="display: block">
   
        </div>
    </form>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ICamposDinamicosEditar.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
