<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenInspeccionNuevo.aspx.cs" Inherits="UI.IFrame.IOrdenInspeccionNuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="direction-vertical full-height padding">
        <div class="row">
            <div class="col s12">
                <div class="contenedor-detalle">
                    <label class="titulo no-select">Requerimientos seleccionados</label>
                </div>
            </div>
        </div>

        <div class="row" id="contenedor-requerimientos">
        </div>

        <div class="form-separador"></div>

        <div class="row">
            <div class="col s12">
                <div class="contenedor-detalle">
                    <label class="titulo no-select">Descripción</label>
                </div>
            </div>
        </div>

        <div class="row ">
            <div class="col s12">
                <div class="input-field fix-margin">
                    <textarea id="inputFormulario_Descripcion" class="materialize-textarea"></textarea>
                    <label for="inputFormulario_Descripcion" class=" no-select textarea">Descripción</label>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenInspeccionNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
