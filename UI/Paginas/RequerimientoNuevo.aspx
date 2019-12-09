<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoNuevo.aspx.cs" Inherits="UI.RequerimientoNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Alertas -->
        <div class="contenedor-alertas">

            <!-- Alerta Ok -->
            <div id="alertaOk" class="container" style="display: none;">
                <div class="contenedor-alerta card">
                    <i class="icono material-icons colorExitoTexto no-select">check_circle</i>
                    <div class="textos">
                        <label class="titulo no-select">La operación se completo correctamente</label>
                        <div id="contenedorNumeroReclamo" class="flex direction-horizontal center-align">
                            <label class="titulo">Requerimiento N°</label>
                            <label id="textoNumeroReclamo" class="subtitulo"></label>
                        </div>
                        <div class="mail">
                            <i class="material-icons">email</i>
                            <label class="detalle">Mail enviado correctamente</label>
                        </div>
                    </div>
                    <div class="contenedor-botones">
                        <a class="btn waves-effect waves-light no-select" id="btnImprimirRequerimiento"><i class="btn-icono material-icons">print</i>Imprimir</a>
                        <a class="btn waves-effect waves-light no-select" id="btnEnviarMail"><i class="btn-icono material-icons">email</i>Reenviar E-Mail</a>
                        <a class="btn waves-effect waves-light no-select colorExito" id="btnNuevoRequerimiento"><i class="btn-icono material-icons">add</i>Nuevo</a>
                    </div>
                </div>
            </div>
        </div>

        <!-- Formulario -->
        <div id="contenedorFormulario" class="flex-main no-scroll flex direction-vertical">
            <!-- Card Formulario -->
            <div id="cardFormulario" class="card contenedor  flex flex-main direction-vertical">

                <div class="contenedor-main  no-margin  no-padding flex direction-vertical">
                    <iframe id="iframe" class="flex-main" src="IFrame/IRequerimientoNuevo.aspx" style="width: 100%; border: none;"></iframe>
                </div>

                <!-- Footer -->
                <div class="contenedor-footer separador">
                    <div class="contenedor-botones  row">
                        <a id="btnAgregarNota" class="btn waves-effect waves-light" style="display:none"><i class="material-icons btn-icono">add</i>Agregar nota</a>
                        <a id="btnLimpiar" class="btn waves-effect waves-light">
                            <i class="material-icons btn-icono">clear</i>
                            Limpiar</a>
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
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>


</asp:Content>
