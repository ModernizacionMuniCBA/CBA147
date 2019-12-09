<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoTopBarrios.aspx.cs" Inherits="UI.Paginas.RequerimientoTopBarrios" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoTopBarrios.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/markerclustererplus/2.1.4/markerclusterer.js"></script>


    <div id="contenedor_Encabezado">
        <div id="contenedor_Info" class="card" style="display: none">
            <i class="material-icons">info_outline</i>
            <label>Como usted esta perfilado en el CPC 3 - Arguello solo ve requerimientos asociados a este CPC</label>
        </div>
        <div id="contenedor_Filtros">
            <a id="btn_Filtros" class="btn-flat chico waves-effect"><i class="material-icons btn-icono">filter_list</i><label>Filtrar</label></a>
        </div>
    </div>
    <div id="contenedor_FiltrosContenido">
        <div class="filtros">
            <div class="mi-input-field">
                <label>Area</label>
                <select id="selectFiltro_Area" style="width: 100%" name="select_Area"></select>
            </div>
            <div class="mi-input-field">
                <label>Categoría</label>
                <select id="selectFiltro_Categoria" style="width: 100%" name="select_Categoria"></select>
            </div>
            <div class="mi-input-field">
                <label>Zona</label>
                <select id="selectFiltro_Zona" style="width: 100%" name="select_Zona"></select>
            </div>
            <div class="mi-input-field">
                <label>Cantidad</label>
                <select id="selectFiltro_Cantidad" style="width: 100%" name="select_Cantidad"></select>
            </div>
        </div>

        <a id="btn_Filtrar" class="btn btn-redondo waves-effect chico"><i class="material-icons">search</i></a>
    </div>


    <div id="contenedor_Formulario">
        <div id="contenedor_Mapa" class="card">
            <Controles:Mapa ID="mapa" runat="server" />
        </div>

        <div id="contenedor_Data">
        </div>

    </div>


    <div id="template_Barrio" style="display: none">
        <div class="data-barrio">
            <div class="encabezado">
                <label class="numero"></label>
                <label class="barrio"></label>
                <label class="cantidad"></label>
            </div>
        </div>

    </div>


    <div id="template_BarrioArea" style="display: none">
        <div class="area" onclick="onClickPopupArea(this)">
            <label class="nombre"></label>
            <label class="cantidad"></label>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoTopBarrios.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>


</asp:Content>
