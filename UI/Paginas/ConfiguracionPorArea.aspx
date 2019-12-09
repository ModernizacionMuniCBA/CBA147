<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="ConfiguracionPorArea.aspx.cs" Inherits="UI.ConfiguracionPorArea" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/ConfiguracionPorArea.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Card Filtros -->
        <div id="cardFiltros" class="card contenedor">
            <div class="contenedor-main">
                <div class="row">
                    <div class="col s4">
                        <!-- Área -->
                        <div class="mi-input-field">
                            <label class="no-select">Área</label>
                            <select id="select_Area" style="width: 100%"></select>
                        </div>
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

        <!-- Grd -->
        <div id="contenedorConfiguraciones" class="card contenedor flex-main no-scroll flex direction-vertical full-height">
            <div class="contenido">
                <div class="configuracion" id="contenedor_tipoMotivoBandeja">
                    <label class="titulo">Tipos de motivos por defecto en Bandeja de entrada</label>
                    <div class="opciones"></div>
                </div>

                <div class="configuracion" id="contenedor_estadoCreacionOT">
                    <label class="titulo">Estado por defecto en creación de Orden de Trabajo</label>
                    <div class="opciones"></div>
                </div>
            </div>
            <!-- <div id="cardResultadoReclamos" class="card contenedor flex direction-vertical full-height"> -->
            <!-- Cargando -->

            <!-- <div class="cargando" style="display: none"> -->
            <!-- <div class="preloader-wrapper big active"> -->
            <!-- <div class="spinner-layer"> -->
            <!-- <div class="circle-clipper left"> -->
            <!-- <div class="circle"></div> -->
            <!-- </div> -->
            <!-- <div class="gap-patch"> -->
            <!-- <div class="circle"></div> -->
            <!-- </div> -->
            <!-- <div class="circle-clipper right"> -->
            <!-- <div class="circle"></div> -->
            <!-- </div> -->
            <!-- </div> -->
            <!-- </div> -->
            <!-- </div> -->
            <!-- </div> -->

            <div class="contenedor-footer separador">
                <div class="flex direction-horizontal">
                    <div class="tabla-footer flex-main">
                    </div>
                    <div class="contenedor-botones">
                        <a id="btnGuardar" class="btn waves-effect colorExito">Guardar</a>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/ConfiguracionPorArea.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
