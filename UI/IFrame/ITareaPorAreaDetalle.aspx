<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ITareaPorAreaDetalle.aspx.cs" Inherits="UI.IFrame.ITareaPorAreaDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ITareaPorAreaDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor_Detalle">
        <div id="contenedor_Encabezado">
            <div id="contenedor_Informacion">
                <label id="texto_Nombre"></label>
                <label id="texto_Area"></label>
            </div>

            <div id="contenedor_Indicadores">
                <label id="texto_IndicadorEstado">Dado de baja</label>
            </div>
        </div>

        <div id="contenedor_Alertas">
        </div>

        <div id="contenedor_Acciones">
            <label id="btn_Acciones" class="link no-select">Ocultar acciones</label>
            <div class="contenido visible">
            </div>
        </div>

        <div id="template_Accion" style="display: none">
            <div class="accion">
                <i class="material-icons icono"></i>
                <label class="texto"></label>
            </div>
        </div>

        <div class="flex direccion-horizontal" id="contenedor_SeccionDescripcion">
            <label class="titulo">Descripción</label>
            <label id="texto_Descripcion"></label>
        </div>

        <div id="contenedor_Contenido">
            <div id="contenedor_InfoAdicional" class="seccion">
                <label class="titulo">Información adicional</label>
                <div class="linea2">
                    <label class="texto">Creado el</label>
                    <label class="textoFechaCreacion"></label>
                    <label class="texto textoUsuarioCreadorConector">por</label>
                    <label class="textoUsuarioCreador link"></label>
                </div>
                <div class="linea3">
                    <label class="texto">Fue modificado el</label>
                    <label class="textoFechaModificacion"></label>
                    <label class="texto textoUsuarioModificacionConector">por</label>
                    <label class="textoUsuarioModificacion link"></label>
                </div>
            </div>
        </div>

    </div>

    <%--Templates--%>
    <div id="template_Alerta" style="display: none">
        <div class="alerta">
            <div class="textos">
                <label class="contenido"></label>
                <label class="detalle"></label>
            </div>
            <label class="link"></label>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe-ui-default.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ITareaPorAreaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
