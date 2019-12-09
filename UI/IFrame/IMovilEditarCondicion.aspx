<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilEditarCondicion.aspx.cs" Inherits="UI.IFrame.IMovilEditarCondicion" %>

<%@ Register Src="~/Controls/ControlRequerimientoDetalle.ascx" TagName="RequerimientoDetalle" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilEditarCondicion.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll">

        <div class="contenedor-main padding">

            <div class="row">
                <!-- Condicion -->
                <div class="col s12">
                    <div class="mi-input-field">
                        <label class="no-select">Condición</label>
                        <select id="select_Condicion" style="width: 100% "></select>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilEditarCondicion.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
