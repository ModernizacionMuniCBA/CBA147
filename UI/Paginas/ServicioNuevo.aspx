<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="ServicioNuevo.aspx.cs" Inherits="UI.ServicioNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/ServicioNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical full-height no-scroll">
        <!-- Alertas -->
        <div class="contenedor-alertas">

            <!-- Alerta Ok -->
            <div id="alertaOk" class="container" style="display: none;">
                <div class="contenedor-alerta card">
                    <i class="icono material-icons colorTextoExito no-select">check_circle</i>
                    <div class="textos">
                        <label class="titulo no-select"></label>
                        <label class="detalle"></label>
                    </div>
                    <div class="contenedor-botones">
                        <a class="btn waves-effect waves-light no-select colorExito" id="btnNuevoServicio">Nuevo</a>
                    </div>
                </div>
            </div>

            <!-- Alerta Error Fatal -->
            <div id="alertaErrorFatal" class="container" style="display: none;">
                <div class="contenedor-alerta card">
                    <i class="icono material-icons colorTextoError no-select">error</i>
                    <div class="textos">
                        <label class="titulo no-select"></label>
                        <label class="detalle"></label>

                    </div>
                    <div class="contenedor-botones">
                        <a class="btn waves-effect waves-light no-select btnIrInicio">Ir a inicio</a>
                        <a class="btn waves-effect waves-light no-select btnIrConsulta">Ir a consulta</a>
                    </div>
                </div>
            </div>

            <!-- Alerta Error -->
            <div id="alertaError" class="container" style="display: none;">
                <div class="contenedor-alerta card">
                    <i class="icono material-icons colorTextoError no-select">error</i>
                    <div class="textos">
                        <label class="titulo no-select"></label>
                        <label class="detalle"></label>

                    </div>
                    <i class="boton waves-effect no-select" onclick="ocultarError()">close</i>
                </div>
            </div>
        </div>

        <!-- Card Formulario -->
        <div id="cardFormulario" class="card contenedor flex flex-main direction-vertical">

            <div class="contenedor-main flex no-padding no-margin">
                <iframe class="flex-main" id="iframeRegistrar" src="IFrame/IServicioNuevo.aspx" style="width: 100%; border: none;"></iframe>
            </div>

            <!-- Footer -->
            <div class="contenedor-footer separador">
                <div class="contenedor-botones  row">
                    <a class="btn waves-effect waves-light colorExito btnOk"><i class="material-icons btn-icono">save</i>Registrar</a>
                </div>
            </div>

            <!-- Cargando -->
            <div class="cargando" style="display: none">
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
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/ServicioNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
