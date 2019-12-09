<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="CambiarPass.aspx.cs" Inherits="UI.CambiarPass" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/CambiarPass.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical  full-screen no-scroll">

        <!-- Card Formulario -->
        <div id="cardFormulario" class="card contenedor flex flex-main direction-vertical">

            <div class="contenedor-main">
                <div class="row margin-top">
                    <div class="col s12 m6 l4">
                        <div class="input-field">
                            <input id="input_PassNueva" type="password" autocomplete="new-password" />
                            <label for="input_PassNueva" class="no-select">Contraseña nueva</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col s12 m6 l4">
                        <div class="input-field">
                            <input id="input_PassNueva2" type="password" autocomplete="new-password" />
                            <label for="input_PassNueva2" class="no-select">Reingrese la contraseña</label>
                            <a class="control-observacion colorTextoError no-select" style="display: none"></a>
                        </div>
                    </div>
                </div>


            </div>

            <!-- Footer -->
            <div class="contenedor-footer separador">
                <div class="contenedor-botones">
                    <a class="btnCambiarPass colorExito btn waves-effect no-select">
                        <i class="material-icons btn-icono">vpn_key</i>
                        Cambiar contraseña
                    </a>
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
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/CambiarPass.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
