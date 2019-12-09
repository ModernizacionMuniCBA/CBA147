<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenAtencionCriticaDetalle.aspx.cs" Inherits="UI.IFrame.IOrdenAtencionCriticaDetalle" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlDomicilioDetalle.ascx" TagName="DomicilioDetalle" TagPrefix="Controles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenAtencionCriticaDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll padding">

        <div class="row">
            <!--Nombre -->
       <%--     <div class="col s12">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Nombre y apellido</label>
                    </div>
                    <div class="col s12   contenedor-detalle flex direction-vertical">
                        <label class="detalle" id="textoNombre" runat="server"></label>
                    </div>
                </div>
            </div>--%>

                        <!-- Nomge de usuario -->
<%--            <div class="col s12" runat="server">
                <div class="row ">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Usuario</label>
                    </div>
                    <div class="col s12  contenedor-detalle flex direction-vertical">
                        <label class="detalle" id="textoNombreUsuario" runat="server"></label>
                    </div>
                </div>

            </div>--%>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenAtencionCriticaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
