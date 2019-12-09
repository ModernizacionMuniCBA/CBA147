<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilAgregarReparacion.aspx.cs" Inherits="UI.IFrame.IMovilAgregarReparacion" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%; overflow: auto;">
        <div style="display: block">
            <div id="contenedor" class="scroll row padding full-height">
                <!-- Motivo -->
                <div class="col s12">
                    <div class="input-field fix-margin">
                        <input id="inputFormulario_Motivo" type="text" maxlength="200" required="" aria-required="true"></input>
                        <label for="inputFormulario_Motivo" class="no-select">Motivo</label>
                        <a id="errorFormulario_Motivo" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Fecha de Reparacion-->
                <div class="col s12 m4">
                    <div class="input-field fix-margin">
                        <input id="inputFechaReparacion" type="text" class="date fechaMenorQueHoy" name="date" maxlength="10" autocomplete="off" required="" aria-required="true" />
                        <label for="inputFechaReparacion" class="no-select">Fecha de reparación</label>
                        <input id="pickerFechaReparacion" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaReparacion" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Monto Reparacion-->
                <div class="col s12 m4">
                    <div class="input-field ">
                        <input id="inputFormulario_MontoReparacion" type="number" />
                        <label for="inputFormulario_MontoReparacion" class="no-select">Monto de reparación</label>
                        <a id="errorFormulario_MontoReparacion" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Taller -->
                <div class="col s12 m4">
                    <div class="input-field fix-margin">
                        <input id="inputFormulario_Taller" type="text"></input>
                        <label for="inputFormulario_Taller" class="no-select">Taller</label>
                        <a id="errorFormulario_Taller" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Observaciones -->
                <div class="col s12">
                    <div class="input-field fix-margin">
                        <textarea id="inputFormulario_Observaciones" class="materialize-textarea contador"></textarea>
                        <label for="inputFormulario_Observaciones" class="no-select">Observaciones</label>
                        <a id="errorFormulario_Observaciones" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Mi JS -->
                <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilAgregarReparacion.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
            </div>
        </div>
    </form>
</asp:Content>
