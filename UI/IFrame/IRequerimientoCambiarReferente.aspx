<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoCambiarReferente.aspx.cs" Inherits="UI.IFrame.IRequerimientoCambiarReferente" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorUsuario.ascx" TagName="SelectorUsuario" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoCambiarReferente.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll">

        <div class="contenedor-main padding">

            <div class="row">
                <label class="subtitulo">Datos de contacto</label>
                <div class="col s12 no-padding">
                    <Controles:SelectorUsuario runat="server" />
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoCambiarReferente.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
