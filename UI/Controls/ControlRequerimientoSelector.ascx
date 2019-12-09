<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlRequerimientoSelector.ascx.cs" Inherits="UI.Controls.ControlRequerimientoSelector" %>

<!-- Busqueda requerimiento -->
<div class="row" id="RequerimientoSelector_ContenedorBusqueda">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">
            <!-- Nro de Requerimiento -->
            <div class="col s8">

                <div class="input-field fix-margin">
                    <input id="RequerimientoSelector_Input_NumeroRequerimiento" type="text" />
                    <label for="RequerimientoSelector_Input_NumeroRequerimiento" class="no-select">Nro Requerimiento</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>
            <div class="col s4">

                <div class="input-field fix-margin">
                    <input id="RequerimientoSelector_Input_Anio" type="text" />
                    <label for="RequerimientoSelector_Input_Anio" class="no-select">Año</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

        </div>

        <!-- Botones -->
        <div class="input-buttons margin-top margin-left">
            <a id="RequerimientoSelector_BtnBuscar" class="btn btn-cuadrado waves-effect tooltipped" data-position="bottom" data-delay="50" data-tooltip="Buscar"><i class="material-icons">search</i></a>
        </div>

        <!-- Resultados-->
        <div id="RequerimientoSelector_ContenedorTabla" class="row" style="display: none">
            <div class="flex-main flex direction-vertical">
                <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                    <table id="RequerimientoSelector_Tabla"></table>
                </div>
                <div class="tabla-footer">
                </div>
            </div>
        </div>

    </div>

</div>

<!-- Requerimiento Seleccionado -->
<div class="row" id="RequerimientoSelector_ContenedorRequerimientoSeleccionado" style="display: none;">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">

            <div class="col s12 flex direction-vertical">
                <label class="requerimiento-titulo"></label>
                <label class="requerimiento-detalle"></label>
            </div>

        </div>
        <div class="input-buttons margin-left">
            <a id="RequerimientoSelector_BtnDetalleRequerimiento" class="btn btn-cuadrado waves-effect"><i class="material-icons">description</i></a>
            <a id="RequerimientoSelector_BtnCancelarRequerimiento" class="btn btn-cuadrado waves-effect"><i class="material-icons">close</i></a>
        </div>
    </div>

</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlRequerimientoSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
