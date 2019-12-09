<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectorMotivo.ascx.cs" Inherits="UI.Controls.SelectorMotivo" %>

<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Controls/SelectorMotivo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />



<div id="SelectorMotivo_ContenedorSeleccion">


    <!-- Switch -->
<%--    <div class="row" id="contenedorSwitch">
        <div class="flex direccion-horizontal right">
            <div class="mi-input-field">
                <div class="switch mismo-color" id="SelectorMotivo_Switch">
                    <div class="opcion opcion1">
                        <i class="material-icons">search</i>
                        <label>Busqueda rápida</label>
                    </div>

                    <label>
                        <input type="checkbox">
                        <span class="lever"></span>
                    </label>
                    <div class="opcion opcion2">
                        <i class="material-icons">touch_app</i>
                        <label>Manual</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="flex left col s6" id="SelectorMotivo_ContenedorMensajeUrgentes" style="display: none">
            <label>Solo se listarán los motivos peligrosos.</label>
        </div>
    </div>--%>

    <!-- Busqueda o seleccion -->

    <div style="min-height: 76px">
        <div class="mi-input-field" id="SelectorMotivo_ContenedorTiposMotivo" style="display:none; margin-bottom: 16px; padding-left: 6px;">
            <div class="radio-buttons horizontal">
                <div>
                    <input type="radio" class="with-gap" name="tiposMotivos" id="radio_TipoMotivo_General" checked />
                    <label for="radio_TipoMotivo_General">General</label>
                </div>
                <div>
                    <input type="radio" class="with-gap" name="tiposMotivos" id="radio_TipoMotivo_Interno" />
                    <label for="radio_TipoMotivo_Interno">Interno</label>
                </div>
                <div id="contenedor_RadioTipoMotivo_Privado" style="display:none">
                    <input type="radio" class="with-gap" name="tiposMotivos" id="radio_TipoMotivo_Privado"  />
                    <label for="radio_TipoMotivo_Privado">Privado</label>
                </div>
            </div>
        </div>
        <!-- Busqueda -->
        <div id="SelectorMotivo_ContenedorBusqueda" class="row" style="display: none">
            <div class="col s12">
                <div class="input-field fix-margin">
                    <input id="SelectorMotivo_Input_Buscar" type="text" />
                    <label for="SelectorMotivo_Input_Buscar" class="no-select">Buscar motivo</label>
                </div>
            </div>
        </div>

        <!-- Seleccion -->
        <div class="row " id="SelectorMotivo_ContenedorManual" >
            <div class="col s12 no-margin no-padding">

                <div class="flex direction-horizontal">

                    <div class="flex-main row" style="display: flex; padding: 0 0.75rem">

                        <!-- Servicio -->
                        <div class="mi-input-field" style="flex: 1; padding-right: 0.75rem"  id="SelectorMotivo_Contenedor_Select_Servicio">
                            <label class="no-select">Servicio</label>
                            <select id="SelectorMotivo_Select_Servicio" style="width: 100%"></select>
                            <a class="control-observacion colorTextoError no-select"></a>
                        </div>

                        <!-- Area -->
                        <div class="mi-input-field" style="display: none; flex: 1; padding-right: 0.75rem" id="SelectorMotivo_Contenedor_Select_Area">
                            <label class="no-select">Área</label>
                            <select id="SelectorMotivo_Select_Area" style="width: 100%"></select>
                            <a class="control-observacion colorTextoError no-select"></a>
                        </div>

                        <!-- Categoria -->
                        <div class="mi-input-field" style="display: none; flex: 1; padding-right: 0.75rem" id="SelectorMotivo_Contenedor_Select_Categoria">
                            <label class="no-select">Categoría</label>
                            <select id="SelectorMotivo_Select_Categoria" style="width: 100%"></select>
                            <a class="control-observacion colorTextoError no-select"></a>
                        </div>

                        <!-- Motivo -->
                        <div class="mi-input-field" style="flex: 2;">
                            <label class="no-select">Motivo</label>
                            <select id="SelectorMotivo_Select_Motivo" style="width: 100%"></select>
                            <a class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</div>

<!-- Seleccionado -->
<div id="SelectorMotivo_ContenedorMotivoSeleccionado" style="display: none">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">

            <div class="col s12 flex direction-vertical">
                <label class="motivo-servicio"></label>
                <label class="motivo-area"></label>
                <label class="motivo-categoria"></label>
                <label class="motivo-nombre"></label>

                <label class="motivo-descripcion"></label>
            </div>

        </div>
        <div class="input-buttons margin-left">
            <a id="SelectorMotivo_BtnCancelarMotivo" class="btn btn-cuadrado waves-effect waves-light"><i class="material-icons">close</i></a>
        </div>
    </div>
</div>

<!-- Card flotante -->
<div id="SelectorMotivo_Card" class="card" style="display: none">
    <div class="contenido" style="overflow-y: auto">
        <table></table>
    </div>
</div>


<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/SelectorMotivo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
