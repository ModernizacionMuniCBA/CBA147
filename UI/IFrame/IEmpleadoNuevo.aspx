<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IEmpleadoNuevo.aspx.cs" Inherits="UI.IFrame.IEmpleadoNuevo" %>

<%@ Register Src="~/Controls/SelectorUsuario.ascx" TagName="SelectorUsuario" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IEmpleadoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="flex: 1" id="contenedor" class="scroll">

        <div class="row">
            <!-- Area -->
            <div class="col s6"  id="contenedorSelectArea">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Area</label>
                    <select id="inputFormulario_SelectArea" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>

            <!-- Seccion -->
            <div class="col s6"  id="contenedorSelectSecciones">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Sección</label>
                    <select id="inputFormulario_SelectSeccion" style="width: 100%"></select>
                  
                </div>
            </div>
        </div>

        <div class="row">
            <!-- Usuario -->
            <div class="col s12 margin-bottom no-padding">
                <Controles:SelectorUsuario runat="server" />
            </div>
        </div>

        <div class="row" id="contenedorSelectFunciones">
            <!-- Función -->
            <div class="col s11 l4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Función</label>
                    <select id="inputFormulario_SelectFunciones" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>
            <div id="contenedor_BotonNuevaFuncion" class="col s1">
                <a id="btnNuevaFuncion" class="btn  waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Configurar funciones"><i class="material-icons">add</i></a>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IEmpleadoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
