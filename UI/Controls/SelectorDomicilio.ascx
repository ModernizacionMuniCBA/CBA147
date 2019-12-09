<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectorDomicilio.ascx.cs" Inherits="UI.Controls.SelectorDomicilio" %>

<!-- Buscar Domicilio -->
<div id="SelectorDomicilio_ContenedorBusqueda" class="row">


    <div class="row">
        <div class="col s12">
            <div class="mi-input-field">
                <div class="radio-buttons">
                    <div>
                        <input class="with-gap" name="SelectorDomicilio_group1" type="radio" id="SelectorDomicilio_radio_Calle" checked />
                        <label for="SelectorDomicilio_radio_Calle"><i class="material-icons">home</i>Calle y Nro</label>
                    </div>
                    <div>
                        <input class="with-gap" name="SelectorDomicilio_group1" type="radio" id="SelectorDomicilio_radio_Barrio" />
                        <label for="SelectorDomicilio_radio_Barrio"><i class="material-icons">location_city</i>Barrio</label>
                    </div>
                    <div style="display:none">
                        <input class="with-gap" name="SelectorDomicilio_group1" type="radio" id="SelectorDomicilio_radio_Otro" />
                        <label for="SelectorDomicilio_radio_Otro">Otro</label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="flex direction-horizontal col s12 no-margin no-padding">

        <div class="flex-main row">

            <!-- Calle -->
            <div id="SelectorDomicilio_ContenedorPorCalle">

                <!-- Calle -->
                <div class="col s12 m8 l9" id="SelectorDomicilio_ContenedorCalle">
                    <div class="input-field">
                        <input id="SelectorDomicilio_Input_Calle" type="text" />
                        <label for="SelectorDomicilio_Input_Calle" class="no-select">Calle</label>
                        <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                    </div>
                </div>

                <!-- Altura -->
                <div class="col s12 m4 l3" id="SelectorDomicilio_ContenedorAltura">
                    <div class="input-field">
                        <input id="SelectorDomicilio_Input_Altura" type="text" class="validarNumericoEntero" />
                        <label for="SelectorDomicilio_Input_Altura" class="no-select">Altura</label>
                        <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                    </div>
                </div>
            </div>

            <!-- Barrio -->
            <div class="col s12" id="SelectorDomicilio_ContenedorBarrio" style="display: none;">
                <div class="input-field">
                    <input id="SelectorDomicilio_Input_Barrio" type="text" />
                    <label for="SelectorDomicilio_Input_Barrio" class="no-select">Barrio</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>



            <!-- Otro -->
            <div class="col s12" id="SelectorDomicilio_ContenedorOtro" style="display: none;">
                <div class="input-field">
                    <textarea id="SelectorDomicilio_Input_Otro" class="materialize-textarea"></textarea>
                    <label for="SelectorDomicilio_Input_Otro" class=" no-select textarea">Detalle del domicilio</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

        </div>

        <!-- Botones -->
        <div class="input-buttons" id="SelectorDomicilio_ContenedorBotones">
            <a id="SelectorDomicilio_BtnBuscar" class="btn btn-cuadrado waves-effect"><i class="material-icons">search</i></a>
        </div>
    </div>
</div>

<!-- Domicilio Seleccionado -->
<div id="SelectorDomicilio_ContenedorDomicilioSeleccionada" class="row" style="display: none">
    <div class="flex direction-horizontal col s12 no-margin no-padding">

        <div class="flex-main row">
            <div class="col s12 flex direction-vertical">
                <label class="domicilio-titulo titulo"></label>
                <label class="domicilio-detalle barrio"></label>
                <label class="domicilio-detalle cpc"></label>
            </div>

            <div class="col s12">

                <!-- Observaciones -->
                <div class="input-field" id="SelectorDomicilio_ContenedorObservaciones">
                    <textarea id="SelectorDomicilio_Input_Observaciones" class="materialize-textarea"></textarea>
                    <label for="SelectorDomicilio_Input_Observaciones" class=" no-select textarea">Detalle del domicilio</label>
                </div>
            </div>
        </div>

        <div class="input-buttons margin-left">
            <a id="SelectorDomicilio_BtnVerMapa" class="btn btn-cuadrado waves-effect"><i class="material-icons">room</i></a>
            <a id="SelectorDomicilio_BtnCancelarDomicilio" class="btn btn-cuadrado waves-effect"><i class="material-icons">close</i></a>
        </div>

    </div>

</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/SelectorDomicilio.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
