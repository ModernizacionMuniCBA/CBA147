<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ITareaNueva.aspx.cs" Inherits="UI.IFrame.ITareaNueva" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ISeccionNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="display: block" class="scroll">
        <div id="contenedor" class="scroll padding full-height">
            <div class="row">
                <!-- Nombre -->
                <div class="col s12 m4 l6">
                    <div class="input-field ">
                        <input id="inputFormulario_Nombre" type="text" />
                        <label for="inputFormulario_Nombre" class="no-select">Nombre</label>
                        <a id="errorFormulario_Nombre" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>

            <div class="row">
                <!-- Observacion -->
                <div class="col s12">
                    <div class="input-field fix-margin">
                        <textarea id="inputFormulario_Observaciones" class="materialize-textarea contador" length="100"></textarea>
                        <label for="inputFormulario_Observaciones" class="no-select">Observaciones</label>
                        <a id="errorFormulario_Observacion" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>

            <div id="contenedorEstado" class="row" style="display: none">
                <!-- Activo -->
                <div class="col s12">
                    <div class="mi-input-field">
                        <label id="lblActivo" class="no-select">Activo</label>
                        <div class="radio-buttons  flex direction-horizontal">
                            <div>
                                <input class="with-gap" type="radio" id="rdbActivoSi" name="esActivo" checked />
                                <label for="rdbActivoSi">Si</label>
                            </div>
                            <div class="margin-left">
                                <input class="with-gap" type="radio" id="rdbActivoNo" name="esActivo" />
                                <label for="rdbActivoNo">No</label>
                            </div>
                        </div>
                        <a id="error_Activo" class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>


        </div>
        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ITareaNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
