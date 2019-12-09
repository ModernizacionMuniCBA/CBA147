<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoSelector.aspx.cs" Inherits="UI.IFrame.IRequerimientoSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<%@ Register Src="~/Controls/ControlRequerimientoSelector.ascx" TagName="RequerimientoSelector" TagPrefix="Controles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical no-scroll">

        <div class="contenedor-main padding">

            <!-- Seccion Reclamo -->
            <div class="row">
                <div class="col s12 no-padding no-margin">
                    <div class="row">
                        <div class="col s12">
                            <Controles:RequerimientoSelector runat="server" />
                            <label id="errorFormulario_Reclamo" class="margin-left control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
