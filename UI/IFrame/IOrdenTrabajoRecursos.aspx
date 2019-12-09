<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoRecursos.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoRecursos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoRecursos.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="input-field">
        <input type="text" id="input_Material" />
        <label for="input_Material">Materiales</label>
    </div>

    <div class="input-field">
        <input type="text" id="input_Personal" />
        <label for="input_Personal">Personal</label>
    </div>


    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoRecursos.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
