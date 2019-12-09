<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IFlotaDetalle.aspx.cs" Inherits="UI.IFrame.IFlotaDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IFlotaDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor_Detalle">
        <div class="contenedor_Encabezado">

            <label class="nombre"></label>

            <div class="contenedor_Estado">
                <i class="icono material-icons">swap_vertical_circle</i>
                <label class="texto"></label>
            </div>
            <label class="fechaAlta"></label>
            <label class="fechaBaja"></label>


            <div class="contenedor_Movil">
                <label class="titulo">Móvil</label>
                <label class="numero"></label>
                <label class="tipo"></label>
                <label class="marca"></label>
                <label class="btnVerDetalle link">Ver detalle</label>
            </div>

            <div class="contenedor_Acciones">
                <div class="contenido">
                </div>
            </div>
        </div>

        <div class="contenedor_Alertas">
        </div>

        <div class="contenedor_Informacion">
            <div class="contenedor_Personal">
                <label class="titulo">Personal</label>
                <div class="contenido">
                </div>
            </div>

                <div class="contenedor_InfoAdicional" class="seccion">
                    <label class="titulo">Información adicional</label>
                    <div class="linea2">
                        <label class="texto">Creado el </label>
                        <label class="textoFechaCreacion"></label>
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
    <div id="template_Trabajos" style="display: none">
        <div class="contenedor_Listado">
            <div class="contenido">
                <div class="contenedor_Tabla">
                    <div class="tabla-contenedor">
                        <table></table>
                    </div>
                    <div class="tabla-footer">
                    </div>
                </div>
            </div>
            <div class="contenedor_Cargando panel-mensaje">
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
            </div>
            <div class="contenedor_Error panel-mensaje">
                <i class="material-icons">error_outline</i>
                <label>Error procesando la solicitud</label>
                <a class="btn waves-effect btn_Reintentar">Reintentar</a>
            </div>
        </div>
    </div>

    <div id="template_Empleado" style="display: none">
        <div class="empleado">
            <div class="persona">
                <div class="foto"></div>
                <label class="link"></label>
                <label class="observaciones"></label>
            </div>
        </div>
    </div>

    <div id="template_Accion" style="display: none">
        <div class="accion">
            <i class="material-icons icono"></i>
            <label class="texto"></label>
        </div>
    </div>

    <div id="template_Alerta" style="display: none">
        <div class="alerta">
            <div class="textos">
                <label class="contenido"></label>
                <label class="detalle"></label>
            </div>
            <label class="link"></label>
        </div>
    </div>

    <div id="template_HistorialEstados" style="display: none">
        <div id="contenedor_HistorialEstados">
        </div>
    </div>

    <div id="template_HistorialEstadoItem" style="display: none">
        <div class="estado">
            <div class="indicador">
                <div class="circulo">
                    <i class="material-icons">swap_vertical</i>
                </div>
                <div class="linea1">
                </div>
                <div class="linea2">
                </div>
            </div>

            <div class="textos">
                <label class="nombre">Nuevo</label>

                <label class="motivo">Test test test test test test 1</label>
                <div class="contenedor_Fecha">
                    <label>Por </label>
                    <label class="nombrePersona link"></label>
                    <label class="fecha"></label>
                </div>
            </div>
        </div>
    </div>

    <%--Panel Deslizable--%>
    <div id="contenedor_PanelDeslizable">
        <div class="fondo"></div>
        <div class="contenido">
            <div class="encabezado">
                <label class="titulo">Titulo seccion deslizable</label>
                <a id="btn_CerrarPanelDeslizable" class="btn-flat btn-redondo waves-effect"><i class="material-icons">close</i></a>
            </div>
            <div class="contenedor_Contenido"></div>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe-ui-default.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IFlotaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
