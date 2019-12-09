<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IServicioNuevo.aspx.cs" Inherits="UI.IFrame.IServicioNuevo" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IServicioNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="display: block" class="scroll">

        <div id="contenedor_Encabezado">

            <div class="row">
                <div class="col s12 m6">
                    <div class="input-field">
                        <input type="text" id="input_Nombre" />
                        <label for="input_Nombre">Nombre</label>
                    </div>
                </div>
            </div>
        </div>


        <div id="contenedor_Detalle">

            <div class="row">
                <div class="col s12">
                    <div class="input-field">
                        <input type="text" id="input_Descripcion" />
                        <label for="input_Descripcion">Descripcion</label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col s12">
                    <div class="input-field">
                        <input type="text" id="input_Icono" />
                        <label for="input_Icono">Icono</label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col s12">
                    <div class="input-field">
                        <input type="text" id="input_UrlIcono" />
                        <label for="input_UrlIcono">UrlIcono</label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col s12">
                    <div class="input-field">
                        <input type="text" id="input_Color" />
                        <label for="input_Color">Color</label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col s12">
                    <p>
                        <input type="checkbox" id="check_Principal" />
                        <label for="check_Principal">Destacado</label>
                    </p>
                </div>
            </div>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IServicioNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
