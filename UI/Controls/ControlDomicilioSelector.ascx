<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlDomicilioSelector.ascx.cs" Inherits="UI.Controls.ControlDomicilioSelector" %>

<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Controls/Styles/ControlDomicilioSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />

<!-- Switch -->
<div class="row" id="UbicacionSelector_ContenedorSwitchUbicacion" style="display: none">
    <div class="flex direccion-horizontal right">
        <div class="mi-input-field">
            <div class="switch mismo-color" id="UbicacionSelector_SwitchUbicacion">
                <div class="opcion opcion1">
                    <i class="material-icons">search</i>
                    <label>Edificios municipales</label>
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
</div>

<div id="UbicacionSelector_ContenedorBusqueda" class="visible scroll padding">
    <div id="UbicacionSelector_ContenedorInstrucciones" style="display: none">
        <label></label>
    </div>

    <div id="UbicacionSelector_ContenedorBuscar">
        <div class="input-field ">
            <input id="UbicacionSelector_InputBuscar" type="text" required="" aria-required="true" />
            <label for="UbicacionSelector_InputBuscar" class="no-select">Ingrese datos del domicilio y pulse buscar</label>
        </div>
        <div class="contenedor-boton">
            <a id="UbicacionSelector_BtnMapa" class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Seleccionar en mapa"><i class="material-icons">map</i></a>
            <a id="UbicacionSelector_BtnBuscar" class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Buscar"><i class="material-icons">search</i></a>
        </div>
    </div>
</div>

<div class="flex-main row" id="UbicacionSelector_ContenedorBusqueda_EdificioMunicipal">

    <!-- Categoria -->
    <div class="col s12 m6 l6">
        <div class="mi-input-field">
            <label class="no-select">Categoría</label>
            <select id="UbicacionSelector_SelectCategoriaEdificio" style="width: 100%">
            </select>
            <a class="control-observacion colorTextoError no-select"></a>
            <div class="input-error"><a></a></div>
        </div>
    </div>

    <!-- Edificio -->
    <div class="col s12 m6 l6">
        <div class="mi-input-field">
            <label class="no-select">Edificio Municipal</label>
            <select id="UbicacionSelector_SelectEdificio" style="width: 100%"></select>
            <div class="input-error"><a></a></div>
        </div>
    </div>
</div>

<div id="UbicacionSelector_ContenedorSugerencias" class="scroll">
</div>

<div id="UbicacionSelector_ContenedorUbicacionSeleccionada" style="display: none">
</div>

<div id="UbicacionSelector_ContenedorObservaciones" style="display: none">
    <div class="input-field">
        <textarea id="UbicacionSelector_InputObservaciones" class="materialize-textarea"></textarea>
        <label for="UbicacionSelector_InputObservaciones" class="no-select textarea">Observación del domicilio</label>
    </div>
</div>

<div id="UbicacionSelector_TemplateSugerencia" style="display: none">
    <div class="sugerencia clickable">
        <i class="material-icons"></i>
        <div class="textos">
            <label class="texto1"></label>
            <label class="texto2"></label>
        </div>

        <div class="contenedor-boton">
            <a class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Buscar"><i class="material-icons">check</i></a>
        </div>
    </div>
</div>

<div id="UbicacionSelector_TemplateUbicacionSeleccionada" style="display: none">
    <div class="ubicacion">
        <div class="textos">
            <label class="domicilio"></label>
            <label class="barrio"></label>
            <label class="cpc"></label>
            <label class="observaciones"></label>
        </div>
        <div class="cancelar">
            <i class="btn-icono material-icons">clear</i>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlDomicilioSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
