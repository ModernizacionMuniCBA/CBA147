<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoCambiarMotivo.aspx.cs" Inherits="UI.IFrame.IRequerimientoCambiarMotivo" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoCambiarMotivo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll full-height">

        <div class="contenedor-main padding">
            <div id="info" class="card">
                <i class="material-icons">help</i>
                <label></label>
            </div>

            <div class="row margin-top">
                <div class="col s12 no-padding">
                    <Controles:SelectorMotivo runat="server" />
                </div>
            </div>

            <div class="row" id="contenedor_Area">
                <div class="col s12">
                    <label class="titulo">Su requerimiento quedara a cargo del Área:</label>
                    <label id="texto_Area"></label>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoCambiarMotivo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
