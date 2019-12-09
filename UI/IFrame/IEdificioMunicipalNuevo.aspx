<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IEdificioMunicipalNuevo.aspx.cs" Inherits="UI.IFrame.IEdificioMunicipalNuevo" %>

<%@ Register Src="~/Controls/ControlDomicilioSelector.ascx" TagName="ControlDomicilioSelector" TagPrefix="Controles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IEdificioMunicipalNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
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
            <Controles:ControlDomicilioSelector runat="server"/>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IEdificioMunicipalNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
