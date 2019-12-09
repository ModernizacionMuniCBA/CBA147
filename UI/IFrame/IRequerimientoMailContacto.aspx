<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoMailContacto.aspx.cs" Inherits="UI.IFrame.IRequerimientoMailContacto" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoMailContacto.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenido" style="display: block">

       <div class="form-separador" style="display:none"></div>
        
        <div class="row" id="contenedor-mensaje">
            <!-- Usuario-->
            <div class="col s12">
                <div class="input-field">
                    <textarea id="input_Mensaje" class="materialize-textarea"></textarea>
                    <label for="input_Mensaje" class="no-select">Contenido del mensaje</label>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoMailContacto.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
