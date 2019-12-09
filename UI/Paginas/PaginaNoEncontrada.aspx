<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="PaginaNoEncontrada.aspx.cs" Inherits="UI.PaginaNoEncontrada" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="<%=ResolveUrl("~/Paginas/Styles/PaginaNoEncontrada.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
      <div id="contenedor_Error" class="panel-mensaje visible">
        <i class="material-icons">error_outline</i>
        <label>La página solicitada no existe.</label>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/PaginaNoEncontrada.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

