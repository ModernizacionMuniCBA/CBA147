<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="UI.Inicio" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/Inicio.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div class="contenedor" style="display: block">

        <div class="row" style="display: none">
            <div class="col s12 no-padding">
                <div id="cardBusqueda" class="card flex direction-horizontal no-margin">
                    <i class="material-icons">search</i>
                    <input id="input_Busqueda" type="text" autocomplete="off" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col s12 no-padding">
                <div class="card flex-main no-margin" id="cardUltimosReclamos" style="opacity: 0">
                    <div class="contenedor no-margin">
                        <div class="contenedor-header">
                            <label class="titulo">Últimos requerimientos generales</label>
                        </div>
                        <div class="contenedor-main no-margin no-padding">
                            <div class="tabla-contenedor flex-main flex direction-vertical">
                                <table id="tablaRequerimiento"></table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row margin-top">
            <div class="col s6 no-padding ">
                <div class="card flex-main no-margin margin-right" id="cardRQPeligrososAntiguos" style="opacity: 0">
                    <div class="contenedor no-margin">
                        <div class="contenedor-header">
                             <i id="estadoTituloRQPeligrosos" class="material-icons  margin-right" >swap_vertical_circle</i><label class="titulo">Requerimientos Peligrosos Nuevos</label>
                        </div>
                        <div class="contenedor-main no-margin no-padding">
                            <div class="tabla-contenedor flex-main flex direction-vertical">
                                <table id="tablaRQPeligrososAntiguos"></table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col s6 no-padding">
                <div class="card flex-main no-margin" id="cardOrdenesAntiguas" style="opacity: 0">
                    <div class="contenedor no-margin">
                        <div class="contenedor-header">
                            <i id="estadoTituloOrdenes" class="material-icons  margin-right" >swap_vertical_circle</i> <label class="titulo"></label>
                        </div>
                        <div class="contenedor-main no-margin no-padding">
                            <div class="tabla-contenedor flex-main flex direction-vertical">
                                <table id="tablaOrdenAntiguas"></table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="contenedorResultadoBusqueda" class="flex-main card contenedor no-margin" style="display: none">

        <div class="contenedor-main flex direction-vertical no-padding">
            <div class="tabla-contenedor flex-main flex direction-vertical">
                <table id="tablaResultadoBusqueda"></table>
            </div>
        </div>

        <!-- Footer -->
        <div class="contenedor-footer flex direction-horizontal separador">
            <div class="tabla-footer flex-main">
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


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/Inicio.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

