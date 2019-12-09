<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRubroMotivoPorMotivoNuevo.aspx.cs" Inherits="UI.IFrame.IRubroMotivoPorMotivoNuevo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRubroMotivoPorMotivoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="flex: 1" id="contenedor" class="scroll">

        <div class="row">
            <!-- Nombre -->
            <div class="col s8">
                <div class="input-field">
                    <input type="text" id="input_Nombre" />
                    <label for="input_Nombre">Nombre</label>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>

            <!-- Categoria-->
            <div class="col s4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Categoría</label>
                    <select id="select_Categoria" style="width: 100%"></select>

                </div>
            </div>
        </div>

        <!-- Seccion Ubicacion -->
        <div class="row">
            <div class="col s12 m6">
                <div class="row">
                    <div class="col s12 no-margin no-padding">
                        <div id="contenedor_DomicilioSeleccionado" style="display: none">
                            <div class="mapa">
                            </div>
                            <div class="contenido">
                                <label id="texto_DomicilioTitulo"></label>
                                <label id="texto_DomicilioDescripcion"></label>
                                <label id="texto_DomicilioBarrio"></label>
                                <label id="texto_DomicilioCpc"></label>
                                <a id="btn_CancelarDomicilio" class="btn waves-effect"><i class="btn-icono material-icons">clear</i>Cancelar</a>
                            </div>

                        </div>
                        <div id="contenedor_DomicilioNoSeleccionado">
                            <label>No selecciono ninguna ubicación</label>
                            <a id="btn_SeleccionarDomicilio" class="btn waves-effect colorExito"><i class="btn-icono material-icons">location_on</i>Definir ubicación</a>
                        </div>
                        <label id="errorFormulario_Domicilio" class="control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important;"></label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRubroMotivoPorMotivoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
