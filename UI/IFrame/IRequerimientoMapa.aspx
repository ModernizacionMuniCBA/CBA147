<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoMapa.aspx.cs" Inherits="UI.IFrame.IRequerimientoMapa" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoMapa.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/markerclustererplus/2.1.4/markerclusterer.js"></script>

    <Controles:Mapa runat="server" />

    <div class="no-print">
        <div id="contenedor_Areas" class="card contenedor card-filtro">
            <div class="contenedor-header">
                <label class="titulo">Areas</label>
                <a class="btn-flat btn-redondo chico waves-effect btn_Cerrar"><i class="material-icons">clear</i></a>
            </div>
            <div class="contenedor-main"></div>
        </div>

        <div id="contenedor_Estados" class="card contenedor card-filtro">
            <div class="contenedor-header">
                <label class="titulo">Estados</label>
                <a class="btn-flat btn-redondo chico waves-effect btn_Cerrar"><i class="material-icons">clear</i></a>
            </div>
            <div class="contenedor-main"></div>
        </div>

        <div id="contenedor_Referencias" class="card contenedor card-filtro">
            <div class="contenedor-header">
                <label class="titulo">Referencias</label>
                <a class="btn-flat btn-redondo chico waves-effect btn_Cerrar"><i class="material-icons">clear</i></a>
            </div>
            <div class="contenedor-main"></div>
        </div>

        <a id="btn_Areas" class="btn-mapa btn-filtro">Áreas</a>
        <a id="btn_Estados" class="btn-mapa btn-filtro">Estados</a>
        <a id="btn_Referencias" class="btn-mapa btn-filtro">Referencias</a>

        <div id="template_Item" style="display: none">
            <div class="item">
                <label class="nombre"></label>
            </div>
        </div>

        <div id="template_Referencia" style="display: none">
            <div class="item">
                <svg class="svg-icon" fill="" viewBox="0 0 64 86">
                    <path d="" stroke="#000" stroke-width="2px" />
                </svg>
                <label class="nombre"></label>
            </div>
        </div>

        <div id="template_Requerimiento" style="display: none">
            <div class="requerimiento">
                <label class="numero link"></label>
                <label class="estado"></label>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoMapa.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
