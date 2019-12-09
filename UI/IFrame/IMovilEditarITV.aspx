
<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilEditarITV.aspx.cs" Inherits="UI.IFrame.IMovilEditarITV" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%; overflow: auto;">
        <div style="display: block">
            <div id="contenedor" class="scroll row padding full-height">
   
                <!-- Fecha de último ITV-->
                <div class="col s12 m6">
                    <div class="input-field fix-margin">
                        <input id="inputFechaUltimoITV" type="text" class="date fechaMenorQueHoy" name="dateUltimo" maxlength="10" autocomplete="off"   />
                        <label for="inputFechaUltimoITV" class="no-select">Fecha de último ITV</label>
                        <input id="pickerFechaUltimoITV" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaUltimoITV" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Fecha de vencimiento ITV-->
                <div class="col s12 m6">
                    <div class="input-field fix-margin">
                        <input id="inputFechaVencimientoITV" type="text" class="date fechaMayorQueHoy" name="dateVencimiento" maxlength="10" autocomplete="off" required="" aria-required="true" />
                        <label for="inputFechaVencimientoITV" class="no-select">Fecha de vencimiento de ITV</label>
                        <input id="pickerFechaVencimientoITV" type="date" class="datepicker" style="display: none;" />
                        <a id="botonFechaVencimientoITV" class="btn-flat waves-effect boton-input">
                            <i class="material-icons">today</i>
                        </a>
                        <a class="control-observacion colorTextoError  no-select"></a>
                    </div>
                </div>

                <!-- Observaciones -->
                <div class="col s12">
                    <div class="input-field fix-margin">
                        <textarea id="inputFormulario_Observaciones" class="materialize-textarea contador" ></textarea>
                        <label for="inputFormulario_Observaciones" class="no-select">Observaciones</label>
                        <a id="errorFormulario_Observaciones" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Mi JS -->
                <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilEditarITV.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
            </div>
        </div>
    </form>
</asp:Content>
