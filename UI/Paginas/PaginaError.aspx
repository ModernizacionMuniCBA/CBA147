<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="PaginaError.aspx.cs" Inherits="UI.PaginaError" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="<%=ResolveUrl("~/Paginas/Styles/PaginaError.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
   
   <div id="contenedor_Error" class="panel-mensaje visible">
        <i class="material-icons"></i>
        <label></label>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/PaginaError.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

