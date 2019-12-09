<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RubroMotivoConsulta.aspx.cs" Inherits="UI.RubroMotivoConsulta" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/SeccionConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Card Filtros -->
        <div id="cardFormularioFiltros" class="card contenedor">

            <div class="contenedor-main">

                <div class="row">
                    <!-- Grupo -->
                    <div class="col s12 m4">
                        <div class="mi-input-field">
                            <label class="no-select">Grupo</label>
                            <select id="selectFormulario_Grupo" style="width: 100%"></select>
                        </div>
                    </div>


                    <!-- Busqueda -->
                    <div class="col s12 m4" style="display:none">
                        <div class="input-field">
                            <i class="material-icons prefix">search</i>
                            <input id="inputBusqueda" type="text" />
                            <label for="inputBusqueda" class="no-select">Buscar</label>
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
                            <a id="btnNuevoGrupo" class="btn waves-effect"><i class="btn-icono material-icons">settings</i>Configurar grupos</a>
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
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RubroMotivoConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
