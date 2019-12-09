<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoMovilNuevo.aspx.cs" Inherits="UI.TipoMovilNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="<%=ResolveUrl("~/Paginas/Styles/TipoMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll">

        <!-- Card Filtros -->
        <div id="cardFormularioFiltros" class="card contenedor">
            <div class="contenedor-main">
                <div class="row ">
                    <div class="col s6">
                        <div class="input-field fix-margin">
                            <input id="inputFormulario_Nombre" type="text" />
                            <label for="inputFormulario_Nombre" class=" no-select">Tipo Móvil</label>
                        </div>
                    </div>
                    <div class="col s6" id="contenedorBtnAgregarTipo">
                        <a class="btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect" data-position="bottom" data-delay="50" data-tooltip="Agregar Tipo" id="btn_agregarTipo"><i class="material-icons">add</i></a>
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
    <div id="contenedorResultadoTipos" class="flex-main no-scroll flex direction-vertical full-height">

        <div id="cardResultadoTipos" class="card contenedor flex direction-vertical full-height">

            <div class="contenedor-main no-padding flex direction-vertical full-height">
                <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                    <table id="tabla"></table>
                </div>

            </div>

            <div class="contenedor-footer separador">
                <div class="flex direction-horizontal">
                    <div class="tabla-footer flex-main">
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

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/TipoMovilNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
