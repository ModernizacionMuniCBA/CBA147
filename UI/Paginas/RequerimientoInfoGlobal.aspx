<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoInfoGlobal.aspx.cs" Inherits="UI.Paginas.RequerimientoInfoGlobal" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoInfoGlobal.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorDomicilio.ascx" TagName="SelectorDomicilio" TagPrefix="Controles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div id="contenedor_Cargando">

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

        <label class="texto1">Generando informe...</label>
        <label class="texto2">Esta operacion puede demorar un tiempo</label>
    </div>

    <div id="contenedor_Tabla" class="card contenedor">

        <%--Content--%>
        <div class="contenedor-main">
            <div class="tabla-contenedor">
                <table id="tabla"></table>
            </div>


        </div>

        <!-- Footer -->
        <div class="contenedor-footer separador">
            <div class="tabla-footer">
            </div>
            <div class="contenedor-botones  row">
                <a id="btn_Exportar" class="btn waves-effect waves-light">Exportar</a>
            </div>
        </div>
    </div>

    <div id="contenedor_Resultado">
        <i class="material-icons">check_circle</i>
        <label>Reporte generado correctamente</label>
        <div class="contenedor_Botones">
            <a id="btn_ExportarExcel" class="btn waves-effect"><i class="btn-icono material-icons">file_download</i>Excel</a>
            <%--<a id="btn_ExportarPdf" class="btn waves-effect"><i class="btn-icono material-icons">file_download</i>PDF</a>--%>
        </div>
    </div>

    <div id="contenedor_Error">
        <i class="material-icons">error_outline</i>
        <label></label>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoInfoGlobal.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>


</asp:Content>
