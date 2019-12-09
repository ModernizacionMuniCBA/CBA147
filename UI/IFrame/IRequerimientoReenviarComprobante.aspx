<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoReenviarComprobante.aspx.cs" Inherits="UI.IFrame.IRequerimientoReenviarComprobante" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoReenviarComprobante.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenido" style="display: block">
        <form id="form">
            <div class="row" style="display: none">
                <div class="col s12 no-padding">
                    <Controles:RequerimientoDetalle runat="server" />
                </div>
            </div>

            <div class="form-separador" style="display: none"></div>

            <div class="row" id="contenedor-email">
                <div class="col s12" id="indicadorSinMail">
                    <label>El requerimiento no posee un e-mail asociado. Por favor ingrese uno:</label>
                </div>
                <!-- Usuario-->
                <div class="col s12 m6">
                    <div class="input-field">
                        <input id="input_Email" type="text" class="select-requerido email" />
                        <label for="input_Email" class="no-select">E-Mail</label>
                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoReenviarComprobante.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
