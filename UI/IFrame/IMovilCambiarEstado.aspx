<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilCambiarEstado.aspx.cs" Inherits="UI.IFrame.IMovilCambiarEstado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilCambiarEstado.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll">

        <div class="contenedor-main padding">

            <div class="row">
                <label class="subtitulo margin-top">Nuevo estado</label>
                <!-- Estado -->
                <div class="col s12">
                    <div class="mi-input-field">
                        <label class="no-select">Estado</label>
                        <select id="select_Estado" style="width: 100% "></select>
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
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilCambiarEstado.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
