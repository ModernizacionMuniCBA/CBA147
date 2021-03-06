﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="EstadisticaReclamoCPC.aspx.cs" Inherits="UI.EstadisticaReclamoCPC" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/EstadisticaReclamoCPC.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.charts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.theme.fusion.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/rgbcolor.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/StackBlur.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/html2canvas.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/canvg.min.js") %>"></script>


    <div class="flex direction-vertical full-height">

        <div id="cardConsulta" class="card contenedor flex direction-vertical no-scroll">

            <div class="contenedor-main">

                <!-- Ambito Trabajo -->
                <div class="row">
                    <div class="col s12 m6 l4">
                        <div class="mi-input-field">
                            <label class="no-select">Ambito</label>
                        </div>
                        <label id="textoAmbito"></label>
                    </div>
                </div>

                <!-- Area -->
                <div class="row">
                    <div class="col s12 m6 l4">
                        <div class="mi-input-field">
                            <label class="no-select">Area</label>
                            <select id="select_Area" style="width: 100%"></select>
                            <a id="errorFormulario_Area" class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>
                    <!-- SubArea -->
                    <div class="col s12 m6 l3" id="contenedorSelectSubarea" style="display: none">
                        <div class="mi-input-field">
                            <label class="no-select">Subárea</label>
                            <select id="select_Subarea" style="width: 100%"></select>
                            <a id="errorFormulario_Subarea" class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>
                </div>

                <!-- Tipo -->
                <div class="row">
                    <div class="col s12">
                        <div class="mi-input-field no-margin">
                            <label>Tipo</label>
                            <div class="radio-buttons">

                                <div>
                                    <input id="radio_Reclamo" class="with-gap" type="radio" name="consulta_groupTipo" />
                                    <label for="radio_Reclamo">Requerimientos</label>
                                </div>
                                <div>
                                    <input id="radio_RelevamientoInterno" class="with-gap" type="radio" name="consulta_groupTipo" />
                                    <label for="radio_RelevamientoInterno">Relevamiento De Oficio</label>
                                </div>
                                <div>
                                    <input id="radio_Ambos" class="with-gap" type="radio" name="consulta_groupTipo" checked />
                                    <label for="radio_Ambos">Todos</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Estados -->
                <div class="row margin-top">
                    <div class="col s12">
                        <label>Estados</label>
                        <div class="contenedorCheckboxEstados">
                            <div id="checkboxEstados">
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Fechas -->
                <div class="row margin-top">
                    <div class="col s12 m12 l9 no-padding">
                        <Controles:SelectorFecha runat="server" />
                    </div>
                </div>

                <!-- CPC -->
                <%--                <div class="row margin-top">
                    <div class="col s12">
                        <label class="no-select">CPC</label>
                        <select id="select_CPC" style="width: 100%"></select>
                    </div>
                </div>--%>
            </div>
            <!-- Footer -->
            <div class="contenedor-footer separador">
                <div class="contenedor-botones no-select">
                    <a id="btnGenerar" class="btn waves-effect no-select colorExito">Generar</a>
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

        <div id="cardEstadisticas" class="card contenedor flex-main flex full-height" style="display: none">

            <div class="no-scroll contenedor-main no-padding no-margin flex flex-main direction-vertical">
                <div id="filtros" style="padding: 5px"></div>
                <div class="row" style="width: 100%">
                    <div id="contenedor_Acciones">
                        <label id="btn_Acciones" class="link no-select">Ocultar Info</label>
                        <div class="contenido visible">
                            <div>
                                Los datos corresponden a la informacion de las areas de su incumbencia.
Informe estadístico que muestra la cantidad de requerimientos por CPC calculado según la jurisdicción de los barrios que le corresponden, considerando la ubicación asociada al requerimiento. Cabe destacar que ésta ubicación no se relaciona con el domicilio  del Contacto, aunque en ocasiones pueda ser la misma.
Considera los requerimientos creados en el período y en los estados indicados en el filtro.
Ejemplo: se ven requerimientos que ingresaron y que fueron o no atendidos, osea que hay requerimientos en estado Nuevo y Completado entre otros.
                            </div>

                        </div>
                    </div>

                </div>
                <%--EL container 1 es torta, el 2 es barra--%>
                <div id="contenedorGraficos">
                    <div id="chart-containerBarra" class="grafico"></div>
                    <div id="chart-containerTorta" class="grafico"></div>
                </div>

                <div id="contenedorGrilla" class="contenedor-main no-margin no-padding" style="display: none">
                    <div class="tabla-contenedor flex-main flex direction-vertical">
                        <table id="tablaEstadistica"></table>
                    </div>
                </div>
                <%--Meto el canvas en algun lado para iprimir la imagen--%>
                <canvas id="canvasIdBarra" style="display: none"></canvas>
                <canvas id="canvasIdRadio" style="display: none"></canvas>
                <div class="mi-input-field">
                    <div class="radio-buttons" style="justify-content: center;">
                        <div>
                            <input class="with-gap" name="estadistica_groupTipo" type="radio" id="radio_Barra" checked />
                            <label for="radio_Barra">Gráfico de barras</label>
                        </div>
                        <div>
                            <input class="with-gap" name="estadistica_groupTipo" type="radio" id="radio_Torta" />
                            <label for="radio_Torta">Gráfico Circular </label>
                        </div>
                        <div>
                            <input class="with-gap" name="estadistica_groupTipo" type="radio" id="radio_Grilla" />
                            <label for="radio_Grilla">Grilla de datos</label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <div class="contenedor-footer separador">
                <div class="contenedor-botones no-select">
                    <a id="btnVolver" class="btn waves-effect no-select colorExito"><i class="material-icons btn-icono">filter_list</i>Cambiar filtros</a>
                    <%-- <a id="btnGenerarMapaCriticidad" class="btn waves-effect no-select colorExito">Mapa Critico</a>--%>
                    <%-- <a id="btnGenerarMapa" class="btn waves-effect no-select colorExito">Mapa</a>--%>

                    <a id="btnGenerarExcel" class="btn-flat waves-effect"><i class="material-icons no-select icono_separador hide-on-med-and-down">file_download</i>
                        Descargar
                    </a>
                </div>
            </div>
        </div>


    </div>

    <!-- Mi JS -->

    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/Estadisticas/EstadisticaReclamoCPC.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
