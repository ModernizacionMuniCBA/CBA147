<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IEmpleadoEditarFunciones.aspx.cs" Inherits="UI.IFrame.IEmpleadoEditarFunciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IEmpleadoEditarFunciones.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="flex: 1" id="contenedor" class="scroll">
        <div class="flex direccion-horizontal" id="contenedor_Informacion">
           
             <div id="contenedor_Empleado" class="seccion flex-main">
                <label class="titulo">Empleado</label>
                <label id="texto_Empleado"></label>
            </div>

            <div id="contenedor_Area" class="seccion flex-main">
                <label class="titulo">Área</label>
                <label id="texto_Area"></label>
            </div>
        </div>
        <div class="row" id="contenedorSelectFunciones">
            <!-- Función -->
            <div class="col s12 l4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Función</label>
                    <select id="inputFormulario_SelectFunciones" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IEmpleadoEditarFunciones.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>
