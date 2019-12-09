<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlSelectorRangoFecha.ascx.cs" Inherits="UI.Controls.ControlSelectorRangoFecha" ClientIDMode="Static" %>
<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Controls/Styles/ControlUbicacionFecha.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />


<div class="row" id="filaEntreFechas">
    <!-- Fecha -->
    <div id="selectorRango" class="col s12 m12 l4">
        <div class="mi-input-field">
            <label>Fecha</label>
            <select id="ControlSelectorRangoFecha_select_Fecha" style="width: 100%"></select>
            <a class="control-observacion colorTextoError  no-select"></a>
        </div>
    </div>
    <!-- Fecha desde -->
    <div class="col s12 m6 l4" id="ControlSelectorRangoFecha_ContenedorFechaDesde">
        <div class="input-field fix-margin">
            <input id="ControlSelectorRangoFecha_input_FechaDesde" type="text" class="date" />
            <label for="ControlSelectorRangoFecha_input_FechaDesde" class="no-select">Fecha desde</label>
            <input id="ControlSelectorRangoFecha_picker_FechaDesde" type="date" class="datepicker" style="display: none;" />
            <a id="ControlSelectorRangoFecha_boton_FechaDesde" class="btn-flat waves-effect boton-input">
                <i class="material-icons">today</i>
            </a>
            <a class="control-observacion colorTextoError  no-select"></a>
        </div>
    </div>
    <!-- Fecha hasta -->
    <div class="col s12 m6 l4" id="ControlSelectorRangoFecha_ContenedorFechaHasta">
        <div class="input-field fix-margin">
            <input id="ControlSelectorRangoFecha_input_FechaHasta" type="text" class="date" />
            <label for="ControlSelectorRangoFecha_input_FechaHasta" class="no-select">Fecha hasta</label>
            <input id="ControlSelectorRangoFecha_picker_FechaHasta" type="date" class="datepicker" style="display: none;" />
            <a id="ControlSelectorRangoFecha_boton_FechaHasta" class="btn-flat waves-effect boton-input">
                <i class="material-icons">today</i>
            </a>
            <a class="control-observacion colorTextoError  no-select"></a>
        </div>
    </div>
</div>
<div class="row" id="filaCheckMes">
    <div class="col checkboxs">
        <div>
            <input class="with-gap" name="group1" type="checkbox" id="check_SoloMes" />
            <label for="check_SoloMes">Solo por mes</label>
        </div>
    </div>
</div>

<div class="row" id="i_mes" style="display: none;">

  


    <!-- Año -->
    <div class="col s12 m6 l4">
        <div class="mi-input-field" id="controlSelectorRangoFecha_año">
            <label class="no-select">Año</label>
            <select id="select_Años" style="width: 100%"></select>
            <a id="errorFormulario_Años" class="control-observacion colorTextoError no-select"></a>
  
        </div>
    </div>

    
    <!-- Meses -->
    <div class="col s12 m6 l4 no-padding">
        <div class="mi-input-field">
            <label class="no-select">Mes</label>
            <select id="select_Meses" style="width: 100%"></select>
            <a id="errorFormulario_Meses" class="control-observacion colorTextoError no-select"></a>
           
        </div>
    </div>

    <%--    <div class="col s12 m6 l4">

        <div class="input-field fix-margin">

            <div class="row" id="i_mes" style="display: none;">
            </div>

        </div>
    </div>--%>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlSelectorRangoFecha.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
