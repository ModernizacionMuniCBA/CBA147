<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="UsuarioConsulta.aspx.cs" Inherits="UI.UsuarioConsulta" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/UsuarioConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="flex direction-vertical no-scroll full-height">

        <!-- Card Filtros -->
        <div id="cardFormularioFiltros" class="card contenedor">

            <div class="contenedor-main">

                <div class="row">
                    <div class="col s12 m6 l2">
                        <div class="input-field">
                            <input id="input_Nombre" name="nombre" type="text" maxlength="200" required="" aria-required="true" />
                            <label for="input_Nombre" class="no-select">Nombre</label>
                        </div>
                    </div>
                    <div class="col s12 m6 l2">
                        <div class="input-field">
                            <input id="input_Apellido" name="apellido" type="text" maxlength="200" required="" aria-required="true" />
                            <label for="input_Apellido" class="no-select">Apellido</label>
                        </div>
                    </div>
                                <div class="col s12 m6 l2">
                        <div class="input-field">
                            <input id="input_Username" name="apellido" type="text" maxlength="200" required="" aria-required="true" />
                            <label for="input_Username" class="no-select">Usuario</label>
                        </div>
                    </div>
                    <div class="col s12 m6 l2">
                        <div class="input-field">
                            <input id="input_Dni" name="dni" type="text" maxlength="8" required="" aria-required="true" />
                            <label for="input_Dni" class="no-select">N° Documento</label>
                        </div>
                    </div>
                    <div class="col s12 m6 l2">
                        <div class="input-field">
                            <input id="input_Email" class="mail" type="text" maxlength="100" />
                            <label for="input_Email" class="no-select">Email</label>
                        </div>
                    </div>
                    <div class="col s12 m6 l2">                     
                        <a id="btnBuscar" class="btn waves-effect colorExito"><i class="material-icons btn-icono">search</i>Buscar</a>
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

        <!-- Grd -->
        <div id="contenedorResultadoUsuarios" class="flex-main no-scroll flex direction-vertical full-height">

            <div id="cardResultadoUsuarios" class="card contenedor flex direction-vertical full-height">

                <div class="contenedor-main no-padding flex direction-vertical full-height">
                    <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                        <table id="tabla"></table>
                    </div>

                </div>

                <div class="contenedor-footer separador">
                        <div class="flex direction-horizontal">
                            <div class="tabla-footer flex-main">
                            </div>

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


        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/UsuarioConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
