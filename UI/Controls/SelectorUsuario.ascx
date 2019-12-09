<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectorUsuario.ascx.cs" Inherits="UI.Controls.SelectorUsuario" %>
<!-- Busqueda persona -->
<div class="row" id="SelectorUsuario_ContenedorBusqueda">
    <div class="flex direction-horizontal col s12 no-margin no-padding">
        <div class="flex-main row">
            <!-- Tipo -->
            <div class="col s12 m3 l3" id="SelectorUsuario_ContenedorTipo">
                <div class="mi-input-field">
                    <label class="no-select" style="opacity: 0">Tipo</label>
                    <select id="SelectorUsuario_Select_Tipo" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

            <%--            <!-- Nro de Documento -->
            <div class="col s12 m6 l6" id="SelectorUsuario_ContenedorNumeroDocumento">

                <div class="input-field fix-margin">
                    <input id="SelectorUsuario_Input_NumeroDocumento" type="text" />
                    <label for="SelectorUsuario_Input_NumeroDocumento" class="no-select">Nro Documento</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>--%>

            <!-- Nombre y Apellido -->
            <div class="col s12 m9 l9" id="SelectorUsuario_ContenedorNombre">

                <div class="row">
                    <div class="col s12 m6 l6">
                        <div class="input-field fix-margin">
                            <input id="SelectorUsuario_Input_Nombre" type="text" />
                            <label for="SelectorUsuario_Input_Nombre" class="no-select">Nombre</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                    <div class="col s12 m6 l6">
                        <div class="input-field fix-margin">
                            <input id="SelectorUsuario_Input_Apellido" type="text" />
                            <label for="SelectorUsuario_Input_Apellido" class="no-select">Apellido</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Username -->
            <div class="col s12 m6 l6" id="SelectorUsuario_ContenedorUsername" style="display: none">

                <div class="input-field fix-margin">
                    <input id="SelectorUsuario_Input_Username" type="text" />
                    <label for="SelectorUsuario_Input_Username" class="no-select">Usuario</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

            <!-- E-Mail -->
            <div class="col s12 m6 l6" id="SelectorUsuario_ContenedorEmail" style="display: none">

                <div class="input-field fix-margin">
                    <input id="SelectorUsuario_Input_Email" type="text" />
                    <label for="SelectorUsuario_Input_Email" class="no-select">E-mail</label>
                    <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                </div>
            </div>

        </div>

        <!-- Botones -->
        <div class="input-buttons margin-top margin-left">
            <a id="SelectorUsuario_BtnBuscar" class="btn btn-cuadrado waves-effect tooltipped" data-position="bottom" data-delay="50" data-tooltip="Buscar"><i class="material-icons">search</i></a>
            <a id="SelectorUsuario_BtnNuevoUsuario" class="btn  waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Agregar usuario"><i class="material-icons">add</i></a>
        </div>
    </div>

</div>

<!-- Persona Seleccionada -->
<div class="row" id="SelectorUsuario_ContenedorUsuarioSeleccionado" style="display: none;">
    <div class="flex direction-horizontal col s12 no-margin">
        <div class="flex-main row">

            <div class="entity-detalle">
                <label class="titulo"></label>
                <label class="detalle"></label>
            </div>


        </div>
        <div class="input-buttons margin-left">
            <a id="SelectorUsuario_BtnDetalleUsuario" class="btn btn-cuadrado waves-effect"><i class="material-icons">description</i></a>
            <a id="SelectorUsuario_BtnCancelarUsuario" class="btn btn-cuadrado waves-effect"><i class="material-icons">close</i></a>
        </div>
    </div>

</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/SelectorUsuario.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
