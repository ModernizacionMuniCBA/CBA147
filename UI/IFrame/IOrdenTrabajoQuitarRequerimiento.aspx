<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoQuitarRequerimiento.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoQuitarRequerimiento" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoQuitarRequerimiento.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll">

        <div class="contenedor-main padding">

            <div class="card" id="contenedor_Ayuda">
                <i class="material-icons">help</i>
                <label>Usted esta por cambiar el estado del requerimiento seleccionado. Lo que ocasionara que salga de la orden en la que se encuentra actualmente.</label>
            </div>


            <div class="row">
                <label class="subtitulo margin-top">Nuevo estado</label>
                <!-- Estado -->
                <div class="col s12">
                    <div class="mi-input-field">
                        <label class="no-select">Estado</label>
                        <select id="select_Estado" style="width: 100%"></select>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <!-- Observaciones  -->
                <div class="col s12">
                    <div class="input-field">
                        <textarea id="input_Motivo" class="materialize-textarea"></textarea>
                        <label for="input_Motivo" class=" no-select">Motivo</label>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>

                <div class="col s12 mi-input-field ">
                    <div class="checkboxs" id="contenedorCheckboxDesmarcar">
                        <input type="checkbox" id="check_Desmarcar" />
                        <label for="check_Desmarcar">Pasar a control de área operativa</label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoQuitarRequerimiento.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
