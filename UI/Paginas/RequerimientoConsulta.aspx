<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoConsulta.aspx.cs" Inherits="UI.RequerimientoConsulta" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/SelectorPersona.ascx" TagName="SelectorPersona" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorUsuario.ascx" TagName="SelectorUsuario" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenido">

        <div id="contenedor-busqueda" class="flex direction-vertical full-height no-scroll">

            <!-- Card Filtros avanzados -->
            <div id="cardFormularioConsultaAvanzada" class="card contenedor flex direction-vertical no-scroll">

                <div class="contenedor-main">

                    <!-- Seccion OT -->
                    <div id="contenedor_SeccionOT">
                        <div class="row">

                            <div class="col s12">
                                <div class="row" id="contenedor-areaunica-ot">
                                    <div class="col s12">
                                        <label class="titulo">Usted esta por crear una Orden de Trabajo. Recuerde que debe estar asociada a un área única.</label>
                                        <div class="form-separador"></div>
                                    </div>
                                </div>
                                <div class="row" id="contenedor-ambito">
                                    <div class="col s12">
                                        <label class="titulo">Además como su usuario está perfilado en un CPC, solo puede trabajar con requerimientos que esten asociados a el.</label>
                                        <div class="form-separador"></div>
                                    </div>
                                </div>

                                <div id="contenedor-area-ot"></div>
                            </div>
                        </div>
                    </div>

                    <div class="contenedor-contenido">
                        <%--Seccion area--%>
                        <div id="contenedor_SeccionArea">
                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo grande no-select">Área</label>
                                    </div>
                                </div>

                                <div class="col s12 m9">
                                    <div class="row">
                                        <!-- Área -->
                                        <div class="col s12 m6 l4">
                                            <div class="mi-input-field">
                                                <label class="no-select">Area</label>
                                                <select id="selectFormulario_Area" style="width: 100%"></select>
                                                <%--<a id="errorFormulario_Motivo" class="control-observacion colorTextoError no-select"></a>--%>
                                            </div>
                                        </div>

                                        <!-- Subarea -->
                                        <div id="contenedor_SeccionSubarea">
                                            <div class="col s12 m6 l4">
                                                <div class="mi-input-field">
                                                    <label class="no-select">Subarea</label>
                                                    <select id="selectFormulario_Subarea" style="width: 100%" disabled="true"></select>
                                                    <%--<a id="errorFormulario_Motivo" class="control-observacion colorTextoError no-select"></a>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <%--                                     <!-- Categoria -->
                                        <div id="contenedor_SeccionCategoria" style="display:none">
                                            <div class="col s12 m6 l4">
                                                <div class="mi-input-field">
                                                    <label class="no-select">Categoria</label>
                                                    <select id="selectFormulario_Categoria" style= "width: 100%" disabled="true" ></select>                                                   
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Seccion Motivo -->
                        <div id="contenedor_SeccionMotivo">
                            <div class="form-separador"></div>
                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo grande no-select">Servicio y Motivo</label>
                                    </div>
                                    <div class="row">
                                        <a id="btnUrgente" class="btn waves-effect waves-light ">Solo Peligrosos</a>
                                    </div>
                                </div>
                                <div class="col s12 m9 l9">
                                    <Controles:SelectorMotivo runat="server" />
                                    <label id="errorFormulario_Motivo" class="margin-left control-observacion colorTextoError no-select" style="display: none"></label>
                                </div>
                            </div>
                        </div>

                        <%--Seccion ubicacion--%>
                        <div id="contenedor_SeccionUbicacion">
                            <div class="form-separador"></div>

                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo no-select">Ubicación del Reclamo</label>
                                    </div>
                                </div>
                                <div class="col s12 m9 l9">
                                    <div class="row">
                                        <!-- Zona -->
                                        <div class="col s12 m6 l3">
                                            <div class="mi-input-field">
                                                <label class="no-select">Zona</label>
                                                <select id="select_Zona" style="width: 100%"></select>
                                                <a id="errorFormulario_Zona" class="control-observacion colorTextoError no-select"></a>
                                            </div>
                                        </div>

                                        <!-- CPC -->
                                        <div class="col s12 m6 l3" id="contenedor_CPC">
                                            <div class="mi-input-field">
                                                <label class="no-select">CPC</label>
                                                <select id="selectFormulario_CPC" style="width: 100%"></select>
                                            </div>
                                        </div>

                                        <!-- Barrio -->
                                        <div class="col s12 m12 l6">
                                            <div class="mi-input-field">
                                                <label class="no-select">Barrio</label>
                                                <select id="selectFormulario_Barrio" style="width: 100%"></select>
                                            </div>
                                        </div>
                                        <!-- Domicilio -->
                                        <div class="col s12 ">
                                            <div class="input-field">
                                                <input type="text" id="input_Domicilio" />
                                                <label for="input_Domicilio">Dirección u observaciones del domicilio</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Seccion usuario -->
                        <div id="contenedor_SeccionUsuario">

                            <div class="form-separador"></div>

                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo grande no-select">Usuario Asociado</label>
                                    </div>
                                </div>

                                <div class="col s12 m9 l9">
                                    <div class="row">
                                        <div class="col s12 no-margin no-padding">
                                            <Controles:SelectorUsuario runat="server" />
                                        </div>
                                        <a id="errorFormulario_Usuario" class="col s12 control-observacion colorTextoError no-select"></a>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Seccion Estado -->
                        <div id="contenedor_SeccionEstado">

                            <div class="form-separador"></div>

                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo no-select">Estado</label>
                                    </div>
                                </div>
                                <div class="col s12 m9 l9">
                                    <div class="row">
                                        <div class="col s12">
                                            <div class="contenedorCheckboxEstados">
                                                <div id="checkboxEstados">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Seccion Fechas -->
                        <div id="contenedor_SeccionFechas">

                            <div class="form-separador"></div>

                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo no-select">Fechas</label>
                                    </div>
                                </div>
                                <div class="col s12 m9 l9">
                                    <Controles:SelectorFecha runat="server" />
                                </div>
                            </div>
                        </div>

                        <!-- Seccion Origen-->
                        <div id="contenedor_SeccionOrigen">
                            <div class="form-separador"></div>
                            <div class="row">

                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo grande no-select">Origen</label>
                                    </div>
                                </div>

                                <div class="col s12 m9 l9">
                                    <div class="row">

                                        <!-- Origen -->
                                        <div class="col s8 ">
                                            <div class="mi-input-field">
                                                <select id="select_Origen" style="width: 100%"></select>
                                                <a id="errorFormulario_Origen" class="control-observacion colorTextoError no-select"></a>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Seccion Otros -->
                        <div class="form-separador"></div>
                        <div class="row">
                            <div class="col s12 m3 l3">
                                <div class="row">
                                    <label class="col s12 subtitulo no-select">Otros</label>
                                </div>
                            </div>
                            <div class="col s12 m9 l9">
                                <div class="row">

                                    <div class="col s12 m3 mi-input-field">
                                        <label class="no-select">Prioridad</label>
                                        <div id="checkboxPrioridades" class="checkboxs vertical">
                                            <div>
                                                <input class="with-gap" name="group1" type="checkbox" id="check_PrioridadNormal" />
                                                <label for="check_PrioridadNormal"><i class="material-icons colorTextoPrioridadNormal">flag</i>Normal</label>
                                            </div>
                                            <div>
                                                <input class="with-gap" name="group1" type="checkbox" id="check_PrioridadMedia" />
                                                <label for="check_PrioridadMedia"><i class="material-icons colorTextoPrioridadMedia">flag</i>Media</label>
                                            </div>
                                            <div>
                                                <input class="with-gap" name="group1" type="checkbox" id="check_PrioridadAlta" />
                                                <label for="check_PrioridadAlta"><i class="material-icons colorTextoPrioridadAlta">flag</i>Alta</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col s12  m3 mi-input-field">
                                        <label class="no-select">Relevamiento</label>
                                        <div class="radio-buttons vertical">
                                            <div>
                                                <input type="radio" class="with-gap" name="group1" id="radio_RelevamientoInterno_Ambos" checked />
                                                <label for="radio_RelevamientoInterno_Ambos">Ambos</label>
                                            </div>
                                            <div>
                                                <input type="radio" class="with-gap" name="group1" id="radio_RelevamientoInterno_Si" />
                                                <label for="radio_RelevamientoInterno_Si">Si</label>
                                            </div>
                                            <div>
                                                <input type="radio" class="with-gap" name="group1" id="radio_RelevamientoInterno_No" />
                                                <label for="radio_RelevamientoInterno_No">No</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col s12  m3 mi-input-field">
                                        <label class="no-select">Inspeccionado</label>
                                        <div class="radio-buttons vertical">
                                            <div>
                                                <input type="radio" class="with-gap" name="Inspeccion" id="radio_Inspeccionado_Ambos" checked />
                                                <label for="radio_Inspeccionado_Ambos">Ambos</label>
                                            </div>
                                            <div>
                                                <input type="radio" class="with-gap" name="Inspeccion" id="radio_Inspeccionado_Si" />
                                                <label for="radio_Inspeccionado_Si">Si</label>
                                            </div>
                                            <div>
                                                <input type="radio" class="with-gap" name="Inspeccion" id="radio_Inspeccionado_No" />
                                                <label for="radio_Inspeccionado_No">No</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Footer -->
                <div class="contenedor-footer separador">
                    <div class="contenedor-botones no-select">
                        <a id="btnLimpiar" class="btn waves-effect waves-light">
                            <i class="material-icons btn-icono">clear</i>
                            Limpiar Filtros</a>
                        <a class="btn waves-effect colorExito btnOk no-select">
                            <i class="material-icons btn-icono">search</i>
                            Consultar
                        </a>
                    </div>
                </div>

                <!-- Cargando -->
                <div class="cargando" style="display: none">
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
            </div>

            <!-- Resultado consulta -->
            <div id="contenedor_ResultadoConsulta" style="display: none">
                <div id="contenedor_Areas" class="contenedor">
                    <div class="encabezado">
                        <label class="titulo">Areas</label>
                        <a id="btnActualizarAreas" class="btn-flat waves-effect btn-redondo"><i class="material-icons">refresh</i></a>
                    </div>
                    <div class="contenedor-main">
                    </div>

                    <!-- Cargando -->
                    <div class="cargando opaco" style="display: none">
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
                </div>

                <div id="contenedor_TablaResultado" class="contenedor card">
                    <div class="contenedor-main flex direction-vertical no-padding">
                        <div id="contenedor_EncabezadoResultado">
                            <div class="filtrosTiposMotivos" style="display: none">
                                <div id="tipoGeneral" class="tipo btn waves-effect waves-light">
                                    <label class="nombre">General</label>
                                    <label class="cantidadRequerimientos" style="display: none">0</label>
                                </div>
                                <div id="tipoInterno" class="tipo btn waves-effect waves-light">
                                    <label class="nombre">Interno</label>
                                    <label class="cantidadRequerimientos" style="display: none">0</label>
                                </div>
                                <div id="tipoPrivado" style="display: none" class="tipo btn waves-effect waves-light">
                                    <label class="nombre">Privado</label>
                                    <label class="cantidadRequerimientos" style="display: none">0</label>
                                </div>
                            </div>
                            <div class="filtros">
                                <label id="textoFiltros"></label>
                                <%--<label id="btnCambiarFiltros">Editar filtros</label>--%>
                            </div>
                        </div>
                        <div class="tabla-contenedor flex-main flex direction-vertical">
                            <table id="tablaResultadoConsulta"></table>
                        </div>
                    </div>

                    <!-- Footer -->
                    <div class="contenedor-footer flex direction-horizontal separador">
                        <div class="tabla-footer flex-main">
                        </div>

                        <div class="contenedor-botones no-select">
                            <a class="btnExportarExcel btn waves-effect no-select tooltipped" data-tooltip="Exportar" data-position="top">
                                <i class="material-icons btn-icono no-margin">cloud_download</i>
                                <label class="hide-on-med-and-down margin-left">Exportar</label>
                            </a>

                            <a class="btnGenerarMapa btn waves-effect no-select tooltipped" data-tooltip="Mapa" data-position="top">
                                <i class="material-icons btn-icono no-margin">map</i>
                                <label class="hide-on-med-and-down margin-left">Mapa</label>
                            </a>

                            <a id="btnCambiarFiltro" class="btn waves-effect no-select colorExito"><i class="material-icons btn-icono">filter_list</i>Cambiar filtros</a>

                            <%--           <a class="btnImprimir btn waves-effect no-select tooltipped" data-tooltip="Imprimir" data-position="top">
                                <i class="material-icons btn-icono no-margin">print</i>
                                <label class="hide-on-med-and-down margin-left">Imprimir</label>
                            </a>--%>
                        </div>
                    </div>

                    <!-- Cargando -->
                    <div class="cargando opaco" style="display: none">
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
                </div>

            </div>
        </div>

        <div id="contenedor-ot">
            <div class="card contenedor">
                <label id="tituloOt">Orden de Trabajo</label>
                <label id="areaOrdenTrabajo"></label>
                <label id="cantidadRequerimientosSeleccionados"></label>

                <div class="contenedor-main">
                </div>
                <div class="contenedor-footer separador">
                    <div class="help">
                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        <div style="display: none">
                            Seleccione todos los requerimientos que desea que formen parte de una OT.<br />
                            Tenga en cuenta que los requerimientos deben ser de la MISMA AREA.<br />
                        </div>
                    </div>

                    <div class="contenedor-botones">
                        <a id="btnCancelarOt" class="btn btn-cuadrado"><i class="material-icons">clear</i></a>
                        <a id="btnOt" class="btn btn-cuadrado colorExito"><i class="material-icons">check</i></a>
                    </div>
                </div>
            </div>
        </div>

        <!-- Resultado consulta -->
        <div id="contenedor_BandejaVacia" class="contenedor" style="display: none">
            <div>
                <i class="material-icons">check_circle</i>
                <label>No hay requerimientos para solucionar</label>
            </div>
        </div>
    </div>

    <div id="contenedor_Tabla" class="contenedor-main" style="display: none">
        <div class="tabla-contenedor">
            <table id="tablaGenerarExcel"></table>
        </div>
    </div>




    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
