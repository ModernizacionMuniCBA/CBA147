<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="NotificacionSistemaConsulta.aspx.cs" Inherits="UI.NotificacionSistemaConsulta" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/NotificacionSistemaConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Card Filtros -->
        <div id="contenedorFiltros" class="card contenedor">

            <div class="contenedor-main">

                <div class="row">
                    <!-- Estado -->
                    <div id="contenedorEstado" runat="server" class="col s12 m6 l6">
                        <div class="mi-input-field">
                            <label id="lblActivo" class="no-select">Estado</label>
                            <div class="radio-buttons  flex direction-horizontal">
                                <div>
                                    <input class="with-gap" type="radio" id="rdbTodos" name="esActivo" />
                                    <label for="rdbTodos">Todos</label>
                                </div>
                                <div class="margin-left">
                                    <input class="with-gap" type="radio" id="rdbActivoSi" name="esActivo" checked />
                                    <label for="rdbActivoSi">Activo</label>
                                </div>
                                <div class="margin-left">
                                    <input class="with-gap" type="radio" id="rdbActivoNo" name="esActivo" />
                                    <label for="rdbActivoNo">De baja</label>
                                </div>
                            </div>
                            <a id="error_Activo" class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>

                    <input type="checkbox" />

                    <!-- Busqueda -->
                    <div class="col s12 m6 l6">
                        <div class="input-field">
                            <i class="material-icons prefix">search</i>
                            <input id="inputBusqueda" type="text" />
                            <label for="inputBusqueda" class="no-select">Buscar...</label>
                        </div>
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

        <!-- Grd -->
        <div id="contenedorResultado" class="flex-main no-scroll flex direction-vertical full-height">

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
                        <div class="contenedor-botones">
                            <a id="btnNuevo" class="btn colorExito"><i class="btn-icono material-icons">add</i>Nuevo</a>
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
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/NotificacionSistemaConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

