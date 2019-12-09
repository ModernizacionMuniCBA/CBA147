<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageBaseIFrame.Master" AutoEventWireup="true" CodeBehind="INotificacionParaUsuarioNuevo.aspx.cs" Inherits="UI.IFrame.INotificacionParaUsuarioNuevo" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Include stylesheet -->
    <link href="https://cdn.quilljs.com/1.1.6/quill.snow.css" rel="stylesheet">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>

    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/INotificacionParaUsuarioNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div class="encabezado">
        <input id="input_Titulo" placeholder="Titulo" type="text" />
        <label id="contenedorCheckNotificar">
            <input type="checkbox" id="check_Notificar" value="first_checkbox" />
            Notificar</label>
    </div>

    <!-- Create the editor container -->
    <div id="contenedorEditor">
        <div id="editor">
        </div>
    </div>


    <!-- Include the Quill library -->
    <script src="https://cdn.quilljs.com/1.1.6/quill.js"></script>
    <script>
      
    </script>

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/select2.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/js.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/INotificacionParaUsuarioNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
