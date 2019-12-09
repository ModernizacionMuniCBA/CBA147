<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMotivoNuevo.aspx.cs" Inherits="UI.IFrame.IMotivoNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMotivoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">




    <div style="display: block; height: 100%" class="scroll">

        <div id="contenedor_General">

            <div id="contenedor_Form">


                <div id="contenedor_Encabezado">

                    <div class="row">
                        <div class="col s12">
                            <div class="mi-input-field">
                                <label class="no-select">Área</label>
                                <select id="select_Area" style="width: 100%"></select>
                            </div>
                        </div>
                        <div class="col s12">
                            <div class="mi-input-field">
                                <label class="no-select">Servicio</label>
                                <select id="select_Servicio" style="width: 100%"></select>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12">
                            <div class="input-field">
                                <input type="text" id="input_Nombre" />
                                <label for="input_Nombre">Nombre</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="mi-input-field col s10">
                            <label class="no-select">Categoría</label>
                            <select id="select_Categoria" style="width: 100%"></select>
                        </div>
                        <div id="contenedor_BotonNuevaCategoria" class="col s2">
                            <a id="btnNuevaCategoria" class="btn  waves-effect btn-cuadrado tooltipped" data-position="bottom" data-delay="50" data-tooltip="Configurar categorías"><i class="material-icons">add</i></a>
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
                                <input type="text" id="input_Keywords" />
                                <label for="input_Keywords">Keywords</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <label>Tipo</label>
                        <div class="col s12" id="contenedor_tipos">
                            <p>
                                <input name="grupo_tipo" type="radio" id="radio_TipoGeneral" class="with-gap" checked/>
                                <label for="radio_TipoGeneral">General</label>
                            </p>
                            <p>
                                <input name="grupo_tipo" type="radio" id="radio_TipoInterno" class="with-gap" />
                                <label for="radio_TipoInterno">Interno</label>
                            </p>
                            <p>
                                <input name="grupo_tipo" type="radio" id="radio_TipoPrivado" class="with-gap" />
                                <label for="radio_TipoPrivado">Privado</label>
                            </p>
                        </div>
                    </div>

                    <div class="row" style="display: none">
                        <div class="col s12">
                            <p>
                                <input type="checkbox" id="check_Principal" />
                                <label for="check_Principal">Destacado</label>
                            </p>
                        </div>
                    </div>

                    <div class="row">
                        <label>Criticidad</label>
                        <div class="col s12" id="contenedor_prioridades">
                            <p>
                                <input name="grupo_prioridad" type="radio" id="radio_PrioridadNormal" class="with-gap" checked />
                                <label for="radio_PrioridadNormal">Normal</label>
                            </p>
                            <p>
                                <input name="grupo_prioridad" type="radio" id="radio_PrioridadMedia" class="with-gap" />
                                <label for="radio_PrioridadMedia">Media</label>
                            </p>
                            <p>
                                <input name="grupo_prioridad" type="radio" id="radio_PrioridadAlta" class="with-gap" />
                                <label for="radio_PrioridadAlta">Alta</label>
                            </p>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12">
                            <div class="mi-input-field">
                                <label class="no-select">Esfuerzo</label>
                                <select id="select_Esfuerzo" style="width: 100%"></select>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12">
                            <p>
                                <input type="checkbox" id="check_Urgente" />
                                <label for="check_Urgente">Peligroso</label>
                            </p>
                        </div>
                    </div>
                </div>

            </div>

            <div id="contenedor_FormTabla">
                <div class="encabezado">
                    <label>Campos dinámicos</label>
                    <a class="btn" id="btnAgregarCampo"><i class="material-icons btn-icono">add</i> Nuevo</a>
                </div>
                <div id="contenedor_InputBusqueda">

                    <div class="input-field">
                        <input type="text" id="input_BusquedaCampos" />
                        <label for="input_BusquedaCampos">Buscar...</label>
                    </div>
                </div>

                <div class="contenedor_Tabla">

                    <div class="tabla-contenedor">
                        <table id="tablaCamposDinamicos"></table>
                    </div>

                    <div class="contenedor-footer separador">
                        <div class="flex direction-horizontal">
                            <div class="tabla-footer flex-main">
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMotivoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
