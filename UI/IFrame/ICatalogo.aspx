<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ICatalogo.aspx.cs" Inherits="UI.IFrame.ICatalogo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ICatalogo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" /> --%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="display: block" class="scroll padding">

        <div id="contenedor" class="scroll padding full-height">
            <div class="row">
                <!-- Servicios -->
                <div class="col s12 m4">
                    <div class="mi-input-field">
                        <label class="no-select">Servicio</label>
                        <select id="select_Servicios" style="width: 100%"></select>
                    </div>
                </div>

                <!-- Documents -->
                <div id="document" class="col s12" style="display: none">
                    <div id="motivos" class="waves-effect" style="padding: 10px; cursor: pointer; text-align: center;">
                        <div>
                            <img style="width: 128px;" src="<%=ResolveUrl("~/Resources/Imagenes/file_pdf.png") %>" />
                        </div>
                        <div style="width: 160px;">
                            <label style="font-weight: 700;"></label>
                        </div>
                    </div>

                    <div id="usuarios" class="waves-effect" style="padding: 10px; cursor: pointer; text-align: center;">
                        <div>
                            <img style="width: 128px;" src="<%=ResolveUrl("~/Resources/Imagenes/file_pdf.png") %>" />
                        </div>
                        <div style="width: 160px;">
                            <label style="font-weight: 700;"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ICatalogo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>