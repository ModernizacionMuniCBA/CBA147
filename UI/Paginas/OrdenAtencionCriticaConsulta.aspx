<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Paginas/_MasterPage.Master" CodeBehind="OrdenAtencionCriticaConsulta.aspx.cs" Inherits="UI.OrdenAtencionCriticaConsulta" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link href="<%=ResolveUrl("~/Paginas/Styles/OrdenTrabajo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll ">


        <!-- Card Consulta por numero -->
        <div id="cardConsulta" class="card contenedor flex-main flex direction-vertical no-scroll">

            <div class="contenedor-main">

                     <!-- Seccion Identificacion -->
        <%--                <div id="contenedor_SeccionMotivo">
                            <div class="row">
                                <div class="col s12 m3 l3">
                                    <div class="row">
                                        <label class="col s12 subtitulo grande no-select">Servicio y Motivo</label>
                                    </div>
                                </div>
                                <div class="col s12 m9 l9">
                                    <Controles:SelectorMotivo runat="server" />
                                    <label id="errorFormulario_Motivo" class="margin-left control-observacion colorTextoError no-select" style="display: none"></label>
                                </div>
                            </div>
                        </div>--%>

                <!-- Area -->
                <div id="contenedor_Area">

                    <div class="form-separador"></div>

                    <div class="row">
                        <div class="col s12 m3 l3">
                            <div class="row">
                                <label class="col s12 subtitulo grande no-select">Area</label>
                            </div>
                        </div>
                        <div class="col s12 m9 l9">
                            <div class="row">
                                <div class="col s8 m6 l4">
                                    <div class="flex-main mi-input-field">
                                        <label class="no-select">Área</label>
                                        <select id="select_Area" style="width: 100%"></select>
                                        <a id="error_Area" class="control-observacion colorTextoError no-select" style="display: none"></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Estados -->
                <div id="contenedor_Estados">

                    <div class="form-separador"></div>

                    <div class="row">
                        <div class="col s12 m3 l3">
                            <div class="row">
                                <label class="col s12 subtitulo grande no-select">Estados</label>
                            </div>
                        </div>
                        <div class="col s12 m9 l9">
                            <div class="row">
                                <div class="col s12">
                                    <div class="contenedorCheckboxEstados flex-main">
                                        <div id="checkboxEstados" class="margin-top">
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
            </div>

            <!-- Footer -->
            <div class="contenedor-footer separador">
                <div class="contenedor-botones no-select">
                    <a class="btnLimpiarFiltros btn waves-effect no-select">
                        <i class="material-icons btn-icono">clear</i>
                        Limpiar filtros
                    </a>
                    <a class="btnBuscar btn waves-effect colorExito no-select">
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

    </div>

    <div id="cardResultado" class="card contenedor flex-main flex direction-vertical" style="display: none">
        <div class="contenedor-main flex direction-vertical no-padding">
            <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                <table id="tablaResultadoConsulta"></table>
            </div>
        </div>
        <div class="contenedor-footer separador">
            <div class="flex direction-horizontal">
                <div class="tabla-footer flex-main">
                </div>
                <div class="contenedor-botones no-select">



                    <a class="btnVolverAConsulta btn waves-effect no-select hide-on-med-and-down colorExito">
                        <i class="material-icons btn-icono">filter_list</i>
                        Cambiar Filtros
                    </a>
                    <a class="btnVolverAConsulta btn btn-cuadrado chico waves-effect no-select hide-on-large-only colorExito">
                        <i class="material-icons">filter_list</i>
                    </a>


                </div>
            </div>


        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/OrdenAtencionCriticaConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
