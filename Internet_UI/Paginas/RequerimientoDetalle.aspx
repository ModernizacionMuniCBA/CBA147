<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="RequerimientoDetalle.aspx.cs" Inherits="Internet_UI.Paginas.RequerimientoDetalle" %>


<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Detalle de requerimiento</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Descripcion --%>
    <div class="contenedor-detalle">
        <label>Datos básicos</label>
        <div id="card_Descripcion" class="card card-detalle">
            <div class="content-header">
                <label class="_Numero"></label>
            </div>

            <div class="content-body">
                <div class="info">
                    <strong>Servicio</strong>
                    <label class="_Servicio"></label>
                </div>

                <div class="info">
                    <strong>Motivo</strong>
                    <label class="_Motivo"></label>
                </div>

                <div class="info">
                    <strong>Descripción</strong>
                    <label class="_Descripcion"></label>
                </div>
            </div>
        </div>
    </div>


    <%-- Estado --%>
    <div class="contenedor-detalle">
        <label>Último estado</label>
        <div id="card_Estado" class="card card-detalle">
            <div class="content-header">
                <i class="_EstadoColor mdi mdi-circle"></i>
                <label class="_EstadoNombre"></label>
            </div>

            <div class="content-body">
                <div class="info">
                    <strong>Fecha</strong>
                    <label class="_EstadoFecha"></label>
                </div>
                <div class="info">
                    <strong>Observaciones del estado</strong>
                    <label class="_EstadoObservacion"></label>
                </div>
            </div>
        </div>
    </div>


    <%-- Ubicación --%>
    <div class="contenedor-detalle">
        <label>Ubicación</label>
        <div id="card_Ubicacion" class="card card-detalle">
            <div class="mapa"></div>

            <div class="content-body">
                <div class="info">
                    <strong>Dirección</strong>
                    <label class="_Direccion"></label>
                </div>

                <div class="info">
                    <strong>Observaciones del domicilio</strong>
                    <label class="_Observaciones"></label>
                </div>

                <div class="info">
                    <strong>CPC</strong>
                    <label class="_Cpc"></label>
                </div>

                <div class="info">
                    <strong>Barrio</strong>
                    <label class="_Barrio"></label>
                </div>
            </div>

            <div class="content-footer">
                <a id="btn_AbrirMapa" class="btn waves-effect">Abrir mapa</a>
            </div>
        </div>
    </div>


    <%-- Imágenes --%>
    <div id="contenedor_Imagenes" class="contenedor-detalle" style="display: none;">
        <label>Imágenes</label>
        <div id="card_Imagenes" class="card card-detalle scroll">
            <div class="content-body">
            </div>
        </div>
    </div>


    <%-- Area encargada --%>
    <div class="contenedor-detalle">
        <label>Area encargada</label>
        <div id="card_AreaEncargada" class="card card-detalle">
            <div class="content-body">
                <div class="info">
                    <strong>Secretaría</strong>
                    <label class="_OrganicaSecretaria"></label>
                </div>

                <div class="info">
                    <strong>Dirección</strong>
                    <label class="_OrganicaDireccion"></label>
                </div>

                <div class="info">
                    <strong>Area</strong>
                    <label class="_OrganicaArea"></label>
                </div>

                <div class="info">
                    <strong>Domicilio</strong>
                    <label class="_OrganicaDomicilio"></label>
                </div>
            </div>
        </div>
    </div>


    <%-- Acciones --%>
    <div class="contenedor-botones enter">
        <div>
            <a class="btn btn-transparent btn-reenviar waves-effect">Reenviar comprobante por e-mail</a>
            <a class="btn btn-transparent btn-cancelar waves-effect">Cancelar requerimiento</a>
            <a class="btn btn-transparent btn-volver waves-effect">Volver</a>
        </div>
    </div>


    <%-- Templantes --%>
    <div id="template_DialogoUbicacion" style="display: none">
        <div class="contenedor_Mapa">
        </div>
    </div>

    <div id="template_Imagen" style="display: none">
        <div class="content-imagen waves-effect">
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="visor_Foto">
        <div class="fondo"></div>
        <div class="foto"></div>
        <a class="btn btn-round waves-effect"><i class="mdi mdi-close"></i></a>
    </div>
</asp:Content>
