<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrigenNuevo.aspx.cs" Inherits="UI.IFrame.IOrigenNuevo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrigenNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
     <div style="flex: 1" id="contenedor" class="scroll">
        <div class="row">
            <!-- Nombre -->
            <div class="col s12 l6">
                <div class="input-field">
                    <input id="inputFormulario_Nombre" type="text" />
                    <label for="inputFormulario_Nombre" class="no-select">Nombre</label>
                    <a id="errorFormulario_Nombre" class="control-observacion colorTextoError no-select"></a>
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
                            <input class="with-gap" type="radio" id="rdbActivoSi" name="esActivo" />
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

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrigenNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
