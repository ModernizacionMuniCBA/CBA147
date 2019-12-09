<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IFuncionNueva.aspx.cs" Inherits="UI.IFrame.IFuncionNueva" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ITipoMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Card Filtros -->
    <div id="cardFormularioFiltros">
        <div class="contenedor-main">
            <div class="row ">
                <div class="col s5" id="contenedor_Areas" style="display:none">
                    <div class="mi-input-field">
                        <label class="no-select">Área</label>
                        <select id="select_Area" style="width: 100%"></select>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
                <div class="col s6">
                    <div class="input-field fix-margin">
                        <input id="inputFormulario_Nombre" type="text" />
                        <label for="inputFormulario_Nombre" class=" no-select">Función</label>
                    </div>
                </div>
                <div class="col s1" id="contenedorBtnAgregar">
                    <a class="btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect" data-position="bottom" data-delay="50" data-tooltip="Agregar Función" id="btn_agregar"><i class="material-icons">add</i></a>
                </div>
            </div>
        </div>
    </div>

    <!-- Grd -->
    <div id="contenedorResultado" class="flex-main no-scroll flex direction-vertical full-height">

        <div id="cardResultado" class=" flex direction-vertical full-height">

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
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IFuncionNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
