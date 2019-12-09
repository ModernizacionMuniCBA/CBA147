<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="MapaCritico.aspx.cs" Inherits="UI.MapaCritico" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedorConsulta" class=" card contenedor flex flex-main">
        <div class="contenedor-main flex  direction-vertical no-padding no-margin">
            <iframe id="iframeMapa" class="flex-main" style="border: 0; width: 100%"></iframe>        
        </div>

        <!-- Cargando -->
        <div class="cargando opaco">
            <div class="preloader-wrapper big active">
                <div class="spinner-layer">
                    <div class="circle-clipper left">
                        <div class="circle"></div>
                    </div>
                    <div class="gap-patch">
                        <div class="circle"></div>
                    </div>
                    <div class="circle-clipper right">
                        <div class="circle"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="contenedorError" class=" card contenedor">
        <label runat="server" id="msjError"></label>
    </div>

    <!-- Mi JS -->

    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/MapaCritico.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
