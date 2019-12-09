<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilNuevo2.aspx.cs" Inherits="UI.IFrame.IMovilNuevo2" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%; overflow: auto;">
        <div id="contenedor" class="scroll padding full-height">
            <div class="row">
                <div class="col s6 m3">
                    <!-- Área -->
                    <div class="mi-input-field">
                        <label class="no-select">Área</label>
                        <select id="select_Area" style="width: 100%"></select>
                    </div>
                </div>
                <div class="col s6 m3 mi-input-field">
                    <!-- Tipo de movil -->
                    <label class="no-select">Tipo de móvil</label>
                    <select id="select_TipoMovil" class="select-requerido" style="width: 100%"></select>
                </div>
                <div id="contenedor_BotonNuevoTipo" class="col s6 m1">
                    <a id="btnNuevoTipo" class="btn  waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Configurar tipos"><i class="material-icons">add</i></a>
                </div>
            </div>
            <!----------->
            <!-- DATOS IDENTIFICATORIOS -->
            <!----------->
            <div class="row">
                <!-- Número Interno -->
                <div class="col s12 m3 ">
                    <div class="input-field ">
                        <input id="inputFormulario_NumeroInterno" name="NumeroInterno" type="text" required="" aria-required="true" />
                        <label for="inputFormulario_NumeroInterno" class="no-select">Número Interno</label>
                    </div>
                </div>

                <!-- Modelo -->
                <div class="col s6  m3">
                    <div class="input-field ">
                        <input id="inputFormulario_Modelo" type="text" name="modelo" required="" aria-required="true" />
                        <label for="inputFormulario_Modelo" class="no-select">Modelo</label>
                        <a id="errorFormulario_Modelo" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Marca -->
                <div class="col s6 m3">
                    <div class="input-field ">
                        <input id="inputFormulario_Marca" type="text" name="marca" required="" aria-required="true" />
                        <label for="inputFormulario_Marca" class="no-select">Marca</label>
                        <a id="errorFormulario_Marca" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Dominio -->
                <div class="col s6 m2">
                    <div class="input-field ">
                        <input id="inputFormulario_Dominio" type="text" name="dominio" required="" aria-required="true" />
                        <label for="inputFormulario_Dominio" class="no-select">Dominio</label>
                        <a id="errorFormulario_Dominio" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Año -->
                <div class="col s6 m1">
                    <div class="input-field ">
                        <input id="inputFormulario_Año" type="number" name="año" class="año" maxlength="4" required="" aria-required="true" />
                        <label for="inputFormulario_Año" class="no-select">Año</label>
                        <a id="errorFormulario_Año" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>

            <div class="row">
                <!-- Tipo de combustible -->
                <div class="col s6 m3 mi-input-field ">
                    <label class="no-select">Combustible</label>
                    <select id="select_TipoCombustible" style="width: 100%"></select>
                </div>

                <!-- Fecha de incorporaciíon -->
                <div class="col s12 m3">
                    <div class="input-field fix-margin">
                        <input id="inputFechaIncorporacion" type="text" class="date fechaMenorQueHoy" name="date" maxlength="10" autocomplete="off" />
                        <label for="inputFechaIncorporacion" class="no-select">Fecha de incorporación</label>
                        <input id="pickerFechaIncorporacion" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaIncorporacion" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Carga -->
                <div class="col s12 m3">
                    <div class="input-field ">
                        <input id="inputFormulario_Carga" type="text" name="carga" />
                        <label for="inputFormulario_Carga" class="no-select">Capacidad de carga</label>
                        <a id="errorFormulario_Carga" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Pasajeros -->
                <div class="col s12 m2">
                    <div class="input-field ">
                        <input id="inputFormulario_Asientos" type="number" />
                        <label for="inputFormulario_Asientos" class="no-select">Asientos</label>
                        <a id="errorFormulario_Asientos" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>

            <!----------->
            <!-- CARACTERISTICAS -->
            <!----------->
            <!-- Carga -->
            <div class="row" id="contenedorDatosAdicionales" style="display: none">
                <!-- Valuacion-->
                <div class="col s12 m3">
                    <div class="input-field ">
                        <input id="inputFormulario_Valuacion" type="number" />
                        <label for="inputFormulario_Valuacion" class="no-select">Valuacion</label>
                        <a id="errorFormulario_Valuacion" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Fecha de valuacion-->
                <div class="col s12 m3">
                    <div class="input-field fix-margin">
                        <input id="inputFechaValuacion" type="text" class="date fechaMenorQueHoy" name="date" maxlength="10" autocomplete="off" />
                        <label for="inputFechaValuacion" class="no-select">Fecha de valuación</label>
                        <input id="pickerFechaValuacion" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaValuacion" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Km-->
                <div class="col s12 m3">
                    <div class="input-field ">
                        <input id="inputFormulario_Km" type="number" />
                        <label for="inputFormulario_Km" class="no-select">Kilometraje</label>
                        <a id="errorFormulario_Km" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Fecha de Km-->
                <div class="col s12 m3">
                    <div class="input-field fix-margin">
                        <input id="inputFechaKilometraje" type="text" class="date fechaMenorQueHoy" name="date" maxlength="10" autocomplete="off" />
                        <label for="inputFechaKilometraje" class="no-select">Fecha de Kilometraje</label>
                        <input id="pickerFechaKilometraje" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaKilometraje" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Vencimiento ITV-->
                <div class="col s12 m3">
                    <div class="input-field fix-margin">
                        <input id="inputVencimientoITV" type="text" class="date fechaMayorQueHoy" name="date" maxlength="10" autocomplete="off" />
                        <label for="inputVencimientoITV" class="no-select">Vencimiento ITV</label>
                        <input id="pickerVencimientoITV" type="date" class="datepicker" style="display: none;" />
                        <a id="botonVencimientoITV" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Vencimiento TUV-->
                <div class="col s12 m3">
                    <div class="input-field fix-margin">
                        <input id="inputVencimientoTUV" type="text" class="date fechaMayorQueHoy" name="date" maxlength="10" autocomplete="off" />
                        <label for="inputVencimientoTUV" class="no-select">Vencimiento TUV</label>
                        <input id="pickerVencimientoTUV" type="date" class="datepicker" style="display: none;" />
                        <a id="botonVencimientoTUV" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <div class="col s6 m3">
                    <!-- Condicion -->
                    <div class="mi-input-field">
                        <label class="no-select">Condición del móvil</label>
                        <select id="select_Condicion" style="width: 100%"></select>
                        <a id="errorCondicion" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <div class="col s6 m3">
                    <!-- Estado -->
                    <div class="mi-input-field">
                        <label class="no-select">Estado</label>
                        <select id="select_Estado" style="width: 100%" class="select-requerido"></select>
                        <a id="error_SelectEstado" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Caracteristicas -->
                <div class="col s12">
                    <div class="input-field fix-margin">
                        <textarea id="inputFormulario_Caracteristicas" class="materialize-textarea contador" length="100"></textarea>
                        <label for="inputFormulario_Observaciones" class="no-select">Características</label>
                        <a id="errorFormulario_Caracteristicas" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>

            <!-- Mi JS -->
            <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilNuevo2.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
        </div>
    </form>
</asp:Content>
