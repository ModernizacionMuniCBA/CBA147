<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="EmpleadoConsulta.aspx.cs" Inherits="UI.EmpleadoConsulta" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/EmpleadoConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Card Filtros -->
        <div id="cardFormularioFiltros" class="card contenedor">

            <div class="contenedor-main">

                <div class="row">
                    <!-- Área -->
                    <div class="col s12 m3" id="contenedor-Area">
                        <div class="mi-input-field">
                            <label class="no-select">Área</label>
                            <select id="selectFormulario_Area" style="width: 100%"></select>
                        </div>
                    </div>

                    <%--                    <div class="col s12 m6 l2">
                        <div class="mi-input-field">
                            <label class="no-select">Estado</label>
                            <select id="selectFormulario_Estado" style="width: 100%"></select>
                        </div>
                    </div>--%>

                    <div class="col s12 m2" id="contenedorCheckDeBaja">
                        <input class="with-gap" name="group1" type="checkbox" id="check_IncluirDeBaja" />
                        <label for="check_IncluirDeBaja">Incluir de baja</label>
                    </div>

                    <div class="col s12 m4 l3">
                        <div class="row">

                            <div class="mi-input-field col s10">
                                <label class="no-select">Función</label>
                                <select id="selectFormulario_Funcion" style="width: 100%"></select>
                                <a class="control-observacion colorTextoError no-select"></a>
                            </div>

                            <div id="contenedor_BotonNuevaFuncion" class="col s1">
                                <a class="btn btnNuevaFuncion waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Configurar funciones"><i class="material-icons">add</i></a>
                            </div>

                        </div>
                    </div>

                    <!-- Busqueda -->
                    <div class="col s12 m4 " id="contenedor-Busqueda">
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
                            <a id="btnNuevo" class="btn waves-effect colorExito"><i class="btn-icono material-icons">add</i>Nuevo</a>
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
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/EmpleadoConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
