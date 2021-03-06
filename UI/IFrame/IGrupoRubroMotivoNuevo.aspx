﻿<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IGrupoRubroMotivoNuevo.aspx.cs" Inherits="UI.IFrame.IGrupoRubroMotivoNuevo" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ITipoMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

        <!-- Card Filtros -->
        <div id="cardFormularioFiltros">
            <div class="contenedor-main">
                <div class="row ">
                    <div class="col s6">
                        <div class="input-field fix-margin">
                            <input id="inputFormulario_Nombre" type="text" />
                            <label for="inputFormulario_Nombre" class=" no-select">Grupo de Rubros</label>
                        </div>
                    </div>
                    <div class="col s6" id="contenedorBtnAgregar">
                        <a class="btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect" data-position="bottom" data-delay="50" data-tooltip="Agregar Tipo" id="btn_agregarRubro"><i class="material-icons">add</i></a>
                    </div>
                </div>
            </div>
        </div>

    <!-- Grd -->
    <div class="flex-main no-scroll flex direction-vertical full-height">

        <div class="flex direction-vertical full-height">

            <div class="contenedor-main no-padding flex direction-vertical full-height">
                <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                    <table id="tablaRubros"></table>
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
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IGrupoRubroMotivoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
