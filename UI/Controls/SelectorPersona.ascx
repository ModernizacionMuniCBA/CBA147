<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectorPersona.ascx.cs" Inherits="UI.Controls.SelectorPersona" %>
<!-- Busqueda persona -->
<div class="row" id="SelectorPersona_ContenedorBusqueda">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">
            <!-- Tipo Persona-->
            <div class="hide">

                <div class="col s12 l3 l3">
                    <div class="mi-input-field">
                        <label class="no-select">Persona</label>
                        <select id="SelectorPersona_Select_TipoPersona" style="width: 100%"></select>
                        <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                    </div>
                </div>
            </div>

            <!-- Tipo Documento-->
            <div class="col s12 m3 l3" id="SelectorPersona_ContenedorTipo">
                <div class="mi-input-field">
                    <label class="no-select" style="opacity:0">Tipo</label>
                    <select id="SelectorPersona_Select_Tipo" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>


            <!-- Nro de Documento -->
            <div class="col s12 m6 l6" id="SelectorPersona_ContenedorNumero">

                <div class="input-field fix-margin">
                    <input id="SelectorPersona_Input_NumeroDocumento" type="text" />
                    <label for="SelectorPersona_Input_NumeroDocumento" class="no-select">Nro Documento</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

            <!-- Nombre y Apellido -->
            <div class="col s12 m9 l9" id="SelectorPersona_ContenedorNombre" style="display:none">

                <div class="row">
                    <div class="col s12 m6 l6">
                        <div class="input-field fix-margin">
                            <input id="SelectorPersona_Input_Nombre" type="text" />
                            <label for="SelectorPersona_Input_Nombre" class="no-select">Nombre</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                    <div class="col s12 m6 l6">
                         <div class="input-field fix-margin">
                            <input id="SelectorPersona_Input_Apellido" type="text" />
                            <label for="SelectorPersona_Input_Apellido" class="no-select">Apellido</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                </div>

            </div>

        </div>

        <!-- Botones -->
        <div class="input-buttons margin-top margin-left">
            <a id="SelectorPersona_BtnBuscar" class="btn btn-cuadrado waves-effect tooltipped" data-position="bottom" data-delay="50" data-tooltip="Buscar"><i class="material-icons">search</i></a>
            <a id="SelectorPersona_BtnNuevaPersonaFisica" class="btn  waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Agregar persona física"><i class="material-icons">person_add</i></a>
        </div>
    </div>

</div>

<!-- Persona Seleccionada -->
<div class="row" id="SelectorPersona_ContenedorPersonaSeleccionada" style="display: none;">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">

            <div class="col s12 flex direction-vertical">
                <label class="persona-nombre"></label>
                <label class="persona-detalle"></label>
            </div>

        </div>
        <div class="input-buttons margin-left">
            <a id="SelectorPersona_BtnDetallePersona" class="btn btn-cuadrado waves-effect"><i class="material-icons">description</i></a>
            <a id="SelectorPersona_BtnCancelarPersona" class="btn btn-cuadrado waves-effect"><i class="material-icons">close</i></a>
        </div>
    </div>

</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/SelectorPersona.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
