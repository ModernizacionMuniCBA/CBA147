<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageBaseIFrame.Master" AutoEventWireup="true" CodeBehind="INotificacionParaUsuarioDetalle.aspx.cs" Inherits="UI.IFrame.INotificacionParaUsuarioDetalle" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Include stylesheet -->
    <link href="https://cdn.quilljs.com/1.1.6/quill.snow.css" rel="stylesheet">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>

    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/INotificacionParaUsuarioDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Create the editor container -->
    <div id="editor">
    </div>


    <!-- Include the Quill library -->
    <script src="https://cdn.quilljs.com/1.1.6/quill.js"></script>
    <script>
      
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/INotificacionParaUsuarioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
