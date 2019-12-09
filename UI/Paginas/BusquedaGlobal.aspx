<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="BusquedaGlobal.aspx.cs" Inherits="UI.BusquedaGlobal" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/BusquedaGlobal.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="contenedor_busqueda">

        <div id="contenedor_Buscando">
            <div class="preloader-wrapper big active">
                <div class="spinner-layer">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div>
                    <div class="gap-patch">
                        <div class="circle"></div>
                    </div>
                    <div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>

            <label>Buscando</label>
        </div>

        <div id="contenedor_IndicadorVacio">
            <i class="material-icons">search</i>
            <label>Sin resultados</label>
        </div>

        <div id="contenedorResultadosBusquedaGlobal">
            <div id="contenedorResultadoRequerimientoBusquedaGlobal" class="tarjeta card contenedor">
                <div class="contenedor-header">
                    <label class="titulo">Requerimientos</label>
                </div>
                <div class="contenedor-main no-margin no-padding">
                    <div class="tabla-contenedor flex-main flex direction-vertical">
                        <table id="tabla_RequerimientoBusqueda"></table>
                    </div>
                    <div class="tabla-footer">
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/BusquedaGlobal.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
