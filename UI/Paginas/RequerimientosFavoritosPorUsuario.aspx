<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientosFavoritosPorUsuario.aspx.cs" Inherits="UI.RequerimientosFavoritosPorUsuario" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientosFavoritosPorUsuario.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical no-scroll full-height">

        <div class="flex-main no-scroll flex direction-vertical full-height">

            <div id="cardResultado" class="card contenedor flex direction-vertical full-height">

                <div class="contenedor-main no-padding flex direction-vertical full-height">
                    <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                        <table id="tabla"></table>
                    </div>

                </div>

                <div class="contenedor-footer separador">
                    <div class="flex direction-horizontal">
                        <div class="tabla-footer flex-main">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientosFavoritosPorUsuario.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
