<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilEditarKilometraje.aspx.cs" Inherits="UI.IFrame.IMovilEditarKilometraje" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%; overflow: auto;">
        <div style="display: block">
            <div id="contenedor" class="scroll row padding full-height">
                <!-- Kilometraje-->
                <div class="col s12 m6">
                    <div class="input-field ">
                        <input id="inputFormulario_Kilometraje" type="number" required="" aria-required="true" />
                        <label for="inputFormulario_Kilometraje" class="no-select">Kilometraje</label>
                        <a id="errorFormulario_Kilometraje" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Fecha de kilometraje-->
                <div class="col s12 m6">
                    <div class="input-field fix-margin">
                        <input id="inputFechaKilometraje" type="text" class="date fechaMenorQueHoy" name="date" maxlength="10" autocomplete="off" required="" aria-required="true" />
                        <label for="inputFechaKilometraje" class="no-select">Fecha de kilometraje</label>
                        <input id="pickerFechaKilometraje" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaKilometraje" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
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
                <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilEditarKilometraje.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
            </div>
        </div>
    </form>
</asp:Content>
