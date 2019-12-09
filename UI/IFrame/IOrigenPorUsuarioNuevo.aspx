<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrigenPorUsuarioNuevo.aspx.cs" Inherits="UI.IFrame.IOrigenPorUsuarioNuevo" %>

<%@ Register Src="~/Controls/SelectorUsuario.ascx" TagName="SelectorUsuario" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrigenPorUsuarioNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="flex: 1" id="contenedor" class="scroll">
        <div class="row">
            <!-- Area -->
            <div class="col s12 m6 no-margin no-padding">
                <Controles:SelectorUsuario runat="server" />
            </div>
        </div>

        <div class="row" id="contenedorSelectOrigen">
            <!-- Origen -->
            <div class="col s12 m6 l4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Origen</label>
                    <select id="inputFormulario_SelectOrigen" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrigenPorUsuarioNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
